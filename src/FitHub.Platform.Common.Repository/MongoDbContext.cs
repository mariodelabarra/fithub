using MongoDB.Entities;

namespace FitHub.Platform.Common.Repository
{
    public static class MongoDbContext
    {
        public static async Task Initialize(string databaseName, string mongoUrl)
        {
            await DB.InitAsync(databaseName, mongoUrl);
        }
    }
}
