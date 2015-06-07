using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

namespace CRRD_Web_Interface.Models
{
    public class ItemOrganization
    {
        public ItemOrganization() { }

        public ItemOrganization(string json)
        {
            JObject jObject = JObject.Parse(json);
            Id = (string)jObject["PartitionKey"];
            Name = (string)jObject["RowKey"];
            Timestamp = (string)jObject["TimeStamp"];
            ETag = (string)jObject["ETag"];
        }

        public ItemOrganization(string itemId, string organizationId, Enums.offering offering)
        {
            this.Id = itemId;
            this.Name = organizationId;
            this.Offering = offering;
        }
        /*
        public ItemOrganization(string itemId, string organizationId, Enums.offering offering)
        {
            this.PartitionKey = itemId;
            this.RowKey = organizationId;
            Offering = (int) offering;
        }*/

        public Enums.offering Offering { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Timestamp { get; set; }

        public string ETag { get; set; }
    }
}