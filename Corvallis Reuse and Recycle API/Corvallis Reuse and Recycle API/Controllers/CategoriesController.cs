using Corvallis_Reuse_and_Recycle_API.Entities;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class CategoriesController : ApiController
    {
        // GET: api/Categories
        /// <summary>
        /// Returns a list of Category names in the Categories table
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Categories> Get()
        {
            return DataAccess.GetTable<Categories>("Categories");
        }

        // GET: api/Categories/5
        /// <summary>
        /// Returns a list of Items in a target Category
        /// </summary>
        /// <param name="Id">Id of target Category</param>
        /// <returns></returns>
        public IEnumerable<Items> Get([FromUri]string Id)
        {
            return DataAccess.GetFKReferenceByPartitionKey<CategoryItem, Items>("CategoryItem", "Items", Id);
        }
        
        // POST: api/Categories        
        [Authorize]
        /// <summary>
        /// Creates a new category in the Categories table with the specified name and associates it with the corresponding list of item ids.
        /// </summary>
        /// <param name="Name">Name of the new Category</param>
        /// <param name="Items">A list of item ids to be associated with the target category</param>
        public void Post([FromUri]string Name, [FromUri]string[] Items = null)
        {
            if (Name == null)
                Name = ""; 
            string NewCategoryGuid = new Guid().ToString();
            DataAccess.AddRow("Categories", new Categories(new Guid().ToString(), Name));
            
            if (Items != null)
                foreach (string ItemId in Items)
                    DataAccess.AddRow("CategoryItem", new CategoryItem(NewCategoryGuid, ItemId));
        }
        
        // PUT: api/Categories/5
        [Authorize]
        /// <summary>
        /// Updates the name of the target category.  Old name is required for an efficient storage table query
        /// </summary>
        /// <param name="Id">Id of the target category</param>
        /// <param name="OldName">Exising name for target category</param>
        /// <param name="NewName">New name for target category</param>
        public void Put([FromUri]string Id, [FromUri]string OldName, [FromUri]string NewName = "")
        {
            if (OldName == null)
                OldName = ""; 
            DataAccess.UpsertRow<Categories>("Categories", Id, OldName, new Categories(Id, NewName));
        }
        
        // DELETE: api/Categories/5
        [Authorize]
        /// <summary>
        /// Deletes the target category
        /// </summary>
        /// <param name="Id">Id of the target category</param>
        /// <param name="Name">Name of the target category</param>
        public void Delete([FromUri]string Id, [FromUri]string Name)
        {
            if (Name == null)
                Name = ""; 
            DataAccess.DeleteRow<Categories>("Categories", Id, Name);
        }        
    }
}
