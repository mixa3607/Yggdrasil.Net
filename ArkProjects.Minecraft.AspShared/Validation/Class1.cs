using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Text;
using FluentValidation;

namespace ArkProjects.Minecraft.AspShared.Validation;
public static class ServiceProviderExtensions
{
    /// <inheritdoc cref="StartupValidatorsCheckHelper.CheckActionsValidators"/>
    public static IServiceProvider RbCheckActionsValidators(this IServiceProvider services, bool onlyOnDevEnv = true)
    {
        StartupValidatorsCheckHelper.CheckActionsValidators(services, onlyOnDevEnv);
        return services;
    }

    /// <inheritdoc cref="StartupValidatorsCheckHelper.CheckTz"/>
    public static IServiceProvider RbCheckTz(this IServiceProvider services, TimeSpan? requiredOffset = null)
    {
        StartupValidatorsCheckHelper.CheckTz(services, requiredOffset);
        return services;
    }

    /// <inheritdoc cref="StartupValidatorsCheckHelper.CheckLocale"/>
    public static IServiceProvider RbCheckLocale(this IServiceProvider services, CultureInfo? requiredCulture = null)
    {
        StartupValidatorsCheckHelper.CheckLocale(services, requiredCulture);
        return services;
    }
}
public class StartupValidatorsCheckHelper
{
    public static void CheckActionsValidators(IServiceProvider serviceProvider, bool onlyOnDevEnv = true)
    {
        var environment = serviceProvider.GetRequiredService<IHostEnvironment>();
        var logger = serviceProvider.GetRequiredService<ILogger<StartupValidatorsCheckHelper>>();
        if (onlyOnDevEnv && !environment.IsDevelopment())
        {
            logger.LogInformation("Skip validators check in non Development env");
            return;
        }

        var scope = serviceProvider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AllRequiredValidatorsRegisteredService>();
        service.CheckControllersParams();
    }

    /// <summary>
    /// Check that process run in required tz. Ignored in Development
    /// </summary>
    /// <param name="services"></param>
    /// <param name="requiredOffset">If null used +0</param>
    public static IServiceProvider CheckTz(IServiceProvider services, TimeSpan? requiredOffset = null)
    {
        var logger = services.GetRequiredService<ILogger<StartupValidatorsCheckHelper>>();
        requiredOffset ??= TimeSpan.FromHours(0);
        var tz = TimeZoneInfo.Local;

        if (tz.BaseUtcOffset == requiredOffset)
            return services;

        if (!services.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            logger.LogCritical("Detect TZ diff! Must run with utc+{h}", requiredOffset.Value.Hours);
            throw new Exception($"Detect TZ diff! Must run with utc+{requiredOffset.Value.Hours}");
        }

        logger.LogError("Detect TZ diff! Ignore bcs in Development env");
        return services;
    }

    /// <summary>
    /// Check that process run with required locale. Ignored in Development
    /// </summary>
    /// <param name="services"></param>
    /// <param name="requiredCulture">If null used Invariant</param>
    public static IServiceProvider CheckLocale(IServiceProvider services, CultureInfo? requiredCulture = null)
    {
        var logger = services.GetRequiredService<ILogger<StartupValidatorsCheckHelper>>();
        requiredCulture ??= CultureInfo.InvariantCulture;
        var culture = CultureInfo.CurrentCulture;

        if (culture.Equals(requiredCulture))
            return services;

        if (!services.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
        {
            logger.LogCritical("Detect locale mismatch! Must run with {locale} locale", requiredCulture.DisplayName);
            throw new Exception($"Detect locale mismatch! Must run with {requiredCulture.DisplayName} locale");
        }

        logger.LogError("Detect locale mismatch! Ignore bcs in Development env");
        return services;
    }
}
public class AllRequiredValidatorsRegisteredService
{
    private readonly IServiceProvider _services;

    public AllRequiredValidatorsRegisteredService(IServiceProvider services)
    {
        _services = services;
    }

