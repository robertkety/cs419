using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

namespace CRRD_Web_Interface.Models
{
    public class CategoryItem
    {
        public CategoryItem() { }

        public CategoryItem(string json)
        {
            JObject jObject = JObject.Parse(json);
            Id = (string)jObject["PartitionKey"];
            Name = (string)jObject["RowKey"];
            Timestamp = (string)jObject["TimeStamp"];
            ETag = (string)jObject["ETag"];
        }

        public CategoryItem (string categoryId, string itemId)
        {
            this.Id = categoryId;
            this.Name = itemId;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Timestamp { get; set; }

        public string ETag { get; set; }
    }
}