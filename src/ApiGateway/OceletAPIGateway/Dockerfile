#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/ApiGateway/OceletAPIGateway/OceletAPIGateway.csproj", "src/ApiGateway/OceletAPIGateway/"]
RUN dotnet restore "src/ApiGateway/OceletAPIGateway/OceletAPIGateway.csproj"
COPY . .
WORKDIR "/src/src/ApiGateway/OceletAPIGateway"
RUN dotnet build "OceletAPIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OceletAPIGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OceletAPIGateway.dll"]
