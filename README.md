# MediLab P10 – Application de Détection du Risque de Diabète

Cette solution se compose de plusieurs microservices .NET, d’un Frontend React et d’un orchestrateur Docker Compose.  
Elle permet de gérer des patients, des notes médicales, d’authentifier des utilisateurs et de calculer un niveau de risque de diabète en fonction des données stockées.



## Table des matières

1. Structure du projet
2. Prérequis  
3. Configuration des microservices  
   - AuthMicroservice  
   - PatientMicroservice  
   - NoteMicroservice 
   - RiskMicroservice  
   - ApiGateway (Ocelot) 
   - frontend_medilab (React)
4. Lancement global avec Docker Compose
5. Variables d’environnement  
6. Exécution des tests unitaires (optionnel)  
7. Persistance des clés Data Protection 


## Structure du projet

```

/iceland549-medilab\_10/
├── AuthMicroservice/
│   ├── AuthMicroservice.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Dockerfile
│   ├── Infrastructure/Data/ (AuthDbContext.cs, AuthDbInitializer.cs)
│   ├── Infrastructure/Repositories/ (IUserRepository.cs, UserRepository.cs)
│   ├── Application/DTOs/ (LoginDto.cs, UserDto.cs)
│   ├── Application/Interfaces/ (IAuthService.cs)
│   ├── Application/Services/ (AuthService.cs)
│   └── Presentation/Controllers/ (AuthController.cs)
│
├── PatientMicroservice/
│   ├── PatientMicroservice.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Dockerfile
│   ├── Infrastructure/Data/ (PatientDbContext.cs)
│   ├── Infrastructure/Repositories/ (IPatientRepository.cs, PatientRepository.cs)
│   ├── Application/DTOs/ (PatientDto.cs)
│   ├── Application/Interfaces/ (IPatientService.cs)
│   ├── Application/Services/ (PatientService.cs)
│   └── Presentation/Controllers/ (PatientsController.cs)
│
├── NoteMicroservice/
│   ├── NoteMicroservice.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Dockerfile
│   ├── Infrastructure/Data/ (MongoDbConfig.cs, NotesSeeder.cs)
│   ├── Infrastructure/Repositories/ (INoteRepository.cs, NoteRepository.cs)
│   ├── Application/DTOs/ (NoteDto.cs)
│   ├── Application/Interfaces/ (INoteService.cs)
│   ├── Application/Services/ (NoteService.cs)
│   └── Presentation/Controllers/ (NotesController.cs)
│
├── RiskMicroservice/
│   ├── RiskMicroservice.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Dockerfile
│   ├── Infrastructure/Clients/ (INoteClient.cs, IPatientClient.cs, NoteClient.cs, PatientClient.cs)
│   ├── Application/DTOs/ (NoteDto.cs, PatientDto.cs, RiskAssessmentDto.cs)
│   ├── Application/Interfaces/ (IRiskService.cs)
│   ├── Application/Services/ (RiskService.cs)
│   └── Presentation/Controllers/ (RiskController.cs)
│
├── ApiGateway/
│   ├── ApiGateway.csproj
│   ├── Program.cs
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── ocelot.json
│   ├── ocelot.Development.json
│   ├── Dockerfile
│   └── Controllers/ (WeatherForecastController.cs)
│
├── frontend\_medilab/
│   ├── package.json
│   ├── package-lock.json
│   ├── Dockerfile
│   ├── public/ (index.html, robots.txt, manifest.json)
│   └── src/
│       ├── App.js
│       ├── index.js
│       ├── components/ (Login.js, PatientsList.js, PatientDetails.js, NotesList.js, RiskLevel.js)
│       └── services/ (api.js)
│
└── docker-compose.yml

````


## Prérequis

- Docker Desktop (Windows/Mac).  
- .NET 8 SDK installé localement.  
- Node.js 14+ et npm (pour lancement ou tests locaux du frontend sans Docker).  
- MongoDB et SQL Server seront démarrés automatiquement par Docker Compose.  


## Configuration des microservices

### AuthMicroservice

- Objectif : gérer l’authentification (login) et générer des JWT.  
- Points clés :
  1. Lecture de la chaîne SQL Server (`ConnectionStrings:DefaultConnection`) dans `appsettings.json`.
  2. Registration d’Identity (UserManager / RoleManager).
  3. Lecture des variables d’environnement pour JWT (clé, issuer, audience).
  4. Migration automatique de la base (`Database.Migrate()`) et exécution du seed (création de l’utilisateur admin) via `AuthDbInitializer`.
  5. Healthcheck exposé sur `/api/auth/login` pour valider que le service est en ligne.
  6. Fichier **Dockerfile** générant une image multi-stage pour compiler et exécuter l’application en runtime uniquement.

### PatientMicroservice

- Objectif : exposer un CRUD sur la collection “Patients” dans SQL Server.  
- Points clés :
  1. Lecture de la chaîne SQL Server (`ConnectionStrings:DefaultConnection`).
  2. Registration d’Entity Framework Core (DbContext) et du service métier `PatientService`.
  3. Migration automatique au démarrage (`context.Database.Migrate()`).
  4. Healthcheck sur `/api/patients`.
  5. Dockerfile construisant l’application puis l’exécutant sur Kestrel.

### NoteMicroservice

- Objectif : exposer un CRUD sur la collection “Notes” dans MongoDB.  
- Points clés :
  1. Lecture des paramètres MongoDB (`MongoDbConfig`) depuis `appsettings.json` ou variables d’environnement.
  2. Registration du client `MongoClient` et du service métier `NoteService`.
  3. Exécution du seeding initial via `NotesSeeder.SeedAsync()` si la collection est vide.
  4. Healthcheck sur `/api/notes?patientId=1`.
  5. Dockerfile multi-stage (compile + runtime) hébergeant le service sur Kestrel.

### RiskMicroservice

- Objectif : calculer et exposer le niveau de risque en agrégeant les données de Patient et Note.  
- Points clés :
  1. Injection de deux `HttpClient` (via `AddHttpClient`) pointant vers PatientMicroservice et NoteMicroservice.
  2. Registration du service métier `RiskService`, qui appelle les clients HTTP pour récupérer les données.
  3. Santé du service vérifiée via `/api/risk/{patientId}`.
  4. Dockerfile multi-stage pour compiler et exécuter sur Kestrel.

### ApiGateway (Ocelot)

- Objectif : centraliser le routage, valider les JWT et gérer le CORS pour le frontend React.  
- Points clés :
  1. Chargement de la configuration Ocelot depuis `ocelot.json` ou `ocelot.Development.json` selon l’environnement (`ASPNETCORE_ENVIRONMENT`).
  2. Registration du schéma d’authentification JWT (mêmes paramètres clé/issuer/audience que AuthMicroservice).
  3. Enabling du CORS pour autoriser `http://localhost:3000`.
  4. Dockerfile multi-stage exposant le port 5000.

