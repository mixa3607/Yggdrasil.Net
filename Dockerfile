FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build_server
COPY ./server-src/ .
ARG COMMIT_SHA=none
ARG COMMIT_REF_NAME=none
RUN dotnet restore && dotnet build -c Release --no-restore
RUN dotnet publish -c Release --no-build -o /yggdrasil ArkProjects.Minecraft.YggdrasilApi

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS app
WORKDIR /app
ARG COMMIT_SHA=none
ARG COMMIT_REF_NAME=none
COPY --from=build_server /yggdrasil /app
EXPOSE 80
ENTRYPOINT ["dotnet", "ArkProjects.Minecraft.YggdrasilApi.dll"]
