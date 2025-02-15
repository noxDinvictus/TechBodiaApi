# Use .NET 8.0 ASP.NET runtime as the base layer
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use .NET 8.0 SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechBodiaApi.csproj", "."]
RUN dotnet restore "./TechBodiaApi.csproj"

COPY . .
WORKDIR "/src/"
RUN dotnet build "TechBodiaApi.csproj" -c Release -o /app/build

# Publish the app
FROM build AS publish
RUN dotnet publish "TechBodiaApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final runtime container
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechBodiaApi.dll"]