### frontend_medilab (React)

- Objectif : interface utilisateur Single-Page Application (login, affichage des patients, détails, notes, niveau de risque).  
- Points clés :
  1. Fichier `src/services/api.js` configurant Axios pour pointer vers `http://localhost:5000` (API Gateway).
  2. Intercepteur Axios ajoutant automatiquement le header `Authorization: Bearer <token>` depuis `localStorage`.
  3. Composants React gérant le cycle de vie (login, récupération des listes, affichage des notes et du risque).
  4. Dockerfile multi-stage : build Node.js ➔ production statique servie par Nginx sur le port 80.

---

## Lancement global avec Docker Compose

1. Ouvrir un terminal à la racine du dépôt (`/iceland549-medilab_10/`).  
2. Exécuter :
   ```bash
   docker-compose up -d
````

3. Docker Compose va :

   * Démarrer SQL Server et MongoDB (bases de données).
   * Construire puis lancer chaque microservice dans son conteneur (Auth, Patient, Note, Risk, ApiGateway, Frontend).
   * Appliquer automatiquement les migrations EF Core et les seeders (admin, notes de test).
   * Exposer les ports suivants :

     * **5001** → AuthMicroservice
     * **5002** → PatientMicroservice
     * **5003** → NoteMicroservice
     * **5004** → RiskMicroservice
     * **5000** → ApiGateway (Ocelot)
     * **3000** → Frontend React

4. Vérifier que tous les services sont “healthy” :

   ```bash
   docker ps
   ```

---

## Variables d’environnement 


* AuthMicroservice :

  * `ConnectionStrings__DefaultConnection` : chaîne de connexion SQL Server 
  * `Jwt__Key` : clé secrète (ex. `Qw3!t9z@LkP#5sV8...`)
  * `Jwt__Issuer` : émetteur (ex. `MediLabAuth`)
  * `Jwt__Audience` : audience (ex. `MediLabApiUsers`)
  * `ADMIN_USERNAME` : utilisateur admin (`admin` par défaut)
  * `ADMIN_PASSWORD` : mot de passe admin (`Admin123!` par défaut)

* PatientMicroservice :

  * `ConnectionStrings__DefaultConnection` : chaîne de connexion SQL Server 
  * `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` : mêmes valeurs que pour AuthMicroservice

* NoteMicroservice :

  * `MongoDb__ConnectionString` : URL MongoDB (ex. `mongodb://mongo:27017`)
  * `MongoDb__Database` : nom de la base (ex. `NotesDb`)
  * `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` : mêmes valeurs que pour AuthMicroservice

* RiskMicroservice :

  * `PatientMicroserviceBaseUrl` : URL du patient service (ex. `http://patient:5002`)
  * `NoteMicroserviceBaseUrl` : URL du note service (ex. `http://note:5003`)
  * `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` : mêmes valeurs que pour AuthMicroservice

* ApiGateway :

  * `Jwt__Key`, `Jwt__Issuer`, `Jwt__Audience` : mêmes valeurs
  * `ASPNETCORE_ENVIRONMENT=Production` pour forcer l’utilisation de `ocelot.json`

* frontend_medilab :

  * `npm install` puis `npm run build` en local si nécessaire.

---


## Exécution des tests unitaires

Les tests unitaires ont été développés pour les services critiques (ex. : `AuthService`, `RiskService`, etc.).
Ils peuvent être exécutés via l’explorateur de tests intégré de Visual Studio (Test Explorer) :

```bash
dotnet test
```

> Note : aucun `Dockerfile` n’a été créé pour les tests, ils sont exécutés localement lors du développement.


---

## Persistance des clés Data Protection

Pour que les clés de chiffrement ASP.NET Core (Data Protection) soient conservées entre redémarrages :

* Le conteneur AuthMicroservice monte le volume local `./keys` vers `/root/.aspnet/DataProtection-Keys`.
* Cela évite que les anciens JWT ou cookies chiffrés deviennent invalides à chaque redéploiement.


