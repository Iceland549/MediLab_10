using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace NoteMicroservice.Domain.Models
{
    public class Note
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("PatientId")]
        public int PatientId { get; set; }

        [BsonElement("Content")]
        public string? Content { get; set; }

        [BsonElement("DateCreated")]
        public DateTime DateCreated { get; set; }
    }
}
