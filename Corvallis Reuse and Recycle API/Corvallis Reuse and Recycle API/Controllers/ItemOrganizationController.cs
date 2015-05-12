using Corvallis_Reuse_and_Recycle_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class ItemOrganizationController : ApiController
    {
        // GET: api/Items
        /// <summary>
        /// Returns a list of all items in the Items table
        /// </summary>
        /// <returns></returns>
        //public IEnumerable<ItemOrganization> Get()
        //{
        //    return DataAccess.GetTable<ItemOrganization>("ItemOrganization");
        //}

        // GET: api/ItemOrganization/5
        /// <summary>
        /// Gets a List of Items objects assigned to the target Organization
        /// </summary>
        /// <param name="Id">The Id of the target Organization</param>
        /// <returns></returns>
        //[Authorize]
        //public IEnumerable<Items> Get([FromUri]string Id)
        //{
        //    return DataAccess.GetFKReferenceByRowKey<ItemOrganization, Items>("ItemOrganization", "Items", Id);
        //}

        // GET: api/ItemOrganization/5
        /// <summary>
        /// Gets a List of Organizations objects assigned to the target Item
        /// </summary>
        /// <param name="ItemId">The Id of the target Item</param>
        /// <param name="Offering">Include accurate offering data</param>
        /// <returns></returns>
        //[Authorize]
        public IEnumerable<Organizations> Get([FromUri]string ItemId, [FromUri]bool Offering)
        {
            List<Organizations> result = new List<Organizations>();

            if (Offering)
            {
                IEnumerable<ItemOrganization> OrganizationOfferings = DataAccess.GetAllRows<ItemOrganization>("ItemOrganization", ItemId);
                foreach (ItemOrganization row in OrganizationOfferings)
                {
                    Organizations organization = DataAccess.GetFirstRow<Organizations>("Organizations", row.RowKey);
                    organization.Offering = row.Offering;
                    result.Add(organization);
                }
            }
            else
                return DataAccess.GetFKReferenceByPartitionKey<ItemOrganization, Organizations>("ItemOrganization", "Organizations", ItemId);

            return result.ToArray();
        }

        // POST: api/ItemOrganization
        /// <summary>
        /// Creates a single row entry in the ItemOrganization table.
        /// </summary>
        /// <param name="ItemId">The Id of the target Item</param>
        /// <param name="OrganizationId">The Id of the target Organizaiton</param>
        /// <param name="Offering">The integer offering for the item at that location (0: none, 1: reuse, 2: recycle, 3: both)</param>
        [Authorize]
        public void Post([FromUri]string ItemId, [FromUri]string OrganizationId = null, [FromUri]int Offering = 0)
        {
            if (ItemId == null)
                ItemId = "";
            if (OrganizationId == null)
                OrganizationId = "";

            DataAccess.AddRow("Items", new Items(new Guid().ToString(), ItemId));

            if (ItemId != "")
                DataAccess.AddRow("ItemOrganization", new ItemOrganization(new Guid().ToString(), OrganizationId, Offering));
        }

        // PUT: api/ItemOrganization/5
        /// <summary>
        /// Updates offering of Item/Organization row in ItemOrganization table
        /// </summary>
        /// <param name="ItemId">Id for target Item</param>
        /// <param name="OrganizationId">If for target Organization</param>
        /// <param name="Offering">The integer offering for the item at that location (0: none, 1: reuse, 2: recycle, 3: both)</param>
        [Authorize]
        public void Put([FromUri]string ItemId, [FromUri]string OrganizationId = null, [FromUri] int Offering = 0)
        {
            if ((ItemId != null) && (ItemId != ""))
            {
                if (OrganizationId != null)
                    OrganizationId = "";

                DataAccess.UpsertRow<ItemOrganization>("ItemOrganization", ItemId, OrganizationId, new ItemOrganization(ItemId, OrganizationId, Offering));
            }
        }

        // DELETE: api/ItemOrganization/5
        /// <summary>
        /// Deletes the target row in the ItemOrganization table
        /// </summary>
        /// <param name="ItemId">Id for target Item</param>
        /// <param name="OrganizationId">If for target Organization</param>
        [Authorize]
        public void Delete([FromUri] string ItemId, [FromUri] string OrganizationId)
        {
            DataAccess.DeleteRow<ItemOrganization>("ItemOrganization", ItemId, OrganizationId);
        }
    }
}
