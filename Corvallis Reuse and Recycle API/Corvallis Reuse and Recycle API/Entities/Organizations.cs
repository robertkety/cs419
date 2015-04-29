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

        public Organizations(string id, string name, string phone, string address1, string address2, string address3, string zipcode, string website, string hours, string notes)
        {
            this.PartitionKey = id;
            this.RowKey = name;
            _phone = phone;
            _address1 = address1;
            _address2 = address2;
            _address3 = address3;
            _zipcode = zipcode;
            _website = website;
            _hours = hours;
            _notes = notes;
        }

        public string _phone { get; set; }
        public string _address1 { get; set; }
        public string _address2 { get; set; }
        public string _address3 { get; set; }
        public string _zipcode { get; set; }
        public string _website { get; set; }
        public string _hours  { get; set; }
        public string _notes { get; set; }
    }
}