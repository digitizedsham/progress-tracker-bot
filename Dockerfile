# -------------------------
# Build
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY . .

RUN dotnet restore ProgressTrackerBot.sln

# Публикуем основной проект
RUN dotnet publish ProgressTrackerBot.Host/ProgressTrackerBot.Host.csproj \
    -c Release \
    -o /app/publish/host \
    --no-restore

# Публикуем проект миграций
RUN dotnet publish ProgressTrackerBot.Migrations/ProgressTrackerBot.Migrations.csproj \
    -c Release \
    -o /app/publish/migrations \
    --no-restore


# -------------------------
# Runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

RUN apt-get update \
    && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app

COPY --from=build /app/publish/host ./host
COPY --from=build /app/publish/migrations ./migrations

ENTRYPOINT ["dotnet"]