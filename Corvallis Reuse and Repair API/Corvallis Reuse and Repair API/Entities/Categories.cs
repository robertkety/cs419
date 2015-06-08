using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Repair_API.Entities
{
    public class Categories : TableEntity
    {
        public Categories() { }

        public Categories(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }
    }
}