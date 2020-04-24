using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace AddOn_FE_DIAN.Certifactura
{
    class Servicios
    {
        public static Dictionary<bool, string> Emitir(string url, string method, string body = "", string token = "", bool saveCookie = false)
        {
            var result = new Dictionary<bool, string>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Procesos.username + ":" + Procesos.password));

                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                request.AddHeader("Authorization", "Basic " + svcCredentials);
                //request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json;charset=UTF-8,application/json", body, ParameterType.RequestBody);
                //Procesos.EscribirLogFileTXT(body);
                IRestResponse response = client.Execute(request);

                result.Add(true, response.Content);
            }
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }
            Procesos.requestSend = body;
            return result;
        }

        public static Dictionary<bool, string> ConsultaDoc(string url, string method, string body = "", string token = "", bool saveCookie = false)
        {
            var result = new Dictionary<bool, string>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                string svcCredentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Procesos.username + ":" + Procesos.password));

                var client = new RestClient(url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json;charset=UTF-8");
                request.AddHeader("Authorization", "Basic " + svcCredentials);
                //request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json;charset=UTF-8,application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

                result.Add(true, response.Content);
                Procesos.responseStatus = response.Content;
            }
            catch (Exception ex)
            {
                result.Add(false, ex.ToString());
            }

            Procesos.EscribirLogFileTXT(body);
            return result;
        }
    }
}
