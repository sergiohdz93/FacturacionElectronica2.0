using System;
using System.Net;

namespace AddOn_FE_DIAN.Carvajal
{
    class MetodosCarvajal
    {
        //Envio Factura Web Service proveedor Carvajal
        public static string UploadFileFE(string encodeFile, string docCode, string codelog)
        {
            Procesos.responseStatus = "";
            string status = "";
            string idTrans = "";
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();
                CarvajalWS.UploadResponse resultadoCliente = null;
                //var testservice = new CustomClient(endpointname, endpoint);

                var requestInterceptor = new CustomEndpointBehavior();
                client.Endpoint.Behaviors.Add(requestInterceptor);

                //CarvajalWS.invoiceServiceClient client = new CarvajalWS.invoiceServiceClient();

                client.Open();

                CarvajalWS.Upload cuerpo = new CarvajalWS.Upload();
                CarvajalWS.UploadRequest uploadDoc = new CarvajalWS.UploadRequest();

                uploadDoc.fileName = "Fac" + codelog + docCode + ".txt";
                uploadDoc.fileData = encodeFile;//"RU5DLElOVk9JQyw5MDA0NDA0NDEsMjA1MDU3NzkyOTEsVUJMIDIuMCxESUFOIDEuMCxGQUM0LDIwMTgtMDEtMDMsMDk6MDg6NTksMSxDT1AsMjAxOC0wMS0wMywsOTA1OTY3NDIsMDM1MCwyLDIwMTYtMTItMzENCkVNSSwxLDkwMDQ0MDQ0MSwzMSwyLDE3MDgxNixTUUEgQ09MT01CSUEgUy5BLixTUUEgQ09MT01CSUEgUy5BLixKYXZpZXIsQ2Fsdm8gQm9ycmVybyxBdi4gVW5pdmVyc2l0YXJpYSBTdXIgNTgzIERpcmVjY2lvbiBQcnVlYmEgU1FBIGRlIENvbG9tYmlhIFBydWViYSBRVUFMSVRZIFNFUlZJQ0lPUyBERSBJTlRFR1JBQ0lPTiBDVFMgQ08sVmFsbGUgZGVsIENhdWNhLFNhbnRpYWdvIGRlIENhbGksU2FudGlhZ28gZGUgQ2FsaSw3NjAwMDEsQ08sRUFOMDM1MA0KQ0RFLDEsSW5nIENhbGlkYWQsNjYxODE2MSxwcnVlYmFzc3FhQGdtYWlsLmNvbQ0KQURRLDIsOTAwNDQwNDQxLDEzLDAsMTgxMSxDYXJ2YWphbCBUZWNub2xvZ2lhIHkgU2VydmljaW9zIFMuQS5TLixDYXJ2YWphbCBUZWNub2xvZ2lhIHkgU2VydmljaW9zIFMuQS5TLixVc3VhcmlvLENhbGlkYWQsU2FudGEgUm9zYSA3NiBTYW50aWFnbyBTYW50aWFnbyBSTSAtIFIuTWV0cm9wb2xpdGFuYS1DT0xPTUJJQSxWYWxsZSBkZWwgQ2F1Y2EsQ2FsaSxDYWxpLDc2MDAwMSxDTywwMzUwDQpDREEsNSxEZXBhcnRhbWVudG8gY29tcHJhcyw2NjE4MTYxLGphdmllci5jYWx2b0BjYXJ2YWphbC5jb20NClRPVCwxMDAwMDAuMzUwLENPUCwxMDAwLjMwMCxDT1AsMTAwMDAwLjM1MCxDT1AsMTAwMC4zMDAsQ09QLDEwMC41MDAsQ09QLDEwMDAuMjUwLENPUCwxMDAwLjEsQ09QDQpUSU0sZmFsc2UsNDU2LjQ5MyxDT1ANCklNUCwwMSwxMDAwLjM1MCxDT1AsNDU2LjQ5MyxDT1AsMTYNClREQyxDT1AsQ09QLDMwMDAuMDAsMjAxNi0wOS0xMA0KQU5ULDEwMDAuMSxDT1AsMjAxNi0wOS0xMA0KRFNDLGZhbHNlLDEsMjg1LjU0LENPUCw0OCxEZXNjdWVudG8gZGUgcHJ1ZWJhLDEwMDAuMzUwLENPUCwxDQpEUkYsMTIzNDU2Nzg5MVNFLDIwMTctMTAtMjMsMjAxOC0wMS0wMyxGQUMsMSw5OTk5OTk5OQ0KUUZBLDg5MDIwOTYxMiw4LDAsU1FBIENPTE9NQklBIFMuQS4sU1FBIENPTE9NQklBIFMuQS4sUHJ1ZWJhcyxDYWxpZGFkLEF2LiBVbml2ZXJzaXRhcmlhIFN1ciA1ODMgRGlyZWNjaW9uIFBydWViYSBTUUEgZGUgQ29sb21iaWEgUHJ1ZWJhIFFVQUxJVFkgU0VSVklDSU9TIERFIElOVEVHUkFDSU9OIENUUyBDTyxWYWxsZSBkZWwgQ2F1Y2EsU2FudGlhZ28gZGUgQ2FsaSxTYW50aWFnbyBkZSBDYWxpLDc2MDAwMSxDTywxNzA4MTYsMDM1MA0KQVFGLDkwMDQ0MDQ0MSwzMSwwLENhcnZhamFsIFRlY25vbG9naWEgeSBTZXJ2aWNpb3MgUy5BLlMuLENhcnZhamFsIFRlY25vbG9naWEgeSBTZXJ2aWNpb3MgUy5BLlMuLENhbGlkYWQsUHJ1ZWJhcyxTYW50YSBSb3NhIDc2IFNhbnRpYWdvIFNhbnRpYWdvIFJNIC0gUi5NZXRyb3BvbGl0YW5hLUNPTE9NQklBLFZhbGxlIGRlbCBDYXVjYSxDYWxpLENhbGksNzYwMDEsQ08sMTgxMSwwMzUwDQpOT1QsQ29uZGljaW9uUGFnb3w0NSBkaWFzIGZlY2hhIGZhY3R1cmENCk5PVCxDdWVudGF8UmVhbGl6YXIgcGFnbyBhIHRyYXZleiBkZSBsYXMgc2lndWllbnRlcyBjdWVudGFzIGNvcnJpZW50ZXMgYSBOb21icmUgZGUgU1FBIENPTE9NQklBIEJBTkNPIERFIEJPR09UQSBYWFhYWA0KTk9ULEVuY2FyZ29Db21lcmNpYWx8MDAxMDENCk5PVCxJbmNvbnRlcm18MzM3NzMNCk5PVCxUZXh0b0xpYnJlRmFjdHVyYXxFc3RhIGZhY3R1cmEgc2UgYXNpbWlsYSBhIHVuYSBsZXRyYSBkZSBjYW1iaW8uIEVsIGNvbXByYWRvciBkZWNsYXJhIGhhYmVyIHJlY2liaWRvIGxhIG1lcmNhbmNpYSBjb250ZW5pZGEgZW4gZXN0YSBmYWN0dXJhLi4uDQpOT1QsR2xvc2FNb250b3xWRUlOVElPQ0hPIE1JTCBRVUlOSUVOVE9TIENJTkNVRU5UQSBZIENVQVRSTyBZIDgwLzEwMCBQRVNPUw0KTk9ULFB1ZXJ0b0VtYmFycXVlfENhbGxhbw0KTk9ULFB1ZXJ0b0Rlc3Rpbm98VmFscGFyYWlzbw0KTk9ULEZsZXRlfDM0MC4wMA0KTk9ULFNlZ3Vyb3w2LjIxDQpOT1QsT3Ryb3NHYXN0b3N8MC4wMA0KTk9ULFRyYW5zcG9ydGV8VHJhbnNwb3J0ZSBtYXJpdGltbw0KTk9ULENvbnRlbmVkb3Jlc3w0DQpOT1QsUGVzb3w4NTEwLjE1Nw0KTk9ULFBlc29OZXRvfDEwMDANCk5PVCxQYXJ0QXJhbmNlbGFyaWF8ODU0NDQ5OTA5MA0KTk9ULENvbmRQYWdvfFRyYW5zZmVyZW5jaWEgYmFuY2FyaWEgOTAgZGlhcw0KTk9ULFRlbGVmb25vfDU2LTItMjM1MzQ0MDANCk5PVCxDaXVFbXB8WVVNQk8NCk5PVCxDb3JyZW9QZXhwfFBSVUVCQVNAU1FBLkNPTQ0KTk9ULFRvdEZjYXwyODIwOC41OQ0KTk9ULFBlZGlkb3wzNDcxNDkNCk5PVCxEZXNjdG9Ub3RhbHwwLjAwJQ0KTk9ULE1lbnNhamVzQXQNCk5PVCwxfFJFRi8wMjYtMDAwMTkwMg0KTk9ULElWQXxJVkEgUslHSU1FTiBDT03aTg0KTk9ULElDQXxJQ0EgQUNUSVZJREFEIDEwMzgzMCAtIEFDVUVSRE8gMDM0IC0gMjAwNQ0KTk9ULENvbnRyaWJ1eWVudGVzfFNPTU9TIEdSQU5ERVMgQ09OVFJJQlVZRU5URVMgUkVTT0xVQ0lPTiAwMDAwNDEgMzAgRU5FLiBERSAyMDE0DQpOT1QsQXV0b3JldGVuZWRvcmVzfFNPTU9TIEFVVE9SRVRFTkVET1JFUyBSRVNPTFVDSU9OIERJQU4gMDM4ODIgMDkgQUJSSUwgMjAwNw0KTk9ULE5BTHxSRVNPTFVDSU9OIERFIEZBQ1RVUkFDSU9OIEVMRUNUUk9OSUNBIENQIE5vICBkZSAyMDE2IC8vICAgZGVsIENQICBhbCAgQ1ANCk5PVCxFWFB8UkVTT0xVQ0lPTiBERSBGQUNUVVJBQ0lPTiBFTEVDVFJPTklDQSBFWCBObyAgZGUgMjAxNSAvLyAgIGRlbCBFWCAgYWwgIEVYDQpPUkMsUUE0NTAwMTM2MTg4LTE5MCwyMDE2LTA5LTA5LDA5OjA5OjA4LDIwMTYwMA0KUkVGLEFBSiw4NzU3NDgzLDIwMTYtMDgtMTYNClJFRixJVixGMDAyNi0wMDAxOTAsMjAxNi0wOC0xNg0KUkVGLElWLEYwMDI2LTAwMDE5MCwyMDE2LTA4LTE2DQpJRU4sY2FsbGUgMTIgTm8gNDMzLEN1bmRpbmFtYXJjYSxCb2dvdGEsQm9nb3RhLCxDTyxDQVJWQUpBTCBTRVJWSUNJT1MgU0FTLDIwMTYtMDktMTMsMDk6MDc6NTQsMjA1MDU3NzkyOTQsMjAxNi0wOS0xMyxDT0RDQUxJMDAxDQpURVQsUG9ydGVzIFBhZ2Fkb3MsREVTLENPRENBTEkwMDEsY2FsbGUgMjlOICMgNkFODQpDVFMsQ1RZUzAwLCwNClJCQyw2MzAyNSxyZWZlcmVuY2lhMTYwNSxyZWZlcmVuY2lhMTYwNg0KSVRFLDEsZmFsc2UsNzU1OC45ODYsTkFSLDIzNTI0LjI4NCxDT1AsMy4xMTI1LENPUCwxMDAzNDI2NFFBLEFydGljdWxvIGRlIFBydWViYSBMTSBTUUEgQ08sQ0IvTkEyWFkgMC42LzEga1YgMjQwIG1tMiBORUdSTyBBcnRpY3VsbyBwcnVlYmFzIGRlc2NyaXBjaW9uIHBhcnRpZGEsLCwsLERBVE9TIFRFQ05JQ09TIEFTT2NpYWRvcyBhbCBhcnRpY3VsbzEyJSwsMTAwMzQyNjRRQSwyMzgxMy4yMDAsQ09QLCwsLA0KTVlNLE1hcmNhLk1hcmNhIGRlbCBhcnTtY3VsbyBPYmxpZ2F0b3JpbyBzaSBsYSBmYWN0dXJhIGVzIGludGVybmFjaW9uYWwsTW9kZWxvLk1vZGVsbyBkZWwgYXJ07WN1bG8uT2JsaWdhdG9yaW8gc2kgbGEgZmFjdHVyYSBlcyBpbnRlcm5hY2lvbmFsLjINCklERSxmYWxzZSwxMjM0NTY3ODkwMTIzNDUuMTIzLENPUCwzNSxURVhUTyBMSUJSRSBkZSByYXpvbiAxMjMlLDIzLCwsDQpJVEUsMixmYWxzZSwxNTA1LE5BUiw0Njg0LjMxLENPUCwzLjExMjUsQ09QLDEwMDM0MjY0LExNLENCL05BMlhZIDAuNi8xIGtWIDI0MCBtbTIgTkVHUk8sLCwsLCwsMTAwMzQyNjQsNDc0MS44MCxDT1AsLCws";

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

                status = resultadoCliente.status;
                idTrans = resultadoCliente.transactionId;

                Procesos.RepoMensajes(idTrans, status);

                client.Close();
            }
            catch (Exception ex)
            {
                Procesos.RepoMensajes("catch", ex.Message);
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
                    Procesos.RepoMensajes(resultadoCliente.errorMessage, resultadoCliente.errorMessage);
                }
                else if (resultadoCliente.processStatus == "PROCESSING")
                {
                    Procesos.EscribirLogFileTXT(resultadoCliente.errorMessage + "" + resultadoCliente.errorMessage);
                    Procesos.RepoMensajes(resultadoCliente.errorMessage, resultadoCliente.processName);
                }

                if(resultadoCliente != null)
                {
                    Procesos.UpdateLog(codeLog, transID, resultadoCliente, request, ReSend, strtext);
                }
                

                client.Close();
            }
            catch (Exception ex)
            {
                Procesos.RepoMensajes("catch", ex.Message);
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
