using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;

namespace Gumblr.Storage.Azure
{
    public class TableDataContext : IStorageProvider
    {
        static readonly string LessThanSuffix = "~";
        CloudStorageAccount mStorageAccount;
        CloudTableClient mTableClient;

        public TableDataContext()
        {
            mStorageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            mTableClient = mStorageAccount.CreateCloudTableClient();
        }

        public async Task Create(string aContainer, string aKey, ITableEntity aItem)
        {
            aItem.PartitionKey = aKey;
            aItem.RowKey = "";
            var table = await GetTableReference(aContainer);
            var operation = TableOperation.Insert(aItem);
            await table.ExecuteAsync(operation);
        }

        public async Task<T> ReadOne<T>(string aContainer, string aKey) where T : class, ITableEntity, new()
        {
            var table = await GetTableReference(aContainer);
            var operation = TableOperation.Retrieve<T>(aKey, "");
            var result = await table.ExecuteAsync(operation);
            return result.Result as T;
        }

        public async Task<IEnumerable<T>> Read<T>(string aContainer, string aPrefix) where T : class, ITableEntity, new()
        {
            var table = await GetTableReference(aContainer);
            TableQuery<T> rangeQuery = new TableQuery<T>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.GreaterThan, aPrefix),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.LessThan, aPrefix + LessThanSuffix)));


            //TODO: change to segmentedasync:
            var result = table.ExecuteQuery<T>(rangeQuery);
            return result;
        }

        public async Task Update(string aContainer, string aKey, ITableEntity aItem)
        {
            throw new NotImplementedException();
        }

        public async Task CreateOrUpdate(string aContainer, string aKey, ITableEntity aItem)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string aContainer, string aKey)
        {
            var entity = await ReadOne<TableEntity>(aContainer, aKey);
            
            var table = await GetTableReference(aContainer); 
            var operation = TableOperation.Delete(entity);
            await table.ExecuteAsync(operation);
        }

        private async Task<CloudTable> GetTableReference(string aContainer)
        {
            //TODO: add in-memory caching, so we dont try to create tables we created

            var table = mTableClient.GetTableReference(aContainer);
            await table.CreateIfNotExistsAsync();
            return table;
        }
    }
}