# Étape 1 : build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RiskMicroservice/RiskMicroservice.csproj", "RiskMicroservice/"]
RUN dotnet restore "RiskMicroservice/RiskMicroservice.csproj"
COPY . .
RUN dotnet publish "RiskMicroservice/RiskMicroservice.csproj" -c Release -o /app/publish

# Étape 2 : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5004
ENTRYPOINT ["dotnet", "RiskMicroservice.dll"]