#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Exchange.Rate.Ecb.Polling.Api/Exchange.Rates.Ecb.Polling.Api.csproj", "Exchange.Rate.Ecb.Polling.Api/"]
COPY ["Exchange.Rates.Core/Exchange.Rates.Core.csproj", "Exchange.Rates.Core/"]
COPY ["Exchange.Rate.Contracts/Exchange.Rates.Contracts.csproj", "Exchange.Rate.Contracts/"]
RUN dotnet restore "Exchange.Rate.Ecb.Polling.Api/Exchange.Rates.Ecb.Polling.Api.csproj"
COPY . .
WORKDIR "/src/Exchange.Rate.Ecb.Polling.Api"
RUN dotnet build "Exchange.Rates.Ecb.Polling.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exchange.Rates.Ecb.Polling.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exchange.Rates.Ecb.Polling.Api.dll"]