using Microsoft.WindowsAzure.Storage.Table;

namespace Corvallis_Reuse_and_Repair_API.Entities
{
    public class ItemOrganization : TableEntity
    {
        public ItemOrganization() { }

        public ItemOrganization(string itemId, string organizationId, int offering)
        {
            this.PartitionKey = itemId;
            this.RowKey = organizationId;
            Offering = offering;
        }
        /*
        public ItemOrganization(string itemId, string organizationId, Enums.offering offering)
        {
            this.PartitionKey = itemId;
            this.RowKey = organizationId;
            Offering = (int) offering;
        }*/

        public int Offering { get; set; } 
    }
}