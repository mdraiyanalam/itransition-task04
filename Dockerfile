# Stage 1: Build React Frontend
FROM node:20 AS frontend-build
WORKDIR /src/ClientApp
COPY ClientApp/package*.json ./
RUN npm ci --only=production
COPY ClientApp/ ./
RUN npm run build

# Stage 2: Build .NET Backend with Node.js
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js 20 (lighter way)
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

COPY ["task04UserManagement.csproj", "."]
RUN dotnet restore "task04UserManagement.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "task04UserManagement.csproj" -c Release -o /app/build

# Stage 3: Publish .NET
FROM build AS publish
RUN dotnet publish "task04UserManagement.csproj" -c Release -o /app/publish

# Stage 4: Final Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=frontend-build /src/ClientApp/dist ./wwwroot

EXPOSE 8080
ENTRYPOINT ["dotnet", "task04UserManagement.dll"]