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
        internal static IEnumerable<T> GetTable<T>(string tableName) where T : TableEntity, new()
        {
            List<T> results = new List<T>();
             
            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(tableName);

            TableQuery<T> query = new TableQuery<T>();
            foreach (T entity in table.ExecuteQuery(query))
            {
                results.Add(entity);
            }

            return results.ToArray();
        }

        internal static IEnumerable<TElement> GetFKReference<TKey, TElement>(string lookupTableName, string derivativeTableName, string id) where TKey : TableEntity, new() where TElement : TableEntity, new()
        {
            List<TElement> result = new List<TElement>();
            List<string> joinResult = new List<string>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
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
        
        internal static bool AddToTable(ITableEntity tableEntity, string storageTableName)
        {
            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
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
