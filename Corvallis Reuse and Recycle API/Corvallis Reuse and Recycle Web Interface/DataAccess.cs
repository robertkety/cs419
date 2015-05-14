using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.AspNet.Identity.Owin;

namespace CRRD_Web_Interface
{
    public class DataAccess
    {
        public static string url = "http://cs419.azurewebsites.net";//"http://localhost/Corvallis%20Reuse%20and%20Recycle%20API/";
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

                if (token != null)
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

        public static SignInStatus PostLogin(Entities.Login login)
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