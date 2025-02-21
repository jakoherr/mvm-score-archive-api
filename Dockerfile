FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG APP_NAME=mvm-score-archive-api
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_TOKEN

WORKDIR /src
COPY . .

RUN dotnet nuget add source \
    --username jakoherr \
    --password $GITHUB_TOKEN \
    --store-password-in-clear-text \
    --name github \
    "https://nuget.pkg.github.com/jakoherr/index.json"

RUN dotnet restore "$APP_NAME/$APP_NAME.csproj"
WORKDIR "/src/$APP_NAME"
RUN dotnet build "$APP_NAME.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "$APP_NAME.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "mvm-score-archive-api.dll"]