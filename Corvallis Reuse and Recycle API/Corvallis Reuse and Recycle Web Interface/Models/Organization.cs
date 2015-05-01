using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRRD_Web_Interface.Models
{
    public class Organization
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string Hours { get; set; }
        public string Notes { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string ZipCode { get; set; }
        public int Offering { get; set; }
    }
}