using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Recycle_API.Entities
{
    public class Organizations : TableEntity
    {
        public Organizations() { }

        public Organizations(string id, string name, string phone, string address1, string address2, string address3, string zipcode, string website, string hours, string notes, int offering = -1)
        {
            this.PartitionKey = id;
            this.RowKey = name;
            Phone = phone;
            AddressLine1 = address1;
            AddressLine2 = address2;
            AddressLine3 = address3;
            ZipCode = zipcode;
            Website = website;
            Hours = hours;
            Notes = notes;
            Offering = offering;
        }
        
        public string Phone { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string ZipCode { get; set; }
        public string Website { get; set; }
        public string Hours { get; set; }
        public string Notes { get; set; }
        public int Offering { get; set; }
    }
}