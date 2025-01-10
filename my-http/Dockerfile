FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 3232
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["my-http/MyHttpServer.csproj", "my-http/"]
COPY ["HttpServerLibrary/HttpServerLibrary.csproj", "HttpServerLibrary/"]
COPY ["MyORMLibrary/MyORMLibrary.csproj", "MyORMLibrary/"]
COPY ["TemlateEngine/TemlateEngine.csproj", "TemlateEngine/"]
RUN dotnet restore "my-http/MyHttpServer.csproj"
COPY . .
WORKDIR "/src/my-http"
RUN dotnet build "MyHttpServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MyHttpServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHttpServer.dll"]
