# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore ProgressTrackerBot.Host/ProgressTrackerBot.Host.csproj
RUN dotnet publish ProgressTrackerBot.Host/ProgressTrackerBot.Host.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ProgressTrackerBot.Host.dll"]