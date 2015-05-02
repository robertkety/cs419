using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
//Add MySql Library
using MySql.Data.Entity;
using MySql.Data.Types;

// SSH
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Table;
using Corvallis_Reuse_and_Recycle_API.Entities;

namespace Corvallis_Reuse_and_Recycle_API
{
    internal class DataAccess
    {
        internal static CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));

        internal static IEnumerable<T> GetTable<T>(string tableName) 
            where T : TableEntity, new()
        {
            List<T> results = new List<T>();

            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            TableQuery<T> query = new TableQuery<T>();
            foreach (T entity in table.ExecuteQuery(query))
            {
                results.Add(entity);
            }

            return results.ToArray();
        }

        /* Gets a specific row (requires primary key AND row key) */
        internal static T GetRow<T>(string tableName, string partitionKey, string rowKey) 
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            return (T)retrievedResult.Result;
        }

        /* Gets first row returned from query of primary key on target storage table (tableName) */
        internal static T GetFirstRow<T>(string tableName, string partitionKey) 
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return table.ExecuteQuery(query).First();
        }

        /* Gets all rows returned from query of primary key on target storage table (tableName) */
        internal static IEnumerable<T> GetAllRows<T>(string tableName, string partitionKey)
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return table.ExecuteQuery(query);
        }

        internal static IEnumerable<T1> GetFKReferenceByPartitionKey<T, T1>(string lookupTableName, string derivativeTableName, string id)
            where T : TableEntity, new()
            where T1 : TableEntity, new()
        {
            // Fetch derivative GUIDs for that lookups partition key
            IEnumerable<T> joinResult = GetAllRows<T>(lookupTableName, id);//new List<string>();

            List<T1> result = new List<T1>();
            foreach (T item in joinResult)
                result.Add(GetFirstRow<T1>(derivativeTableName, item.RowKey.ToString()));

            // This will be a List of derivative elements corresponding to the id parameter via the lookup table
            return result.ToArray();
        }

        internal static IEnumerable<T1> GetFKReferenceByRowKey<T, T1>(string lookupTableName, string derivativeTableName, string id)
            where T : TableEntity, new()
            where T1 : TableEntity, new()
        {
            // Fetch derivative GUIDs for that lookups row key
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(lookupTableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, id));
            IEnumerable<T> joinResult = table.ExecuteQuery(query);

            List<T1> result = new List<T1>();
            foreach (T organization in joinResult)
                result.Add(GetFirstRow<T1>(derivativeTableName, organization.PartitionKey.ToString()));

            // This will be a List of derivative elements corresponding to the id parameter via the lookup table
            return result.ToArray();
        }

        internal static bool AddRow<T>(string tableName, T newEntity) 
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            if ((newEntity != null) && (newEntity.PartitionKey.ToString() != "") && (newEntity.RowKey.ToString() != ""))
                table.Execute(TableOperation.Insert(newEntity));
            else
                return false;

            return true;
        }

        internal static bool UpsertRow<T>(string tableName, string partitionKey, string rowKey, T modifiedEntity) 
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);
            T tableEntity = (T)retrievedResult.Result;

            if ((modifiedEntity.PartitionKey != null) && (modifiedEntity.RowKey != null))
            {
                if ((tableEntity != null) && (tableEntity.RowKey != modifiedEntity.RowKey))
                    DeleteRow<T>(tableName, partitionKey, rowKey);
                

                TableOperation UpsertOperation = TableOperation.InsertOrReplace(tableEntity);
                table.Execute(UpsertOperation);
            }
            else             
                return false;            

            return true;
        }
        
        internal static bool DeleteRow<T>(string tableName, string partitionKey, string rowKey) 
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);
            T tableEntity = (T)retrievedResult.Result;

            if (tableEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(tableEntity);
                table.Execute(deleteOperation);
            }
            else
                return false;

            return true;
        }

        internal static bool DeleteAllRowsWithId<T>(string tableName, string partitionKey)
            where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            IEnumerable<T> Rows = GetAllRows<T>(tableName, partitionKey);
            
            try
            {
                foreach (T Row in Rows)
                {
                    TableOperation deleteOperation = TableOperation.Delete(Row);
                    table.Execute(deleteOperation);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
