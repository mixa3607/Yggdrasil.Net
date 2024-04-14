using System.Reflection;
using System.Text.Encodings.Web;
using ArkProjects.Minecraft.AspShared.EntityFramework;
using ArkProjects.Minecraft.AspShared.Logging;
using ArkProjects.Minecraft.AspShared.Validation;
using ArkProjects.Minecraft.Database;
using ArkProjects.Minecraft.YggdrasilApi.Misc;
using ArkProjects.Minecraft.YggdrasilApi.Misc.JsonConverters;
using ArkProjects.Minecraft.YggdrasilApi.Options;
using ArkProjects.Minecraft.YggdrasilApi.Services;
using ArkProjects.Minecraft.YggdrasilApi.Services.Server;
using ArkProjects.Minecraft.YggdrasilApi.Services.User;
using ArkProjects.Minecraft.YggdrasilApi.Services.UserPassword;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Application: {builder.Environment.ApplicationName}");
Console.WriteLine($"ContentRoot: {builder.Environment.ContentRootPath}");

//logging
builder.Services.ConfigureRbSerilog(builder.Configuration.GetSection("Serilog"));
builder.Host.AddRbSerilog();

builder.Services
    .AddSingleton<IUserPasswordService, UserPasswordService>()
    .AddScoped<IYgServerService, YgServerService>()
    .AddScoped<IYgUserService, YgUserService>();

//security
var securityOptions = builder.Configuration.GetSection("WebSecurity").Get<WebSecurityOptions>()!;
builder.Services
    .Configure<WebSecurityOptions>(builder.Configuration.GetSection("WebSecurity"))
    ;

var dbOptions = builder.Configuration.GetSection("Database:ConnectionString").Value!;
var dbSourceBuilder = new NpgsqlDataSourceBuilder(dbOptions);
var dbSource = dbSourceBuilder.Build();
builder.Services
    .AddSingleton<IDbSeeder<McDbContext>, McDbContextSeeder>()
    .AddRbDbMigrator<McDbContext>()
    .AddDbContext<McDbContext>(x =>
        x.UseNpgsql(dbSource, y => y
            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
        )
    );

//controllers
builder.Services
    .Configure<ApiBehaviorOptions>(opts => { opts.SuppressConsumesConstraintForFormFileParameters = true; })
    .AddControllers()
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.ContractResolver = new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };
        o.SerializerSettings.Converters.Add(new YggdrasilGuidConverter());
    })
    ;

//swagger
var swaggerOptions = builder.Configuration.GetSection("Swagger").Get<SwaggerOptions>();
builder.Services
    .Configure<SwaggerOptions>(builder.Configuration.GetSection("Swagger"))
    .AddSwaggerGenNewtonsoftSupport()
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(o =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    })
    ;

//#########################################################################


var app = builder.Build();

//forwarded headers
if (securityOptions.EnableForwardedHeaders)
{
    var forwardOptions = new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
        ForwardLimit = 10,
        RequireHeaderSymmetry = false
    };
    forwardOptions.KnownNetworks.Clear();
    forwardOptions.KnownProxies.Clear();
    app.UseForwardedHeaders(forwardOptions);
}

app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (Exception e)
    {
        var err = e is YgServerException ygE
            ? ygE.Response
            : ErrorResponseFactory.Custom(
                StatusCodes.Status500InternalServerError,
                ErrorResponseFactory.ErrorInternalServerError,
                e.ToString());
        var jsonHelper = context.RequestServices.GetRequiredService<IJsonHelper>();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = err.StatusCode;
        var respStream = context.Response.BodyWriter.AsStream();
        await using var textWriter = new StreamWriter(respStream);
        jsonHelper.Serialize(err).WriteTo(textWriter, HtmlEncoder.Default);
    }
});

//logging
app.UseRbSerilogRequestLogging();

// https redirection
if (securityOptions.EnableHttpsRedirections)
{
    app.UseHttpsRedirection();
}

//swagger
if (swaggerOptions?.Enable == true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//controllers
app.MapControllers();


//#########################################################################


app.Services
    .RbCheckTz()
    .RbCheckLocale()
    ;

await app.Services.RbEfMigrateAsync<McDbContext>();
await app.RunAsync();