FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Shared/Fred.Client.Shared.csproj Shared/Fred.Client.Shared.csproj
COPY WebApi/Fred.Client.WebApi.csproj WebApi/Fred.Client.WebApi.csproj
RUN dotnet restore WebApi/Fred.Client.WebApi.csproj

COPY . .

RUN dotnet build WebApi/Fred.Client.WebApi.csproj --configuration Release
RUN dotnet publish WebApi/Fred.Client.WebApi.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0-azurelinux3.0
WORKDIR /app

USER $APP_UID

COPY --chown=$APP_UID --from=build /app/publish .

EXPOSE 5080
ENV ASPNETCORE_HTTP_PORTS=5080

HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl --fail http://localhost:5080/healthz || exit 1

ENTRYPOINT ["dotnet", "Fred.Client.WebApi.dll"]