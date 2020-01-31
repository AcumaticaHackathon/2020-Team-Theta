using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace PX.FirefliesConnector.Ext
{
    public static class WebRequestML
    {
        /// <summary>
        /// Sending POST request.
        /// </summary>
        /// <param name="url">Request Url.</param>
        /// <param name="data">Data for request.</param>
        /// <param name="userName">user name for log in</param>
        /// <param name="password">password for log in</param>
        /// <returns>Response body.</returns>
        public static string HttpPost(string url, string data, string userName = "", string password = "", int timeout = 100000)
        {

            string Out = String.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                req.ContentType = "application/json";
                req.Method = "POST";
                req.ProtocolVersion = HttpVersion.Version11;
                req.Accept = "*/*";

                AddAuthentication(userName, password, req);

                req.Timeout = timeout;
                //req.ContentType = "application/x-www-form-urlencoded";
                byte[] sentData = Encoding.UTF8.GetBytes(data);
                req.ContentLength = sentData.Length;
                using (System.IO.Stream sendStream = req.GetRequestStream())
                {
                    sendStream.Write(sentData, 0, sentData.Length);
                    sendStream.Flush();
                    sendStream.Close();
                }
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                System.IO.Stream ReceiveStream = res.GetResponseStream();
                using (System.IO.StreamReader sr = new System.IO.StreamReader(ReceiveStream, Encoding.UTF8))
                {
                    Char[] read = new Char[256];
                    int count = sr.Read(read, 0, 256);

                    while (count > 0)
                    {
                        String str = new String(read, 0, count);
                        Out += str;
                        count = sr.Read(read, 0, 256);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        public static bool AcceptAllCertifications(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certification, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //for testing purpose only, accept any dodgy certificate... 
        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Sending GET request.
        /// </summary>
        /// <param name="url">Request Url.</param>
        /// <param name="data">Data for request.</param>
        /// <param name="userName">user name for log in</param>
        /// <param name="password">password for log in</param>
        /// <returns>Response body.</returns>
        public static string HttpGet(string url, string data, string userName, string password)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            string Out = String.Empty;
            Uri uri = new Uri(url);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + (string.IsNullOrEmpty(data) ? "" : "?" + data));
            req.ContentType = "application/json";
            req.Method = "GET";
            //req.Credentials = CredentialCache.DefaultCredentials;
            //req.Proxy = null;
            //ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);


            AddAuthentication(userName, password, req);
            try
            {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        private static void AddAuthentication(string userName, string password, HttpWebRequest req)
        {
            if (userName != null && password != null)
            {
                string base64Credentials = GetEncodedCredentials(userName, password);
                req.Headers.Add("Authorization", "Basic " + base64Credentials);
            }
        }


        public static string WebClient(string url, string userName, string password)
        {

            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(userName, password);
                client.UseDefaultCredentials = true;
                var s = client.DownloadString(url);
                return s;
            }
            return String.Empty;
        }

        /// <summary>
        /// Sending Delete request.
        /// </summary>
        /// <param name="url">Request Url.</param>
        /// <param name="data">Data for delete.</param>
        /// <param name="userName">user name for log in</param>
        /// <param name="password">password for log in</param>
        /// <returns>Response body.</returns>
        public static string HttpDelete(string url, string data, string userName, string password)
        {
            string Out = String.Empty;
            Uri uri = new Uri(url);

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url + (string.IsNullOrEmpty(data) ? "" : "?" + data));
            req.ContentType = "application/json";
            req.Method = "DELETE";

            AddAuthentication(userName, password, req);

            try
            {
                System.Net.WebResponse resp = req.GetResponse();
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        Out = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Out = string.Format("HTTP_ERROR :: The second HttpWebRequest object has raised an Argument Exception as 'Connection' Property is set to 'Close' :: {0}", ex.Message);
            }
            catch (WebException ex)
            {
                Out = string.Format("HTTP_ERROR :: WebException raised! :: {0}", ex.Message);
            }
            catch (Exception ex)
            {
                Out = string.Format("HTTP_ERROR :: Exception raised! :: {0}", ex.Message);
            }

            return Out;
        }

        private static string GetEncodedCredentials(string userName, string password)
        {
            string mergedCredentials = string.Format("{0}:{1}", userName, password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }
}
