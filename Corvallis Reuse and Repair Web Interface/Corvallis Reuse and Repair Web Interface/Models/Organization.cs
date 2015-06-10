using System;
using Newtonsoft.Json.Linq;

namespace CRRD_Web_Interface.Models
{
    public class Organization
    {
        public Organization(string json)
        {
            JObject jObject = JObject.Parse(json);
            Id = (string)jObject["PartitionKey"];
            Name = (string)jObject["RowKey"];
            Timestamp = (string)jObject["TimeStamp"];
            ETag = (string)jObject["ETag"];
            Phone = (string)jObject["Phone"];
            AddressLine1 = (string)jObject["AddressLine1"];
            AddressLine2 = (string)jObject["AddressLine2"];
            AddressLine3 = (string)jObject["AddressLine3"];
            ZipCode = (string)jObject["ZipCode"];
            Website = (string)jObject["Website"];
            Hours = (string)jObject["Hours"];
            Notes = (string)jObject["Notes"];
            Offering = (Enums.offering) Enum.Parse(typeof(Enums.offering), (string)jObject["Offering"]);
        }

        public Organization()
        {
            Id = Name = Timestamp = ETag = Phone = AddressLine1 = AddressLine2 = AddressLine3 = ZipCode = Website = Hours = Notes = ""; 
            Offering = Enums.offering.none;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Timestamp { get; set; }
        public string ETag { get; set; }
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Hours { get; set; }
        public string Notes { get; set; }
        public Enums.offering Offering { get; set; }
    }
}
