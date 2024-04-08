FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID 


WORKDIR /app    
EXPOSE 442
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Configuration/Configuration.csproj", "Configuration/"]
COPY ["./Core/Core.csproj", "Core/"]
COPY ["./Domains/Domains.csproj", "Domains/"]
COPY ["./Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "Configuration/Configuration.csproj"
COPY . .
WORKDIR "/src/Configuration"
RUN dotnet build "Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Configuration.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["./cert", "cert"]
ENTRYPOINT ["dotnet", "Configuration.dll"]
