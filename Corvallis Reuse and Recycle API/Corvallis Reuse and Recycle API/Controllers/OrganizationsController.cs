using Corvallis_Reuse_and_Recycle_API.Entities;
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
        public IEnumerable<Organizations> Get()
        {
            return DataAccess.GetOrganizations();
        }

        // POST: api/Organizations
        //[Authorize]
        public void Post([FromUri]string name, [FromUri]string phone = null, [FromUri]string AddressLine1 = null, [FromUri]string AddressLine2 = null, [FromUri]string AddressLine3 = null, [FromUri]string ZipCode = null, [FromUri]string Website = null, [FromUri]string Hours = null, [FromUri]string Notes = null)
        {
            //nulls?
        }
        /*
        // PUT: api/Organizations/5
        //[Authorize]
        public void Put([FromUri]int id, [FromUri]string value)
        {
        }

        // DELETE: api/Organizations/5
        //[Authorize]
        public void Delete([FromUri]int id)
        {
        }
        */
    }
}
