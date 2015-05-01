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
            return DataAccess.GetTable<Organizations>("Organizations");
        }

        // POST: api/Organizations
        //[Authorize]
        public void Post([FromUri]string Name, [FromUri]string Phone = "", [FromUri]string AddressLine1 = "", [FromUri]string AddressLine2 = "", [FromUri]string AddressLine3 = "", [FromUri]string ZipCode = "", [FromUri]string Website = "", [FromUri]string Hours = "", [FromUri]string Notes = "", [FromUri]int Offering = -1)
        {
            DataAccess.AddToTable(new Organizations(new Guid().ToString(), Name, Phone, AddressLine1, AddressLine2, AddressLine3, ZipCode, Website, Hours, Notes, Offering), "Organizations");            
        }
        
        // PUT: api/Organizations/5
        //[Authorize]
        public void Put([FromUri]string id, [FromUri]string Name, [FromUri]string Phone = "", [FromUri]string AddressLine1 = "", [FromUri]string AddressLine2 = "", [FromUri]string AddressLine3 = "", [FromUri]string ZipCode = "", [FromUri]string Website = "", [FromUri]string Hours = "", [FromUri]string Notes = "", [FromUri]int Offering = -1)
        {
            Organizations RowEntity = DataAccess.GetFirstRow<Organizations>("Organizations", id);

        }
        /*
        // DELETE: api/Organizations/5
        //[Authorize]
        public void Delete([FromUri]int id)
        {
        }
        */
    }
}
