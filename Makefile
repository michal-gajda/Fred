build:
	dotnet build Client/src/WebApi/Fred.Client.WebApi.csproj
	dotnet build Server/src/WebApi/Fred.Server.WebApi.csproj

client:
	dotnet build Client/src/WebApi/Fred.Client.WebApi.csproj

server:
	dotnet build Server/src/WebApi/Fred.Server.WebApi.csproj

image:
	docker compose build

kafka-up:
	docker compose -f docker-compose.kafka.yml up --detach

kafka-down:
	docker compose -f docker-compose.kafka.yml down

config:
	docker compose -f docker-compose.yml config

up:
	docker compose -f docker-compose.yml up --detach

down:
	docker compose -f docker-compose.yml down
