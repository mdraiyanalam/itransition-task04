FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["task04UserManagement.csproj", "."]
RUN dotnet restore "task04UserManagement.csproj"

# Copy everything else
COPY . .

# Build
RUN dotnet build "task04UserManagement.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "task04UserManagement.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "task04UserManagement.dll"]