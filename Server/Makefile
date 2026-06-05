build:
	dotnet build src/WebApi/Fred.Server.WebApi.csproj

image:
	docker build -t fred-server-webapi:latest -f src/Dockerfile src

run:
	dotnet run --project src/WebApi/Fred.Server.WebApi.csproj
