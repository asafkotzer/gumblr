using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Gumblr.Helpers;

namespace Gumblr.Storage.Azure
{
    public interface ITableProvider 
    {
        Task Create(string aPartitionKey, string aRowKey, TableEntity aEntity);
        Task<TableEntity> GetRow(string aPartitionKey, string aRowKey);
        Task<IEnumerable<TableEntity>> GetPartition(string aPartition);
        Task Update(string aPartitionKey, string aRowKey, TableEntity aEntity);
        Task CreateOrUpdate(string aPartitionKey, string aRowKey, TableEntity aEntity);
        Task Delete(string aPartitionKey, string aRowKey);
    }

    public class TableStorageProvider : ITableProvider
    {
        static readonly string LessThanSuffix = "~";
        CloudStorageAccount mStorageAccount;
        CloudTable mCloudTableReference;
        CloudTableClient mTableClient;
        string mTableName;

        public delegate TableStorageProvider Factory(IConfigurationRetriever aConfigurationRetriever, string aTableName);

        public TableStorageProvider(IConfigurationRetriever aConfigurationRetriever, string aTableName)
        {
            mStorageAccount = CloudStorageAccount.Parse(aConfigurationRetriever.GetSetting("StorageConnectionString"));
            mTableClient = mStorageAccount.CreateCloudTableClient();
            mTableName = aTableName;
        }

        public async Task Create(string aPartitionKey, string aRowKey, TableEntity aEntity)
        {
            var table = await GetTableReference();

            var dynamicTableEntity = ConvertEntity(aEntity, aPartitionKey, aRowKey);
            var operation = TableOperation.Insert(dynamicTableEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task<TableEntity> GetRow(string aPartitionKey, string aRowKey)
        {
            var table = await GetTableReference();
            var operation = TableOperation.Retrieve<DynamicTableEntity>(aPartitionKey, aRowKey);
            var result = await table.ExecuteAsync(operation);

            var entity = result.Result as DynamicTableEntity;
            return ConvertEntity(entity);
        }

        public async Task<IEnumerable<TableEntity>> GetPartition(string aPartitionKey)
        {
            var table = await GetTableReference();
            var rangeQuery = new TableQuery<DynamicTableEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, aPartitionKey));

            var result = new List<TableEntity>();
            TableContinuationToken token = null;
            do
            {
                var segment = await table.ExecuteQuerySegmentedAsync<DynamicTableEntity>(rangeQuery, token);
                token = segment.ContinuationToken;
                foreach (var entity in segment)
                {
                    result.Add(ConvertEntity(entity));
                }
            } while (token != null);

            return result;
        }

        public async Task Update(string aPartitionKey, string aRowKey, TableEntity aEntity)
        {
            var table = await GetTableReference();
            var dynamicTableEntity = ConvertEntity(aEntity, aPartitionKey, aRowKey);
            var operation = TableOperation.Replace(dynamicTableEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task CreateOrUpdate(string aPartitionKey, string aRowKey, TableEntity aEntity)
        {
            var table = await GetTableReference();
            var dynamicTableEntity = ConvertEntity(aEntity, aPartitionKey, aRowKey);
            var operation = TableOperation.InsertOrReplace(dynamicTableEntity);
            await table.ExecuteAsync(operation);
        }

        public async Task Delete(string aPartitionKey, string aRowKey)
        {
            var table = await GetTableReference();
            var dynamicTableEntity = ConvertEntity(new TableEntity(), aPartitionKey, aRowKey);
            var operation = TableOperation.Delete(dynamicTableEntity);
            await table.ExecuteAsync(operation);
        }

        private async Task<CloudTable> GetTableReference()
        {
            if (mCloudTableReference == null)
            {
                mCloudTableReference = mTableClient.GetTableReference(mTableName);
                await mCloudTableReference.CreateIfNotExistsAsync();
            }

            return mCloudTableReference;
        }

        private EntityProperty CreateEntityProperty(object aFieldValue)
        {
            EntityProperty property;
            if (aFieldValue is bool) { property = new EntityProperty(aFieldValue as bool?); }
            else if (aFieldValue is byte[]) { property = new EntityProperty(aFieldValue as byte[]); }
            else if (aFieldValue is DateTime?) { property = new EntityProperty(aFieldValue as DateTime?); }
            else if (aFieldValue is DateTimeOffset?) { property = new EntityProperty(aFieldValue as DateTimeOffset?); }
            else if (aFieldValue is double?) { property = new EntityProperty(aFieldValue as double?); }
            else if (aFieldValue is Guid?) { property = new EntityProperty(aFieldValue as Guid?); }
            else if (aFieldValue is int?) { property = new EntityProperty(aFieldValue as int?); }
            else if (aFieldValue is long?) { property = new EntityProperty(aFieldValue as long?); }
            else { property = new EntityProperty(aFieldValue.ToString()); }

            return property;
        }

        private TableEntity ConvertEntity(DynamicTableEntity entity)
        {
            var content = entity.Properties.ToDictionary(x => x.Key, x => x.Value.PropertyAsObject);
            return new TableEntity { Properties = content };
        }

        private DynamicTableEntity ConvertEntity(TableEntity aEntity, string aPartitionKey, string aRowKey)
        {
            var properties = aEntity.Properties.Select(x => new { Key = x.Key, Value = CreateEntityProperty(x.Value) }).ToDictionary(x => x.Key, x => x.Value);
            var entity = new DynamicTableEntity(aPartitionKey, aRowKey) { Properties = properties };
            return entity;
        }
    }

    public class TableEntity
    {
        public Dictionary<string, object> Properties { get; set; }

        public TableEntity()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}