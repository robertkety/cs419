using Corvallis_Reuse_and_Recycle_Mobile_Application.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Corvallis_Reuse_and_Recycle_Mobile_Application
{
    public sealed class DataAccess
    {
        public static string url = "http://cs419.azurewebsites.net/";

        /* Thanks! https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn439314.aspx */
        private static async Task<dynamic> getDataFromService(string url)
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (localSettings.Values["token"] != null)
                    request.Headers["Authorization"] = "Bearer " + localSettings.Values["token"].ToString();

                var response = await request.GetResponseAsync().ConfigureAwait(false);
                var stream = response.GetResponseStream();

                var streamReader = new StreamReader(stream);
                string responseText = streamReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                return data;
            }
            catch (WebException webex)
            {
                try
                {
                    /* Thanks! http://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba */
                    using (WebResponse response = webex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;

                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();
                            dynamic message = JsonConvert.DeserializeObject(text);
                            return message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        /* Thanks! https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn439314.aspx */
        private static async Task<dynamic> getDataFromUnsecuredService(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                var response = await request.GetResponseAsync().ConfigureAwait(false);
                var stream = response.GetResponseStream();

                var streamReader = new StreamReader(stream);
                string responseText = streamReader.ReadToEnd();

                dynamic data = JsonConvert.DeserializeObject(responseText);

                return data;
            }
            catch (WebException webex)
            {
                try
                {
                    /* Thanks! http://stackoverflow.com/questions/692342/net-httpwebrequest-getresponse-raises-exception-when-http-status-code-400-ba */
                    using (WebResponse response = webex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;

                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();
                            dynamic message = JsonConvert.DeserializeObject(text);
                            return message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        internal static async Task<List<Category>> GetCategories()
        {
            string path = "api/categories";
            List<Category> CategoryList = new List<Category>();

            dynamic categories = await getDataFromService(url + path).ConfigureAwait(false);

            if (categories == null)
                return null;

            foreach (dynamic category in categories)
                CategoryList.Add(new Category(category.ToString()));
            
            return CategoryList;
        }

        internal static async Task<List<Item>> GetCategoryItem(string categoryId)
        {
            string path = "api/categories/" + categoryId;
            List<Item> ItemsList = new List<Item>();

            dynamic items = await getDataFromService(url + path).ConfigureAwait(false);

            if (items == null)
                return null;

            foreach (dynamic item in items)
                ItemsList.Add(new Item(item.ToString()));
            
            return ItemsList;
        }

        internal static async Task<List<Organization>> GetItemOrganization(string itemId)
        {
            string path = "api/Items/" + itemId;
            List<Organization> OrganizationsList = new List<Organization>();

            dynamic organizations = await getDataFromService(url + path).ConfigureAwait(false);

            if (organizations == null)
                return null;

            foreach (dynamic organization in organizations)
                OrganizationsList.Add(new Organization(organization.ToString()));


            return OrganizationsList;
        }                
    
        internal static async Task<Organization> GetOrganization(string organizationId){
            string path = "api/organizations/";
            List<Organization> OrganizationList = new List<Organization>();
            dynamic organizations = await getDataFromService(url + path + organizationId).ConfigureAwait(false);

            if (organizations == null)
                return null;

            foreach (dynamic organization in organizations)
                OrganizationList.Add(new Organization(organization.ToString()));

            return OrganizationList.First();
        }

        internal static async Task<string> GetCityState(string ZipCode)
        {
            string ziptasticURL = "http://ziptasticapi.com/";
            string RawResults = Convert.ToString(await getDataFromUnsecuredService(ziptasticURL + ZipCode).ConfigureAwait(false));            
            CityStateZip Results = JsonConvert.DeserializeObject<CityStateZip>(RawResults);
 
            if (Results == null)
                return String.Format("{0} is a bad zip code.  Please contact system administrator", ZipCode);

            // Canonize City to first letter upper and remaining lower case
            string uppercaseCity = (Results.city).ToUpper();
            Results.city = String.Concat(uppercaseCity.Substring(0, 1), uppercaseCity.ToLower().Substring(1));
            
            return String.Format("{0}, {1} {2}", Results.city, Results.state, ZipCode);
        }
    }
}
