using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NoteMicroservice.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NoteMicroservice.Infrastructure.Data
{
    public class NotesSeeder
    {
        private readonly IMongoCollection<Note> _notesCollection;

        public NotesSeeder(IOptions<MongoDbConfig> config)
        {
            var mongoClient = new MongoClient(config.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(config.Value.DatabaseName);
            _notesCollection = mongoDatabase.GetCollection<Note>(config.Value.NotesCollectionName);
        }

        public async Task SeedAsync()
        {
            var count = await _notesCollection.CountDocumentsAsync(FilterDefinition<Note>.Empty);
            if (count > 0) return;

            var notes = new List<Note>
            {
                // TestNone
                new Note
                {
                    PatientId = 1,
                    Content = "Le patient déclare qu'il se sent bien, Poids égal ou inférieur au poids recommandé",
                    DateCreated = DateTime.UtcNow,
                },

                // TestBorderline
                new Note
                {
                    PatientId = 2,
                    Content = "Le patient déclare qu'il ressent beaucoup de stress au travail, il se plaint également que son audition est anormale dernièrement",
                    DateCreated = DateTime.UtcNow,
                },
                new Note
                {
                    PatientId = 2,
                    Content = "Le patient déclare avoir fait une réaction aux médicaments au cours des 3 derniers mois, il remarque que son audition continue d'être anormale",
                    DateCreated = DateTime.UtcNow,
                },

                // TestInDanger
                new Note
                {
                    PatientId = 3,
                    Content = "Le patient déclare qu'il fume depuis peu",
                    DateCreated = DateTime.UtcNow,
                },
                new Note
                {
                    PatientId = 3,
                    Content = "Le patient déclare qu'il est fumeur et qu'il a cessé de fumer l'année dernière, il se plaint également de crises d'apnée respiratoire anormales, Tests de laboratoire indiquant un taux de cholestérol LDL élevé",
                    DateCreated = DateTime.UtcNow,
                },

                // TestEarlyOnset
                new Note
                {
                    PatientId = 4,
                    Content = "Le patient déclare qu'il est devenu difficile de monter les escaliers, il se plaint également d'être essouflé, Test de laboratoire indiquant que les anticorps sont élevés, Réaction aux médicaments",
                    DateCreated = DateTime.UtcNow,
                },
                new Note
                {
                    PatientId = 4,
                    Content = "Le patient déclare qu'il a mal au dos lorsqu'il reste assis pendant longtemps",
                    DateCreated = DateTime.UtcNow,
                },
                new Note
                {
                    PatientId = 4,
                    Content = "Le patient déclare avoir commencé à fumer depuis peu, Hémoglobine A1C supérieur au niveau recommandé",
                    DateCreated = DateTime.UtcNow,
                },
                new Note
                {
                    PatientId = 4,
                    Content = "Taille, Poids, Cholestérol, Vertige et Réaction",
                    DateCreated = DateTime.UtcNow,
                }
            };
            await _notesCollection.InsertManyAsync(notes);
        }
    }
}
