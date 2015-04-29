using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Recycle_API.Entities
{
    public class CategoryItem : TableEntity
    {
        public CategoryItem() { }

        public CategoryItem (string categoryId, string itemId)
        {
            this.PartitionKey = categoryId;
            this.RowKey = itemId;
        }
    }
}