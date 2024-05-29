FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 1026

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookService.csproj", "./"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Domains/Domains.csproj", "Domains/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Test.Core/Test.Core.csproj", "Test.Core/"]
RUN dotnet restore "BookService.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "BookService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BookService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY ["cert", "cert"]
ENTRYPOINT ["dotnet", "BookService.dll"]
