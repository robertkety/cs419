using Corvallis_Reuse_and_Recycle_API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Corvallis_Reuse_and_Recycle_API.Controllers
{
    public class OrganizationsController : ApiController
    {
        // GET: api/Organizations
        /// <summary>
        /// Returns a list of all Organization objects in the Organizations table. (Do not use this call to retrieve Offering data - use ItemOrganization)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Organizations> Get()
        {
            return DataAccess.GetTable<Organizations>("Organizations");
        }

        // GET: api/Organizations/5
        /// <summary>
        /// Returns details of target Organization
        /// </summary>
        /// <param name="Id">Id of target Organization</param>
        /// <returns></returns>
        public IEnumerable<Organizations> Get([FromUri]string Id)
        {
            List<Organizations> result = new List<Organizations>();
            result.Add(DataAccess.GetFirstRow<Organizations>("Organizations", Id));
            
            return result.ToArray();
        }

        // POST: api/Organizations
        /// <summary>
        /// Creates a new organization with the following parameter information. (Do not use this call to create Offering data - use ItemOrganization)
        /// </summary>
        /// <param name="Name">Name of the organization (No '/', '\', '#', or '?' characters - See http://stackoverflow.com/a/11515366) </param>
        /// <param name="Phone">Numerical representation of the phone number (No '(', ')', or '-')</param>
        /// <param name="AddressLine1">First line of the street address</param>
        /// <param name="AddressLine2">Second line of the street address</param>
        /// <param name="AddressLine3">Third line of the street address</param>
        /// <param name="ZipCode">Zipcode of the address</param>
        /// <param name="Website">Website for the organization</param>
        /// <param name="Hours">Hours of operation for the organization</param>
        /// <param name="Notes">Any notes for the organization</param>
        [Authorize]
        public void Post([FromUri]string Name, [FromUri]string Phone = "", [FromUri]string AddressLine1 = "", [FromUri]string AddressLine2 = "", [FromUri]string AddressLine3 = "", [FromUri]string ZipCode = "", [FromUri]string Website = "", [FromUri]string Hours = "", [FromUri]string Notes = "")
        {
            if (Name == null)
                Name = "";
            DataAccess.AddRow("Organizations", new Organizations(Guid.NewGuid().ToString(), Name, Phone, AddressLine1, AddressLine2, AddressLine3, ZipCode, HttpUtility.HtmlEncode(Website), Hours, Notes, -1));
        }
        
        // PUT: api/Organizations/5
        /// <summary>
        /// Replaces the target organization with following parameter information. (Do not use this call to update Offering data - use ItemOrganization)
        /// </summary>
        /// <param name="Id">Id of the target organization</param>
        /// <param name="OldName">Old name of the organization (No '/', '\', '#', or '?' characters - See http://stackoverflow.com/a/11515366) </param>
        /// <param name="NewName">New name of the organization (No '/', '\', '#', or '?' characters - See http://stackoverflow.com/a/11515366) </param>
        /// <param name="Phone">Numerical representation of the phone number (No '(', ')', or '-')</param>
        /// <param name="AddressLine1">First line of the street address</param>
        /// <param name="AddressLine2">Second line of the street address</param>
        /// <param name="AddressLine3">Third line of the street address</param>
        /// <param name="ZipCode">Zipcode of the address</param>
        /// <param name="Website">Website for the organization</param>
        /// <param name="Hours">Hours of operation for the organization</param>
        /// <param name="Notes">Any notes for the organization</param>
        [Authorize]
        public void Put([FromUri]string Id, [FromUri]string OldName, [FromUri]string NewName = "", [FromUri]string Phone = "", [FromUri]string AddressLine1 = "", [FromUri]string AddressLine2 = "", [FromUri]string AddressLine3 = "", [FromUri]string ZipCode = "", [FromUri]string Website = "", [FromUri]string Hours = "", [FromUri]string Notes = "")
        {
            if (OldName == null)
                OldName = "";
            DataAccess.UpsertRow<Organizations>("Organizations", Id, OldName, new Organizations(Id, NewName, Phone, AddressLine1, AddressLine2, AddressLine3, ZipCode, HttpUtility.HtmlEncode(Website), Hours, Notes, -1));
        }
        
        // DELETE: api/Organizations/5
        /// <summary>
        /// Deletes the target organization from the Organizations table
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        [Authorize]
        public void Delete([FromUri]string Id, [FromUri]string Name)
        {
            if (Name == null)
                Name = ""; 
            DataAccess.DeleteRow<Organizations>("Organizations", Id, Name);
            var OrganizationItemRows = DataAccess.GetAllRowsByRowKey<ItemOrganization>("ItemOrganization", Id);
            foreach (var row in OrganizationItemRows)
                DataAccess.DeleteRow<ItemOrganization>("ItemOrganization", row.PartitionKey, row.RowKey);
        }
    }
}
