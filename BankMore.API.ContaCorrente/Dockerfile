
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BankMore.sln", "./"]
COPY ["BankMore.API.ContaCorrente/BankMore.API.ContaCorrente.csproj", "./BankMore.API.ContaCorrente/"]
COPY ["BankMore.API.Transferencia/BankMore.API.Transferencia.csproj", "./BankMore.API.Transferencia/"]
COPY ["BankMore.API.Gateway/BankMore.API.Gateway.csproj", "./BankMore.API.Gateway/"]
COPY ["BankMore.Core/BankMore.Core.csproj", "./BankMore.Core/"]
COPY ["BankMore.Infra/BankMore.Infra.csproj", "./BankMore.Infra/"]
COPY ["BankMore.Tests/BankMore.Tests.csproj", "./BankMore.Tests/"]

RUN dotnet restore "BankMore.sln"

COPY . .
WORKDIR "/src/BankMore.API.ContaCorrente"
RUN dotnet build "BankMore.API.ContaCorrente.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankMore.API.ContaCorrente.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
RUN mkdir -p /app/data && chmod 777 /app/data
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankMore.API.ContaCorrente.dll"]
