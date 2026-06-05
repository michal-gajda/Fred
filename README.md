# Fred Jonesz

<p align="center">
  <img src=".github/assets/logo.png" alt="Logo Projektu" width="200" height="200">
</p>

## BOM

- Docker for Desktop

## Client

```powershell
dotnet new classlib --framework net10.0 --output src/Shared --name Fred.Client.Shared
dotnet new webapi --framework net10.0 --no-https --use-controllers --use-program-main --output src/WebApi --name Fred.Client.WebApi
dotnet add src/WebApi reference src/Shared
dotnet sln add src/WebApi
dotnet add src/WebApi package Swashbuckle.AspNetCore.SwaggerUI
```

## Server
