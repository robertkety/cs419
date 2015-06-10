using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Repair_API.Entities
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