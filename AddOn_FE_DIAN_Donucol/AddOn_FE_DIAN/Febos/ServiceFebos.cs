using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class ServiceFebos
    {
        public static DateTime _createdDate;

        public static Dictionary<bool, string> Febos_token(string url, string method, string body = "", bool saveCookie = false)
        {
            var result = new Dictionary<bool, string>();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers.Add("token", "tfuhiyr5e356dtu7");
            //httpRequest.Headers.Add("empresa", "900800100");
            httpRequest.Method = method.ToUpper();
            httpRequest.ServicePoint.Expect100Continue = false;
            //ServicePointManager.ServerCertificateValidationCallback += RemoteSSLTLSCertificateValidate;
            httpRequest.CookieContainer = new CookieContainer();

            //if (cookieData.Count > 0)
            //{
            //    foreach (Cookie cookie in cookieData)
            //    {
            //        httpRequest.CookieContainer.Add(new Uri(url), new Cookie(cookie.Name, cookie.Value));
            //    }
            //}

            if (!string.IsNullOrEmpty(body))
            {
                using (var requestStream = httpRequest.GetRequestStream())
                {
                    var writer = new StreamWriter(requestStream);
                    writer.Write(body);
                    writer.Close();
                }
            }

            try
            {
                //var webResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        //using (var reader = new StreamReader(response.GetResponseStream()))
                        //{
                        //    string result = reader.ReadToEnd();
                        //}
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                        }
                        //if (saveCookie)
                        //    cookieData = webResponse.Cookies;
                    }

                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            string error = reader.ReadToEnd();
                            result.Add(false, error);
                            //error.Replace(@"\t", "\t");
                            //error.Replace(@"\n", "\n");
                            //error.Replace(@"\r", "\r");
                            ////error = JsonConvert.SerializeObject(error);
                            //error = error.Replace(@"\", "");
                            //error = error.Replace("\\", "");

                            //Object outerror = JToken.Parse(error);
                            //error = JsonConvert.SerializeObject(outerror);
                            ////string outerror = error.Replace("\\", "");
                            //result.Add(false, error);
                            //TODO: use JSON.net to parse this string and look at the error message
                        }
                    }
                }
            }

            //try
            //{
            //    var webResponse = (HttpWebResponse)httpRequest.GetResponse();
            //    using (var response = new StreamReader(webResponse.GetResponseStream()))
            //    {
            //        result.Add(true,response.ReadToEnd());
            //    }
            //    if (saveCookie)
            //        cookieData = webResponse.Cookies;
            //}
            //catch (WebException e)
            //{
            //    result.Add(false, e.Message.ToString());
            //}
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }
            return result;
        }

        public static Dictionary<bool, string> Febos_StatusDoc(string url, string method, string idfebos = "", string token = "", bool saveCookie = false)
        {
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            var result = new Dictionary<bool, string>();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("token", token);
            httpRequest.Headers.Add("empresa", Procesos.nit);
            httpRequest.Headers.Add("febosId", idfebos);
            httpRequest.Method = method.ToUpper();
            httpRequest.ServicePoint.Expect100Continue = false;
            httpRequest.CookieContainer = new CookieContainer();

            try
            {
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                        }
                    }

                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            string error = reader.ReadToEnd();
                            result.Add(false, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }
            return result;
        }

        public static Dictionary<bool, string> Febos_documentos(string url, string method, string body = "", string token = "", bool saveCookie = false)
        {
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            var result = new Dictionary<bool, string>();
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("token", token);
            httpRequest.Headers.Add("empresa", Procesos.nit);
            httpRequest.Method = method.ToUpper();
            httpRequest.ServicePoint.Expect100Continue = false;
            httpRequest.CookieContainer = new CookieContainer();

            if (!string.IsNullOrEmpty(body))
            {
                using (var requestStream = httpRequest.GetRequestStream())
                {
                    var writer = new StreamWriter(requestStream);
                    writer.Write(body);
                    writer.Close();
                }
            }

            try
            {
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                        }
                    }

                }
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                    {
                        using (var reader = new StreamReader(errorResponse.GetResponseStream(), Encoding.UTF8))
                        {
                            string error = reader.ReadToEnd();
                            result.Add(false, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }
            Procesos.requestSend = body;
            return result;
        }
    }
}