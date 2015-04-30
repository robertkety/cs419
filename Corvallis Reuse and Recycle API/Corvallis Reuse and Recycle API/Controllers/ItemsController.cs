using Corvallis_Reuse_and_Recycle_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class ItemsController : ApiController
    {
        // GET: api/Items
        public IEnumerable<Items> Get()
        {
            return DataAccess.GetItems();
        }

        // GET: api/Items/5
        public IEnumerable<Organizations> Get([FromUri]string id)
        {
            return DataAccess.GetItemOrganizations(id);
        }
        
        // POST: api/Items
        //[Authorize]
        public void Post([FromUri]string name, [FromUri]string[] categories = null)
        {
            string NewItemGuid = new Guid().ToString();
            DataAccess.AddItem(new Items(NewItemGuid, name));
            if (categories != null)
                foreach (string category in categories)
                    DataAccess.AddCategoryItem(new CategoryItem(category, NewItemGuid));
        }
        /*
        // PUT: api/Items/5
        //[Authorize]
        public void Put([FromUri]int id, [FromUri]string value)
        {
        }

        // DELETE: api/Items/5
        //[Authorize]
        public void Delete([FromUri]int id)
        {
        }
        */
    }
}
