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
        internal static IEnumerable<Categories> GetCategories()
        {
            List<Categories> results = new List<Categories>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categories");

            TableQuery<Categories> query = new TableQuery<Categories>();

            foreach (Categories entity in table.ExecuteQuery(query))
            {
                results.Add(new Categories(
                        entity.PartitionKey.ToString(),
                        entity.RowKey.ToString()
                        ));
            }

            return results.ToArray();
        }

        internal static IEnumerable<Items> GetItems()
        {
            List<Items> results = new List<Items>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Items");

            TableQuery<Items> query = new TableQuery<Items>();

            foreach (Items entity in table.ExecuteQuery(query))
            {
                results.Add(new Items(
                        entity.PartitionKey.ToString(),
                        entity.RowKey.ToString()
                        ));
            }

            return results.ToArray();
        }

        internal static IEnumerable<Organizations> GetOrganizations()
        {
            List<Organizations> results = new List<Organizations>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Organizations");

            TableQuery<Organizations> query = new TableQuery<Organizations>();

            foreach (Organizations entity in table.ExecuteQuery(query))
            {
                results.Add(new Organizations(
                        entity.PartitionKey.ToString(),
                        entity.RowKey.ToString(),
                        entity.Phone.ToString(),
                        entity.AddressLine1.ToString(),
                        entity.AddressLine2.ToString(),
                        entity.AddressLine3.ToString(),
                        entity.ZipCode.ToString(),
                        entity.Website.ToString(),
                        entity.Hours.ToString(),
                        entity.Notes.ToString()
                        ));
            }

            return results.ToArray();
        }

        internal static IEnumerable<Items> GetCategoryItems(string categoryId)
        {
            List<Items> result = new List<Items>();
            List<string> joinResult = new List<string>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            
            // Fetch Item GUIDs for that Category
            CloudTable joinTable = tableClient.GetTableReference("CategoryItem");
            TableQuery<CategoryItem> joinQuery = new TableQuery<CategoryItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, categoryId));
            foreach (CategoryItem entity in joinTable.ExecuteQuery(joinQuery))
                joinResult.Add(entity.RowKey.ToString());

            // Fetch Item details from list of Item GUIDs
            CloudTable table = tableClient.GetTableReference("Items");
            foreach (string item in joinResult)
            {
                TableQuery<Items> query = new TableQuery<Items>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, item));
                Items firstItem = table.ExecuteQuery(query).First();
                result.Add(new Items(
                    firstItem.PartitionKey.ToString(),
                    firstItem.RowKey.ToString()
                ));                
            }

            // This will be a List of Items from the category id parameter
            return result.ToArray();
        }

        internal static IEnumerable<Organizations> GetItemOrganizations(string itemId)
        {
            List<Organizations> result = new List<Organizations>();
            List<KeyValuePair<string, int>> joinResult = new List<KeyValuePair<string, int>>();

            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();

            // Fetch Organization GUIDs for that Item
            CloudTable joinTable = tableClient.GetTableReference("ItemOrganization");
            TableQuery<ItemOrganization> joinQuery = new TableQuery<ItemOrganization>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, itemId));
            foreach (ItemOrganization entity in joinTable.ExecuteQuery(joinQuery))
                joinResult.Add(new KeyValuePair<string, int>(entity.RowKey.ToString(), entity.Offering));

            // Fetch Organization details from list of Organization GUIDs and apply Offering for that Item to Organization Entity
            CloudTable table = tableClient.GetTableReference("Organizations");
            foreach (KeyValuePair<string, int> organization in joinResult)
            {
                TableQuery<Organizations> query = new TableQuery<Organizations>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, organization.Key));
                Organizations firstItem = table.ExecuteQuery(query).First();
                result.Add(new Organizations(
                        firstItem.PartitionKey.ToString(),
                        firstItem.RowKey.ToString(),
                        firstItem.Phone.ToString(),
                        firstItem.AddressLine1.ToString(),
                        firstItem.AddressLine2.ToString(),
                        firstItem.AddressLine3.ToString(),
                        firstItem.ZipCode.ToString(),
                        firstItem.Website.ToString(),
                        firstItem.Hours.ToString(),
                        firstItem.Notes.ToString()
                ));
                result.Last<Organizations>().Offering = organization.Value;
            }

            // This will be a List of Organizations from the item id parameter
            return result.ToArray();
        }

        internal static bool AddItem(Items items)
        {
            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Items");

            if ((items != null) && (items.PartitionKey.ToString() != "") && (items.RowKey.ToString() != ""))
                table.Execute(TableOperation.Insert(items));
            else
                return false;

            return true;
        }

        internal static bool AddCategory(Categories categories)
        {
            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categories");

            if ((categories != null) && (categories.PartitionKey.ToString() != "") && (categories.RowKey.ToString() != ""))
                table.Execute(TableOperation.Insert(categories));
            else
                return false;

            return true;
        }

        internal static bool AddCategoryItem(CategoryItem categoryItem)
        {
            CloudStorageAccount connectionString = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("cs419db"));
            CloudTableClient tableClient = connectionString.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("CategoryItem");

            if ((categoryItem != null) && (categoryItem.PartitionKey.ToString() != "") && (categoryItem.RowKey.ToString() != ""))
                table.Execute(TableOperation.Insert(categoryItem));
            else
                return false;

            return true;
        }
    }
}
