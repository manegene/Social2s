FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /source

# Copy full repo
COPY . .

WORKDIR /source/src

# Restore using REAL solution name
RUN dotnet restore Kmums.sln

RUN dotnet test Kmums.sln \
    --no-restore \
    --logger "console;verbosity=normal" \
    || true

# Publish using REAL project
RUN dotnet publish Social2s.csproj \
    -c Release \
    -o /app \
    --no-restore

# =========================
# RUNTIME STAGE
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "Social2s.dll"]