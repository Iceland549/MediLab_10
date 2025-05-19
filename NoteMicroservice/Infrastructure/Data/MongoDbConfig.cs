namespace NoteMicroservice.Infrastructure.Data
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string NotesCollectionName {  get; set; } = string.Empty; 
    }
}
