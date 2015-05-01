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

        internal static IEnumerable<T> GetTable<T>(string tableName) where T : TableEntity, new()
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
        internal static T GetRow<T>(string tableName, string primaryKey, string rowKey) where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(primaryKey, rowKey);

            TableResult retrievedResult = table.Execute(retrieveOperation);

            return (T) retrievedResult.Result;
        }

        /* Gets first row returned from query of primary key on target storage table (tableName) */
        internal static T GetFirstRow<T>(string tableName, string primaryKey) where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(tableName);
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, primaryKey));
            return table.ExecuteQuery(query).First();
        }

        internal static IEnumerable<TElement> GetFKReference<TKey, TElement>(string lookupTableName, string derivativeTableName, string id) where TKey : TableEntity, new() where TElement : TableEntity, new()
        {
            List<TElement> result = new List<TElement>();
            List<string> joinResult = new List<string>();

            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            // Fetch derivative GUIDs for that lookups primary key
            CloudTable joinTable = tableClient.GetTableReference(lookupTableName);
            TableQuery<TKey> joinQuery = new TableQuery<TKey>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, id));
            foreach (TKey entity in joinTable.ExecuteQuery(joinQuery))
                joinResult.Add(entity.RowKey.ToString());

            // Fetch derivative details from list of derivative GUIDs
            CloudTable table = tableClient.GetTableReference(derivativeTableName);
            foreach (string item in joinResult)
            {
                TableQuery<TElement> query = new TableQuery<TElement>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, item));
                TElement firstItem = table.ExecuteQuery(query).First();
                result.Add(firstItem);
            }

            // This will be a List of derivative elements corresponding to the id parameter via the lookup table
            return result.ToArray();
        }
        
        internal static bool AddToTable<T>(T tableEntity, string storageTableName) where T : TableEntity, new()
        {
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(storageTableName);

            if ((tableEntity != null) && (tableEntity.PartitionKey.ToString() != "") && (tableEntity.RowKey.ToString() != ""))
                table.Execute(TableOperation.Insert(tableEntity));
            else
                return false;

            return true;
        }


    }
}
