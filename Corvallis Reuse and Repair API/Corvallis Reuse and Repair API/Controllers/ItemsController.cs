using Corvallis_Reuse_and_Repair_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Corvallis_Reuse_and_Repair_API.Controllers
{
    public class ItemsController : ApiController
    {
        // GET: api/Items
        /// <summary>
        /// Returns a list of all items in the Items table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Items> Get()
        {
            return DataAccess.GetTable<Items>("Items");
        }

        // GET: api/Items/5
        /// <summary>
        /// Returns a list of organization objects for the target Item id
        /// </summary>
        /// <param name="Id">Id of the target item</param>
        /// <returns></returns>
        public IEnumerable<Organizations> Get([FromUri]string Id)
        {
            List<Organizations> result = new List<Organizations>();

            IEnumerable<ItemOrganization> OrganizationOfferings = DataAccess.GetAllRows<ItemOrganization>("ItemOrganization", Id);
            foreach (ItemOrganization row in OrganizationOfferings)
            {
                Organizations organization = DataAccess.GetFirstRow<Organizations>("Organizations", row.RowKey);
                organization.Offering = row.Offering;
                result.Add(organization);
            }

            return result.ToArray();
            //return DataAccess.GetFKReferenceByPartitionKey<ItemOrganization, Organizations>("ItemOrganization", "Organizations", Id);
        }
        
        // POST: api/Items
        /// <summary>
        /// Creates a new Item with the target name and associates it with the corresponding list of category ids
        /// </summary>
        /// <param name="Name">The name of the new Item</param>
        /// <param name="Categories">A list of category ids to be associated with the target item</param>
        [Authorize]
        public void Post([FromUri]string Name, [FromUri]string[] Categories = null)
        {
            if (Name == null)
                Name = "";
            string NewItemGuid = Guid.NewGuid().ToString();
            DataAccess.AddRow("Items", new Items(NewItemGuid, Name));
            if (Categories != null)
                foreach (string category in Categories)
                    DataAccess.AddRow("CategoryItem", new CategoryItem(category, NewItemGuid));            
        }
        
        // PUT: api/Items/5
        /// <summary>
        /// Updates the name of the target item.  Old name is required for an efficient storage table query
        /// </summary>
        /// <param name="Id">Id of the target item</param>
        /// <param name="OldName">Exising name for target item</param>
        /// <param name="NewName">New name for target item</param>
        [Authorize]
        public void Put([FromUri]string Id, [FromUri]string OldName, [FromUri]string NewName = "")
        {
            if (OldName == null)
                OldName = ""; 
            DataAccess.UpsertRow<Items>("Items", Id, OldName, new Items(Id, NewName));
        }
        
        // DELETE: api/Items/5
        /// <summary>
        /// Deletes the target item from the Items table
        /// </summary>
        /// <param name="Id">Id for the target item</param>
        /// <param name="Name">Name of the target item</param>
        [Authorize]
        public void Delete([FromUri]string Id, [FromUri]string Name)
        {
            if (Name == null)
                Name = ""; 
            DataAccess.DeleteRow<Items>("Items", Id, Name);
            DataAccess.DeleteAllRowsWithId<ItemOrganization>("ItemOrganization", Id);
            var CategoryItemRows = DataAccess.GetAllRowsByRowKey<CategoryItem>("CategoryItem", Id);
            foreach (var row in CategoryItemRows)
                DataAccess.DeleteRow<CategoryItem>("CategoryItem", row.PartitionKey, row.RowKey);
        }  
    }
}
