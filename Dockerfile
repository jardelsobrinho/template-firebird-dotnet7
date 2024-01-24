FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /api-bless-sidi

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /api-bless-sidi
COPY --from=build-env /api-bless-sidi/publish .
ENTRYPOINT ["dotnet", "BlessSidi.Api.dll"]
EXPOSE 8082
ENV ASPNETCORE_URLS=http://*:8082
ENV BS_DATASOURCE=LOCALHOST
ENV BS_PORT=3050
ENV BS_DATABASE=SIDI
