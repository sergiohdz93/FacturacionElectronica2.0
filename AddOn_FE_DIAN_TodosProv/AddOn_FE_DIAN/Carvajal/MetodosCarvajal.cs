using System;
using System.Net;

namespace AddOn_FE_DIAN.Carvajal
{
    class MetodosCarvajal
    {
        //Envio Factura Web Service proveedor Carvajal
        public static string UploadFileFE(string encodeFile, string tipoDoc, string docNum)
        {
            Procesos.responseStatus = "";
            string status = "";
            string idTrans = "";
            string nombreArchvio = "";
            switch (tipoDoc)
            {
                case "01":
                    nombreArchvio = "Factura de Venta " + docNum;
                    break;
                case "02":
                    nombreArchvio = "Factura de Exportación " + docNum;
                    break;
                case "03":
                    nombreArchvio = "Factura de Contingencia " + docNum;
                    break;
                case "91":
                    nombreArchvio = "Nota Crédito " + docNum; 
                    break;
                case "92":
                    nombreArchvio = "Nota Débito " + docNum;
                    break;
            }

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();
                CarvajalWS.UploadResponse resultadoCliente = new CarvajalWS.UploadResponse();

                var requestInterceptor = new CustomEndpointBehavior();
                client.Endpoint.Behaviors.Add(requestInterceptor);

                client.Open();

                CarvajalWS.Upload cuerpo = new CarvajalWS.Upload();
                CarvajalWS.UploadRequest uploadDoc = new CarvajalWS.UploadRequest();

                uploadDoc.fileName = nombreArchvio + ".txt";
                uploadDoc.fileData = encodeFile;

                if (Procesos.nit.IndexOf('_') > 0)
                {
                    uploadDoc.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('_'));
                }
                else if (Procesos.nit.IndexOf('-') > 0)
                {
                    uploadDoc.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('-'));
                }
                else
                {
                    uploadDoc.companyId = Procesos.nit;
                }

                uploadDoc.accountId = Procesos.nit;

                cuerpo.UploadRequest = uploadDoc;

                resultadoCliente = new CarvajalWS.UploadResponse();

                resultadoCliente = client.Upload(cuerpo.UploadRequest);

                idTrans = resultadoCliente.transactionId;

                client.Close();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("uploadFileFE: " + ex.Message);
            }
            return idTrans;
        }

        //Peticion web service estado documento
        public static void DocStatusFE(string codeLog, string transID, string request, Boolean ReSend, string strtext)
        {
            Procesos.responseStatus = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();
                CarvajalWS.DocumentStatusResponse resultadoCliente = null;
                //var testservice = new CustomClient(endpointname, endpoint);

                var requestInterceptor = new CustomEndpointBehavior();
                client.Endpoint.Behaviors.Add(requestInterceptor);

                //CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();

                client.Open();

                CarvajalWS.DocumentStatus cuerpo = new CarvajalWS.DocumentStatus();
                CarvajalWS.DocumentStatusRequest requestDocStatus = new CarvajalWS.DocumentStatusRequest();

                if (Procesos.nit.IndexOf('_') > 0)
                {
                    requestDocStatus.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('_'));
                }
                else if (Procesos.nit.IndexOf('-') > 0)
                {
                    requestDocStatus.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('-'));
                }
                else
                {
                    requestDocStatus.companyId = Procesos.nit;
                }

                requestDocStatus.accountId = Procesos.nit;
                requestDocStatus.transactionId = transID;

                cuerpo.DocumentStatusRequest = requestDocStatus;

                resultadoCliente = new CarvajalWS.DocumentStatusResponse();
                resultadoCliente = client.DocumentStatus(cuerpo.DocumentStatusRequest);

                if (resultadoCliente.processStatus == "FAIL")
                {
                    Procesos.EscribirLogFileTXT(resultadoCliente.errorMessage + "" + resultadoCliente.errorMessage);
                    
                }
                else if (resultadoCliente.processStatus == "PROCESSING")
                {
                    Procesos.EscribirLogFileTXT(resultadoCliente.errorMessage + "" + resultadoCliente.errorMessage);
                    
                }

                if(resultadoCliente != null)
                {
                    Procesos.UpdateLog(codeLog, transID, resultadoCliente, request, ReSend, strtext);
                }
                

                client.Close();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("DocStatusFE: " + ex.Message);
            }
        }

        //Peticion web service descarga documento
        public static string DownloadDocFE(string codeLog, string docType, string numDoc, string downloadType)
        {
            Procesos.responseStatus = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();
                CarvajalWS.DownloadResponse resultadoCliente = null;

                var requestInterceptor = new CustomEndpointBehavior();
                client.Endpoint.Behaviors.Add(requestInterceptor);

                client.Open();

                CarvajalWS.Download cuerpo = new CarvajalWS.Download();
                CarvajalWS.DownloadRequest requestDownloadDoc = new CarvajalWS.DownloadRequest();

                if (Procesos.nit.IndexOf('_') > 0)
                {
                    requestDownloadDoc.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('_'));
                }
                else if (Procesos.nit.IndexOf('-') > 0)
                {
                    requestDownloadDoc.companyId = Procesos.nit.Substring(0, Procesos.nit.IndexOf('-'));
                }
                else
                {
                    requestDownloadDoc.companyId = Procesos.nit;
                }
                requestDownloadDoc.accountId = Procesos.nit;
                requestDownloadDoc.documentType = docType;
                requestDownloadDoc.documentNumber = numDoc;
                requestDownloadDoc.resourceType = downloadType;

                cuerpo.DownloadRequest = requestDownloadDoc;

                resultadoCliente = new CarvajalWS.DownloadResponse();
                resultadoCliente = client.Download(cuerpo.DownloadRequest);
                Procesos.EscribirLogFileTXT(downloadType + resultadoCliente.status);
                //Procesos.EscribirLogFileTXT("Archivo" + resultadoCliente.downloadData);
                client.Close();
                return resultadoCliente.downloadData;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("DownloadDocFE: " + ex.Message);
                return ex.Message;
            }
        }
    }
}
