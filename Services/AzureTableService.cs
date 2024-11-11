using Azure;
using Azure.Data.Tables;

namespace ABCRetailWebApplication.Services
{
    public class AzureTableService
    {
        private readonly TableServiceClient _tableServiceClient;
        private readonly string _tableName;

        public AzureTableService(string connectionString, string tableName)
        {
            _tableServiceClient = new TableServiceClient(connectionString);
            _tableName = tableName;
            _tableServiceClient.CreateTableIfNotExists(_tableName); // Create the table if it doesn't exist
        }


        public async Task<List<T>> GetAllEntities<T>() where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            var entities = new List<T>();

            await foreach (var entity in tableClient.QueryAsync<T>())
            {
                entities.Add(entity);
            }

            return entities;
        }

        public async Task<T> GetEntity<T>(string id) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            return await tableClient.GetEntityAsync<T>("PartitionKey", id);
        }

        public async Task AddEntity<T>(T entity) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.AddEntityAsync(entity);
        }

        public async Task UpdateEntity<T>(T entity) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.UpdateEntityAsync(entity, ETag.All);
        }

        public async Task DeleteEntity<T>(string id) where T : class, ITableEntity, new()
        {
            var tableClient = _tableServiceClient.GetTableClient(_tableName);
            await tableClient.DeleteEntityAsync("PartitionKey", id);
        }
    }
}
