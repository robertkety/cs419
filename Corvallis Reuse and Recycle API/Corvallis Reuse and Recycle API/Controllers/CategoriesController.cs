using Corvallis_Reuse_and_Recycle_API.Entities;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class CategoriesController : ApiController
    {
        // GET: api/Categories
        public IEnumerable<Categories> Get()
        {
            return DataAccess.GetTable<Categories>("Categories");
        }

        // GET: api/Categories/5
        public IEnumerable<Items> Get([FromUri]string id)
        {
            return DataAccess.GetFKReference<CategoryItem, Items>("CategoryItem", "Items", id);
        }
        
        // POST: api/Categories
        //[Authorize]
        public void Post([FromUri]string name, [FromUri]string[] items = null)
        {
            string NewCategoryGuid = new Guid().ToString();
            DataAccess.AddToTable(new Categories(new Guid().ToString(), name), "Category");
            if (items != null)
                foreach (string item in items)
                    DataAccess.AddToTable(new CategoryItem(NewCategoryGuid, item), "CategoryItem");            
        }
        /*
        // PUT: api/Categories/5
        //[Authorize]
        public void Put([FromUri]int id, [FromUri]string value)
        {
        }

        // DELETE: api/Categories/5
        //[Authorize]
        public void Delete([FromUri]int id)
        {
        }
        */
    }
}
