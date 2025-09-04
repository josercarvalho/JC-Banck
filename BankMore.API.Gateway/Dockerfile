
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BankMore.API.Gateway/BankMore.API.Gateway.csproj", "BankMore.API.Gateway/"]
RUN dotnet restore "BankMore.API.Gateway/BankMore.API.Gateway.csproj"
COPY . .
WORKDIR "/src/BankMore.API.Gateway"
RUN dotnet build "BankMore.API.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankMore.API.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankMore.API.Gateway.dll"]
