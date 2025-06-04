# Étape 1 : build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["NoteMicroservice/NoteMicroservice.csproj", "NoteMicroservice/"]
RUN dotnet restore "NoteMicroservice/NoteMicroservice.csproj"
COPY . .
RUN dotnet publish "NoteMicroservice/NoteMicroservice.csproj" -c Release -o /app/publish

# Étape 2 : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5003
ENTRYPOINT ["dotnet", "NoteMicroservice.dll"]
