using Corvallis_Reuse_and_Repair_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Corvallis_Reuse_and_Repair_API.Controllers
{
    public class CategoryItemController : ApiController
    {
        // GET: api/CategoryItem/5
        /// <summary>
        /// Gets a List of Items objects assigned to the target Category
        /// </summary>
        /// <param name="Id">The Id of the target Category</param>
        /// <returns></returns>
        [Authorize]
        public IEnumerable<Items> Get([FromUri]string Id)
        {
            return DataAccess.GetFKReferenceByPartitionKey<CategoryItem, Items>("CategoryItem", "Items", Id);
        }

        // POST: api/CategoryItem
        /// <summary>
        /// Creates rows of table references between the target Category Id and each of the Item Ids in the Items array
        /// </summary>
        /// <param name="Id">The Id of the target Category</param>
        /// <param name="Items">An array of the target Items belonging to the target Category</param>
        [Authorize]
        public void Post([FromUri]string Id, [FromUri]string[] Items = null)
        {
            if (Id == null)
                Id = "";

            if (Items != null)
                foreach (string Item in Items)
                    DataAccess.AddRow("CategoryItem", new CategoryItem(Id, Item));
        }

        /*
        // POST: api/CategoryItem
        /// <summary>
        /// Creates a new Category in the Category table and rows of table references between the new Category Id and each of the Item Ids in the Items array
        /// </summary>
        /// <param name="Name">The Name for the new Category</param>
        /// <param name="Items">An array of the target Items belonging to the new Category</param>
        [Authorize]
        public void Post([FromUri]string Name, [FromUri]string[] Items = null)
        {
            string NewCategoryGuid = new Guid().ToString();
            
            if (Name == null)
                Name = "";

            DataAccess.AddRow("Categories", new Categories(NewCategoryGuid, Name));

            if (Items != null)
                foreach (string Item in Items)
                    DataAccess.AddRow("CategoryItem", new CategoryItem(NewCategoryGuid, Item));
        }*/

        // PUT: api/CategoryItem/5
        /// <summary>
        /// Updates the Category Name for the taget Id and replaces all relational rows in CategoryItem table belonging to target Category Id with each of the Item Ids in the Items array
        /// </summary>
        /// <param name="Id">The Id of the target Category</param>
        /// <param name="Items">An array of the target Items belonging to the target Category</param>
        /// <param name="CreateRelation">A bool value determining if the list of item ids should be related to the target category id</param>
        [Authorize]
        public void Put([FromUri]string Id, [FromUri]string[] Items = null, [FromUri]bool CreateRelation = true)
        {
            if (Items != null)
                foreach (string Item in Items)
                {
                    List<string> ItemId = new List<string>();
                    ItemId.Add(Item);

                    Delete(Id, ItemId.ToArray());
                    if(CreateRelation)
                        DataAccess.AddRow("CategoryItem", new CategoryItem(Id, Item));
                }
        }

        // DELETE: api/CategoryItem/5
        /// <summary>
        /// Deletes all relational rows belonging to target Category Id in the CategoryItem table
        /// </summary>
        /// <param name="Id">The Id of the target Category</param>
        [Authorize]
        public void Delete([FromUri] string Id)
        {
            DataAccess.DeleteAllRowsWithId<CategoryItem>("CategoryItem", Id);
        }

        // DELETE: api/CategoryItem/5?Items[]={0}
        /// <summary>
        /// Deletes all relational rows belonging to target Category Id and Item Id in the CategoryItem table
        /// </summary>
        /// <param name="Id">The Id of the target Category</param>
        /// <param name="Items">An array of the target Items by Id belonging to the target Category</param>
        [Authorize]
        public void Delete([FromUri] string Id, [FromUri]string[] Items = null)
        {
            if (Items != null)
                foreach (string Item in Items)
                    DataAccess.DeleteRow<CategoryItem>("CategoryItem", Id, Item);
        }
    }
}
