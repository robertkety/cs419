﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRRD_Web_Interface.Models
{
    public class Category
    {
        public Category(string json)
        {
            JObject jObject = JObject.Parse(json);
            Id = (string)jObject["PartitionKey"];
            Name = (string)jObject["RowKey"];
            Timestamp = (string)jObject["TimeStamp"];
            ETag = (string)jObject["ETag"];
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Timestamp { get; set; }
        public string ETag { get; set; }
    }
}
