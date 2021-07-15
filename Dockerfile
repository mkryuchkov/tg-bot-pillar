# Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY *.sln ./
COPY TgBotPillar.Common/*.csproj ./TgBotPillar.Common/
COPY TgBotPillar.Core/*.csproj ./TgBotPillar.Core/
COPY TgBotPillar.Storage/*.csproj ./TgBotPillar.Storage/
COPY TgBotPillar.StateProcessor/*.csproj ./TgBotPillar.StateProcessor/
COPY TgBotPillar.StateProcessor.Tests/*.csproj ./TgBotPillar.StateProcessor.Tests/
COPY TgBotPillar.Bot/*.csproj ./TgBotPillar.Bot/
COPY TgBotPillar.Api/*.csproj ./TgBotPillar.Api/

RUN dotnet restore

COPY . .

WORKDIR /app/TgBotPillar.Api
RUN dotnet build -c Release -o /out --no-restore

# Publish
FROM build AS publish
RUN dotnet publish -c Release -o /out --no-restore

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
LABEL org.opencontainers.image.source https://github.com/mkryuchkov/tg-bot-pillar
WORKDIR /app
EXPOSE 8443
COPY --from=publish /out .
ENTRYPOINT ["dotnet", "TgBotPillar.Api.dll"]