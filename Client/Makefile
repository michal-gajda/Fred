build:
	dotnet build src/WebApi/Fred.Client.WebApi.csproj

image:
	docker build -t fred-client-webapi:latest -f src/Dockerfile src

run:
	dotnet run --project src/WebApi/Fred.Client.WebApi.csproj
