#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 8082
ENV ASPNETCORE_URLS=http://*:8082
ENV BS_DATASOURCE=LOCALHOST
ENV BS_PORT=3050
ENV BS_DATABASE=SIDI

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BlessSidi.Api/BlessSidi.Api.csproj", "BlessSidi.Api/"]
COPY ["BlessSidi.Application/BlessSidi.Application.csproj", "BlessSidi.Application/"]
COPY ["BlessSidi.Domain/BlessSidi.Domain.csproj", "BlessSidi.Domain/"]
COPY ["BlessSidi.Infra/BlessSidi.Infra.csproj", "BlessSidi.Infra/"]
COPY ["BlessSidi.Shared/BlessSidi.Shared.csproj", "BlessSidi.Shared/"]
RUN dotnet restore "./BlessSidi.Api/BlessSidi.Api.csproj"
COPY . .
WORKDIR "/src/BlessSidi.Api"
RUN dotnet build "./BlessSidi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BlessSidi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlessSidi.Api.dll"]