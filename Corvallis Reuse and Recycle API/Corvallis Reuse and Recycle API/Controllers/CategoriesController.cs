using System.Collections.Generic;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class CategoriesController : ApiController
    {
        // GET: api/Categories
        public IEnumerable<string> Get()
        {
            return DataAccess.GetCategories();
        }

        // GET: api/Categories/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Categories
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Categories/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Categories/5
        public void Delete(int id)
        {
        }
    }
}
