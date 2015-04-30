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
            return DataAccess.GetCategories();
        }

        // GET: api/Categories/5
        public IEnumerable<Items> Get([FromUri]string id)
        {
            return DataAccess.GetCategoryItems(id);
        }
        
        // POST: api/Categories
        //[Authorize]
        public void Post([FromUri]string name, [FromUri]string[] items = null)
        {
            string NewCategoryGuid = new Guid().ToString();
            DataAccess.AddCategory(new Categories(new Guid().ToString(), name));
            if (items != null)
                foreach (string item in items)
                    DataAccess.AddCategoryItem(new CategoryItem(NewCategoryGuid, item));
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