    public IReadOnlyList<ControllerActionValidationResult> CheckControllersParams(params Assembly[] skipAssemblies)
    {
        var actionDescriptors = _services.GetRequiredService<IActionDescriptorCollectionProvider>().ActionDescriptors;
        var controllerDescriptors = actionDescriptors.Items
            .OfType<ControllerActionDescriptor>()
            .ToArray();

        var results = new List<ControllerActionValidationResult>();
        foreach (var descriptor in controllerDescriptors)
        {
            var paramResults = new List<ControllerActionValidationResult.ArgumentValidationInfo>();

            var actionParams = descriptor.Parameters
                .Where(x => x.BindingInfo?.BindingSource?.IsFromRequest == true)
                .Cast<ControllerParameterDescriptor>()
                .ToArray();
            foreach (var param in actionParams)
            {
                var info = new ControllerActionValidationResult.ArgumentValidationInfo() { ParameterInfo = param.ParameterInfo, };
                paramResults.Add(info);
                var skipAttr = param.ParameterInfo.GetCustomAttribute<SkipValidatorsCheckAttribute>();
                if (skipAttr != null)
                {
                    info.SkipType = ControllerActionValidationResult.ValidationSkipType.Attribute;
                    info.SkipReason = skipAttr.Reason;
                    continue;
                }

                var type = Nullable.GetUnderlyingType(param.ParameterType) ?? param.ParameterType;
                if (skipAssemblies.Any(x => x == type.Assembly))
                {
                    info.SkipType = ControllerActionValidationResult.ValidationSkipType.Assembly;
                    info.SkipReason = "Assembly skip";
                    continue;
                }

                var validatorRequired = (type.IsClass || type.IsValueType) &&
                                        !type.IsPrimitive &&
                                        !type.Namespace!.StartsWith("System.");
                if (!validatorRequired)
                {
                    info.SkipType = ControllerActionValidationResult.ValidationSkipType.MemberType;
                    info.SkipReason = $"Type {type.Name} always skip";
                    continue;
                }

                var validatorType = typeof(IValidator<>).MakeGenericType(type);
                var validator = _services.GetService(validatorType);
                info.ValidatorFound = validator != null;
            }

            results.Add(new ControllerActionValidationResult()
            {
                Arguments = paramResults,
                ControllerName = descriptor.ControllerName,
                ActionName = descriptor.ActionName,
            });
        }

        if (results
            .SelectMany(x => x.Arguments)
            .Any(x => x is { SkipType: ControllerActionValidationResult.ValidationSkipType.No, ValidatorFound: false }))
        {
            throw new HaveMissedValidatorsException(results);
        }

        return results;
    }
}
public class ControllerActionValidationResult
{
    public required string ControllerName { get; set; }
    public required string ActionName { get; set; }
    public IReadOnlyCollection<ArgumentValidationInfo> Arguments { get; set; } = Array.Empty<ArgumentValidationInfo>();

    public override string ToString()
    {
        return $"{ControllerName}.{ActionName}({string.Join(", ", Arguments)});";
    }

    public class ArgumentValidationInfo
    {
        public ValidationSkipType SkipType { get; set; }
        public string? SkipReason { get; set; }
        public required ParameterInfo ParameterInfo { get; set; }
        public bool ValidatorFound { get; set; }

        public override string ToString()
        {
            var inf = SkipType == ValidationSkipType.No
                ? ValidatorFound
                    ? "found"
                    : "miss"
                : "skip";
            return $"{ParameterInfo.Name}: {inf}";
        }
    }

    public enum ValidationSkipType
    {
        No,
        Attribute,
        MemberType,
        Assembly,
    }
}
public class HaveMissedValidatorsException : Exception
{
    public IReadOnlyList<ControllerActionValidationResult> Results { get; }

    public HaveMissedValidatorsException(IReadOnlyList<ControllerActionValidationResult> results) : base(
        BuildMessage(results))
    {
        Results = results;
    }

    private static string BuildMessage(IReadOnlyList<ControllerActionValidationResult> results)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Detect missing validators in controllers");
        foreach (var cav in results)
        {
            sb.AppendLine(cav.ToString());
        }

        return sb.ToString();
    }
}
[AttributeUsage(AttributeTargets.Parameter)]
public class SkipValidatorsCheckAttribute : Attribute
{
    public string Reason { get; }

    public SkipValidatorsCheckAttribute(string reason)
    {
        Reason = reason;
    }
}