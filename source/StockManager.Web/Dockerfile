FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["source/StockManager.Web/StockManager.Web.csproj", "source/StockManager.Web/"]
RUN dotnet restore "source/StockManager.Web/StockManager.Web.csproj"
COPY . .
WORKDIR "/src/source/StockManager.Web"
RUN dotnet build "StockManager.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StockManager.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StockManager.Web.dll"]