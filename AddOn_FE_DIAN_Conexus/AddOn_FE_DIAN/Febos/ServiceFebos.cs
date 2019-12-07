using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

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
                Procesos.EscribirLogFileTXT("StatusDoc: Headers" + httpRequest.Headers);
                Procesos.EscribirLogFileTXT("StatusDoc: RequestUri" + httpRequest.RequestUri);
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                            Procesos.EscribirLogFileTXT("StatusDoc_webResponse: " + result[true]);
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
                            Procesos.EscribirLogFileTXT("StatusDoc_WebException: " + error);
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
                    Procesos.EscribirLogFileTXT("documentos: requestStream" + body);
                    var writer = new StreamWriter(requestStream);
                    writer.Write(body);
                    writer.Close();
                }
            }

            try
            {
                Procesos.EscribirLogFileTXT("documentos: Headers" + httpRequest.Headers);
                Procesos.EscribirLogFileTXT("documentos: RequestUri" + httpRequest.RequestUri);
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                            Procesos.EscribirLogFileTXT("documentos_webResponse: " + result[true]);
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
                            Procesos.EscribirLogFileTXT("documentos_WebException: " + error);
                            result.Add(false, error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("Exception: " + ex);
                result.Add(false, ex.ToString());
            }
            Procesos.requestSend = body;
            return result;
        }

        public static Dictionary<bool, string> Febos_folio(string url, string method, string token = "", bool saveCookie = false)
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

            try
            {
                Procesos.EscribirLogFileTXT("Febos_folio: Headers" + httpRequest.Headers);
                Procesos.EscribirLogFileTXT("Febos_folio: RequestUri" + httpRequest.RequestUri);
                using (var webResponse = httpRequest.GetResponse() as HttpWebResponse)
                {
                    if (httpRequest.HaveResponse && webResponse != null)
                    {
                        using (var response = new StreamReader(webResponse.GetResponseStream()))
                        {
                            result.Add(true, response.ReadToEnd());
                            Procesos.EscribirLogFileTXT("folio_webResponse: " + result[true]);
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
                            Procesos.EscribirLogFileTXT("folio_WebException: " + error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }
            //Procesos.requestSend = body;
            return result;
        }
    }
}