#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["AlphaShopGestUserAPI/AlphaShopGestUserAPI.csproj", "AlphaShopGestUserAPI/"]
RUN dotnet restore "AlphaShopGestUserAPI/AlphaShopGestUserAPI.csproj"
COPY . .
WORKDIR "/src/AlphaShopGestUserAPI"
RUN dotnet build "AlphaShopGestUserAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlphaShopGestUserAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlphaShopGestUserAPI.dll"]