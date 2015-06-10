using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CRRD_Web_Interface.Entities;
using CRRD_Web_Interface.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;

namespace CRRD_Web_Interface
{
    public class DataAccess
    {
        public static string url = ConfigurationManager.AppSettings["web_api_url"];
        public static string token = "";

        /* Thanks! https://msdn.microsoft.com/en-us/library/windows/apps/xaml/dn439314.aspx */
        internal static dynamic getDataFromService(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (token != null)
                    request.Headers["Authorization"] = "Bearer " + token;

                var response = request.GetResponse();
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
        
        /* Thanks! https://msdn.microsoft.com/en-us/library/debx8sh9%28v=vs.110%29.aspx */
        internal static dynamic postDataToService(string url, char[] charArray)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if ((token != null) && (token != ""))
                    request.Headers["Authorization"] = "Bearer " + token;

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(charArray, 0, charArray.Length);
                }
                var response = request.GetResponse();
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

        /* Thanks! https://msdn.microsoft.com/en-us/library/debx8sh9%28v=vs.110%29.aspx*/
        internal static dynamic putDataToService(string url, char[] charArray)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (token != null)
                    request.Headers["Authorization"] = "Bearer " + token;

                request.Method = "PUT";
                request.ContentType = "application/x-www-form-urlencoded";
                
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(charArray, 0, charArray.Length);
                }
                
                var response = request.GetResponse();
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

        /* Thanks! https://msdn.microsoft.com/en-us/library/debx8sh9%28v=vs.110%29.aspx*/
        internal static dynamic deleteDataToService(string url, char[] charArray)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                if (token != null)
                    request.Headers["Authorization"] = "Bearer " + token;

                request.Method = "DELETE";
                request.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(charArray, 0, charArray.Length);
                }
                var response = request.GetResponse();
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

        internal static List<T> Get<T>(string apiName)
        {
            string path = "api/" + apiName;
            List<T> entityList = new List<T>();

            dynamic entities = getDataFromService(url + path);

            if (entities == null)
                return null;

            foreach (dynamic entity in entities)
                entityList.Add((T)Activator.CreateInstance(typeof(T), entity.ToString()));

            return entityList;
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

        internal static async Task<Organization> GetOrganization(string organizationId)
        {
            string path = "api/organizations/";
            List<Organization> OrganizationList = new List<Organization>();
            dynamic organizations = await getDataFromService(url + path + organizationId).ConfigureAwait(false);

            if (organizations == null)
                return null;

            foreach (dynamic organization in organizations)
                OrganizationList.Add(new Organization(organization.ToString()));

            return OrganizationList.First();
        }



        public static SignInStatus PostLogin(Login login)
        {
            string path = "Token";
            string payload = "Email=" + login.username + "&Password=" + login.password + "&ConfirmPassword=" + login.password + "&grant_type=password" + "&UserName=" + login.username;

            dynamic confirmation = postDataToService(url + path, (payload).ToCharArray());

            try
            {
                if (confirmation != null)
                {
                    if (confirmation["access_token"] != null)
                    {
                        token = confirmation["access_token"].ToString();

                        return SignInStatus.Success;
                    }
                    else if (confirmation["error_description"] == "The user name or password is incorrect.")
                        return SignInStatus.Failure;
                }
                
                return SignInStatus.Failure;
            }
            catch (RuntimeBinderException)
            {
                return SignInStatus.Failure;
            }
        }

    }
}