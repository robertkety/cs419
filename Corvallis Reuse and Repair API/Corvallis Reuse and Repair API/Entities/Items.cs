using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Repair_API.Entities
{
    public class Items : TableEntity
    {
        public Items() { }

        public Items(string id, string name)
        {
            this.PartitionKey = id;
            this.RowKey = name;
        }
    }
}