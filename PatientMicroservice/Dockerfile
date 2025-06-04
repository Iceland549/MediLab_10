# Étape 1 : build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PatientMicroservice/PatientMicroservice.csproj", "PatientMicroservice/"]
RUN dotnet restore "PatientMicroservice/PatientMicroservice.csproj"
COPY . .
RUN dotnet publish "PatientMicroservice/PatientMicroservice.csproj" -c Release -o /app/publish

# Étape 2 : runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5002
ENTRYPOINT ["dotnet", "PatientMicroservice.dll"]