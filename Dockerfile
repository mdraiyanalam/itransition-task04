# Stage 1: Build React Frontend (Node 20+)
FROM node:20 AS frontend-build
WORKDIR /src/ClientApp
COPY ClientApp/package*.json ./
RUN npm install
COPY ClientApp/ ./
RUN npm run build

# Stage 2: Build .NET Backend
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
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