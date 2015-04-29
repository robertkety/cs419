using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Recycle_API.Entities
{
    public class ItemOrganization : TableEntity
    {
        public ItemOrganization() { }

        public ItemOrganization(string itemId, string organizationId)
        {
            this.PartitionKey = itemId;
            this.RowKey = organizationId;
        }
    }
}