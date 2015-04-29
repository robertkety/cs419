using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class OrganizationsController : ApiController
    {
        // GET: api/Organizations
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Organizations/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Organizations
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Organizations/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Organizations/5
        public void Delete(int id)
        {
        }
    }
}
