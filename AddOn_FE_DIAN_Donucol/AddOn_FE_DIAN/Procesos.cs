using SAPbouiCOM;
using SAPbobsCOM;
using System;
using System.Drawing;
using System.Linq;
using System.Data;
using System.IO;
using System.Xml;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using AddOn_FE_DIAN.Carvajal;
using AddOn_FE_DIAN.Controllers;
using System.Text;

namespace AddOn_FE_DIAN
{
    class Procesos
    {
        public static Application SBO_Application;
        public static Grid oGrid;
        public static SAPbobsCOM.Company oCompany;
        public static Form oForm = null;
        public static Recordset oRS;
        public static EditTextColumn oCol;
        //public static Recordset oRecordset;
        public static System.Timers.Timer aTimer, bTimer;
        public static int lRetCode;
        public static string sErrMsg;
        public static string sSQL;        
        public static bool senalActiva = true;
        public static bool banderaReenviar = true;
        public static bool banderaVerificaEstados = true;
        public static bool banderaAgregarDoc = true;
        public static string proveedor = "";
        public static string nit = "";
        public static string username = "";
        public static string password = "";
        public static string token = "";
        public static int LogCode;
        public static string requestSend = "";
        public static string responseStatus = "";
        public static string user = "";
        public static DateTime dateSend;
        public static Item oItem;
        public static string FileLog = "FE_DIAN_LOG";

        //Inicializacion de eventos
        public Procesos(SAPbobsCOM.Company oCmpn, Application SBO_App)
        {
            try
            {
                oCompany = oCmpn;
                SBO_Application = SBO_App;
                //Creacion de timer para actualziacion de formulario Monitor Log
                StartMonitorSAPB1();
                //Cargue inicial de parametrizacion
                user = SBO_Application.Company.UserName;
                CargueInicial();
                SBO_Application.FormDataEvent += new _IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormDataEvent);
            }
            catch (Exception ex)
            {
                SBO_Application.SetStatusBarMessage("Exception " + ex.Message, BoMessageTime.bmt_Medium, false);
                Procesos.EscribirLogFileTXT("Procesos: " + ex.Message);
            }
        }

        //Cargue incial de informacion de proveedor
        public static void CargueInicial()
        {
            try
            {
                int i = 0;
                oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                sSQL = Querys.Default.PARAMG;
                oRS.DoQuery(sSQL);
                i = oRS.RecordCount;

                if (i > 0)
                {
                    proveedor = oRS.Fields.Item("U_Proveedor").Value.ToString();
                    nit = oRS.Fields.Item("U_NIT_Emisor").Value.ToString();
                    username = oRS.Fields.Item("U_Email_Usuario").Value.ToString();
                    password = oRS.Fields.Item("U_Clave_Usuario").Value.ToString();
                    token = oRS.Fields.Item("U_Token").Value.ToString();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                oRS = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("CargueInicial: " + ex.Message);
            }
        }

        //captura de eventos de creacion de documentos o registros
        private void SBO_Application_FormDataEvent(ref BusinessObjectInfo BusinessObjectInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                string docnum = "";
                string objtype = "";
                string docentry = "";
                string docDate = "";
                string docTime = "";
                bool docDIAN;
                string estadoInterf;

                oForm = SBO_Application.Forms.ActiveForm;

                //Formulario 133 Factura de Venta
                if (oForm.Type == 133 || oForm.Type == -133)
                {
                    if (BusinessObjectInfo.Type == "13")
                    {
                        //Before Event 
                        if ((BusinessObjectInfo.BeforeAction == false))
                        {
                            try
                            {
                                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD && BusinessObjectInfo.ActionSuccess)
                                {
                                    Procesos.EscribirLogFileTXT("133 FacturaVenta: Inico");
                                    //oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                    SAPbobsCOM.CompanyService oCmpSrv;
                                    SAPbobsCOM.SeriesService oSeriesService;
                                    Series oSeries;
                                    SeriesParams oSeriesParams;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    // set the number of an existing series


                                    Form form = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID);
                                    BusinessObject bisObj = form.BusinessObject;
                                    string uid = bisObj.Key;


                                    //Test DI method GetByKeys using key recived from UI (IBusinessObjectInfo.UniqueId) 
                                    SAPbobsCOM.Documents oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //Obtener inofrmacion del documento creado
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    docnum = Convert.ToString(oInvoice.DocNum);
                                    objtype = Convert.ToString(oInvoice.DocObjectCode);
                                    docentry = Convert.ToString(oInvoice.DocEntry);
                                    docDate = Convert.ToString(oInvoice.DocDate);
                                    docTime = Convert.ToString(oInvoice.DocTime);
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);
                                    SAPbobsCOM.UserTables tbls = null;
                                    SAPbobsCOM.UserTable tbl = null;

                                    tbls = oCompany.UserTables;
                                    tbl = tbls.Item("FEDIAN_CODDOC");
                                    docDIAN = tbl.GetByKey(oSeries.Remarks);

                                    tbl = tbls.Item("FEDIAN_INTERF_CFG");
                                    tbl.GetByKey(oSeries.Remarks);
                                    estadoInterf = tbl.UserFields.Fields.Item("U_WS_Activo").Value;
                                    Procesos.EscribirLogFileTXT("133 FacturaVenta: Estado" + estadoInterf + "docDian: " + docDIAN);
                                    if (docDIAN == true & estadoInterf == "Y")
                                    {
                                        int newCode = 0;
                                        //Añadir registro el tabala del Monitor Log
                                        tbls = null;
                                        tbl = null;

                                        tbls = oCompany.UserTables;
                                        tbl = tbls.Item("FEDIAN_MONITORLOG");
                                        Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                        oRecordset.DoQuery(string.Format(Querys.Default.MaxLog));

                                        newCode = oRecordset.Fields.Item("NextCode").Value;
                                        Procesos.EscribirLogFileTXT("133 FacturaVenta: Code" + newCode);
                                        tbl.Code = Convert.ToString(newCode);
                                        tbl.Name = Convert.ToString(newCode);
                                        tbl.UserFields.Fields.Item("U_DocType").Value = oSeries.Remarks;
                                        tbl.UserFields.Fields.Item("U_Folio").Value = docnum;
                                        tbl.UserFields.Fields.Item("U_ObjType").Value = BusinessObjectInfo.Type;
                                        tbl.UserFields.Fields.Item("U_DocNum").Value = docentry;
                                        tbl.UserFields.Fields.Item("U_Usuario_Envio").Value = user;
                                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = docDate;
                                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = docTime;
                                        tbl.UserFields.Fields.Item("U_Resultado").Value = "";
                                        tbl.UserFields.Fields.Item("U_Status").Value = "";
                                        tbl.UserFields.Fields.Item("U_ProcessID").Value = "";
                                        tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Det_Peticion").Value = "";
                                        tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                                        tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = "";

                                        lRetCode = tbl.Add();

                                        //Vereficar si se añade registro en la tabla
                                        if (lRetCode != 0)
                                        {
                                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                                            //oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            //SBO_Application.MessageBox(sErrMsg);
                                        }
                                        else
                                        {
                                            oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                            oRecordset.DoQuery(string.Format(Querys.Default.GetCodeLog, docentry));
                                            LogCode = Convert.ToInt32(oRecordset.Fields.Item("Code").Value);
                                            Procesos.EscribirLogFileTXT("133 FacturaVenta: InicioSendFE");
                                            SendFE(docentry, LogCode, oSeries.Remarks, false);
                                            Procesos.EscribirLogFileTXT("133 FacturaVenta: FinSendFE");
                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                                            oRecordset = null;
                                            GC.Collect();
                                        }
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                                        tbl = null;
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                                        tbls = null;
                                        GC.Collect();
                                    }                                
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("133-Factura: " + ex.Message);
                            }
                        }
                        else
                        {
                        
                        }
                    }
                }
                //Formulario 60091 Factura de Venta Reserva
                if (oForm.Type == 60091 || oForm.Type == -60091)
                {
                    if (BusinessObjectInfo.Type == "13")
                    {
                        //Before Event 
                        if ((BusinessObjectInfo.BeforeAction == false))
                        {
                            try
                            {
                                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD && BusinessObjectInfo.ActionSuccess)
                                {
                                    Procesos.EscribirLogFileTXT("60091 FacturaReserva: Inico");
                                    //oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                    SAPbobsCOM.CompanyService oCmpSrv;
                                    SAPbobsCOM.SeriesService oSeriesService;
                                    Series oSeries;
                                    SeriesParams oSeriesParams;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    // set the number of an existing series


                                    Form form = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID);
                                    BusinessObject bisObj = form.BusinessObject;
                                    string uid = bisObj.Key;


                                    //Test DI method GetByKeys using key recived from UI (IBusinessObjectInfo.UniqueId) 
                                    SAPbobsCOM.Documents oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //Obtener inofrmacion del documento creado
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    docnum = Convert.ToString(oInvoice.DocNum);
                                    objtype = Convert.ToString(oInvoice.DocObjectCode);
                                    docentry = Convert.ToString(oInvoice.DocEntry);
                                    docDate = Convert.ToString(oInvoice.DocDate);
                                    docTime = Convert.ToString(oInvoice.DocTime);
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);
                                    SAPbobsCOM.UserTables tbls = null;
                                    SAPbobsCOM.UserTable tbl = null;

                                    tbls = oCompany.UserTables;
                                    tbl = tbls.Item("FEDIAN_CODDOC");
                                    docDIAN = tbl.GetByKey(oSeries.Remarks);

                                    tbl = tbls.Item("FEDIAN_INTERF_CFG");
                                    tbl.GetByKey(oSeries.Remarks);
                                    estadoInterf = tbl.UserFields.Fields.Item("U_WS_Activo").Value;
                                    Procesos.EscribirLogFileTXT("60091 FacturaReserva: Estado" + estadoInterf + "docDian: " + docDIAN);
                                    if (docDIAN == true & estadoInterf == "Y")
                                    {
                                        int newCode = 0;
                                        //Añadir registro el tabala del Monitor Log
                                        tbls = null;
                                        tbl = null;

                                        tbls = oCompany.UserTables;
                                        tbl = tbls.Item("FEDIAN_MONITORLOG");
                                        Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                        oRecordset.DoQuery(string.Format(Querys.Default.MaxLog));

                                        newCode = oRecordset.Fields.Item("NextCode").Value;
                                        Procesos.EscribirLogFileTXT("60091 FacturaReserva: Code" + newCode);
                                        tbl.Code = Convert.ToString(newCode);
                                        tbl.Name = Convert.ToString(newCode);
                                        tbl.UserFields.Fields.Item("U_DocType").Value = oSeries.Remarks;
                                        tbl.UserFields.Fields.Item("U_Folio").Value = docnum;
                                        tbl.UserFields.Fields.Item("U_ObjType").Value = BusinessObjectInfo.Type;
                                        tbl.UserFields.Fields.Item("U_DocNum").Value = docentry;
                                        tbl.UserFields.Fields.Item("U_Usuario_Envio").Value = user;
                                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = docDate;
                                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = docTime;
                                        tbl.UserFields.Fields.Item("U_Resultado").Value = "";
                                        tbl.UserFields.Fields.Item("U_Status").Value = "";
                                        tbl.UserFields.Fields.Item("U_ProcessID").Value = "";
                                        tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Det_Peticion").Value = "";
                                        tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                                        tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = "";

                                        lRetCode = tbl.Add();

                                        //Vereficar si se añade registro en la tabla
                                        if (lRetCode != 0)
                                        {
                                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                                            //oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            //SBO_Application.MessageBox(sErrMsg);
                                        }
                                        else
                                        {
                                            oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                            oRecordset.DoQuery(string.Format(Querys.Default.GetCodeLog, docentry));
                                            LogCode = Convert.ToInt32(oRecordset.Fields.Item("Code").Value);
                                            Procesos.EscribirLogFileTXT("60091 FacturaReserva: InicioSendFE");
                                            SendFE(docentry, LogCode, oSeries.Remarks, false);
                                            Procesos.EscribirLogFileTXT("60091 FacturaReserva: FinSendFE");

                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                                            oRecordset = null;
                                            GC.Collect();
                                        }
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                                        tbl = null;
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                                        tbls = null;
                                        GC.Collect();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("60091-FacturaReserva: " + ex.Message);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                //Formulario 65303 Nota Debito
                if (oForm.Type == 65303 || oForm.Type == -65303)
                {
                    if (BusinessObjectInfo.Type == "13")
                    {
                        //Before Event 
                        if ((BusinessObjectInfo.BeforeAction == false))
                        {
                            try
                            {
                                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD && BusinessObjectInfo.ActionSuccess)
                                {
                                    Procesos.EscribirLogFileTXT("65303 NotaDebito: Inico");
                                    //oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                    SAPbobsCOM.CompanyService oCmpSrv;
                                    SAPbobsCOM.SeriesService oSeriesService;
                                    Series oSeries;
                                    SeriesParams oSeriesParams;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    // set the number of an existing series

                                    Form form = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID);
                                    BusinessObject bisObj = form.BusinessObject;
                                    string uid = bisObj.Key;


                                    //Test DI method GetByKeys using key recived from UI (IBusinessObjectInfo.UniqueId) 
                                    SAPbobsCOM.Documents oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    //Obtener inofrmacion del documento creado
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    docnum = Convert.ToString(oInvoice.DocNum);
                                    objtype = Convert.ToString(oInvoice.DocObjectCode);
                                    docentry = Convert.ToString(oInvoice.DocEntry);
                                    docDate = Convert.ToString(oInvoice.DocDate);
                                    docTime = Convert.ToString(oInvoice.DocTime);
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);
                                    SAPbobsCOM.UserTables tbls = null;
                                    SAPbobsCOM.UserTable tbl = null;

                                    tbls = oCompany.UserTables;
                                    tbl = tbls.Item("FEDIAN_CODDOC");
                                    docDIAN = tbl.GetByKey(oSeries.Remarks);

                                    tbl = tbls.Item("FEDIAN_INTERF_CFG");
                                    tbl.GetByKey(oSeries.Remarks);
                                    estadoInterf = tbl.UserFields.Fields.Item("U_WS_Activo").Value;
                                    Procesos.EscribirLogFileTXT("65303 NotaDebito: Estado" + estadoInterf + "docDian: " + docDIAN);
                                    if (docDIAN == true & estadoInterf == "Y")
                                    {
                                        int newCode = 0;
                                        //Añadir registro el tabala del Monitor Log
                                        tbls = null;
                                        tbl = null;

                                        tbls = oCompany.UserTables;
                                        tbl = tbls.Item("FEDIAN_MONITORLOG");
                                        Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                        oRecordset.DoQuery(string.Format(Querys.Default.MaxLog));

                                        newCode = oRecordset.Fields.Item("NextCode").Value;
                                        Procesos.EscribirLogFileTXT("60091 FacturaReserva: Code" + newCode);
                                        tbl.Code = Convert.ToString(newCode);
                                        tbl.Name = Convert.ToString(newCode);
                                        tbl.UserFields.Fields.Item("U_DocType").Value = oSeries.Remarks;
                                        tbl.UserFields.Fields.Item("U_Folio").Value = docnum;
                                        tbl.UserFields.Fields.Item("U_ObjType").Value = BusinessObjectInfo.Type;
                                        tbl.UserFields.Fields.Item("U_DocNum").Value = docentry;
                                        tbl.UserFields.Fields.Item("U_Usuario_Envio").Value = user;
                                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = docDate;
                                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = docTime;
                                        tbl.UserFields.Fields.Item("U_Resultado").Value = "";
                                        tbl.UserFields.Fields.Item("U_Status").Value = "";
                                        tbl.UserFields.Fields.Item("U_ProcessID").Value = "";
                                        tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Det_Peticion").Value = "";
                                        tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                                        tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = "";

                                        lRetCode = tbl.Add();

                                        //Vereficar si se añade registro en la tabla
                                        if (lRetCode != 0)
                                        {
                                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                                            //oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            //SBO_Application.MessageBox(sErrMsg);
                                        }
                                        else
                                        {
                                            oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                            oRecordset.DoQuery(string.Format(Querys.Default.GetCodeLog, docentry));
                                            LogCode = Convert.ToInt32(oRecordset.Fields.Item("Code").Value);
                                            Procesos.EscribirLogFileTXT("65303 NotaDebito: InicioSendFE");
                                            SendFE(docentry, LogCode, oSeries.Remarks, false);
                                            Procesos.EscribirLogFileTXT("65303 NotaDebito: FinSendFE");

                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                                            oRecordset = null;
                                            GC.Collect();
                                        }
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                                        tbl = null;
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                                        tbls = null;
                                        GC.Collect();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("65303-NotaDebito: " + ex.Message);
                            }
                        }
                        else
                        {

                        }
                    }
                }
                //Formulario 179 Nota Credito de Venta
                if (oForm.Type == 179 || oForm.Type == -179)
                {
                    if (BusinessObjectInfo.Type == "14")
                    {
                        //Before Event 
                        if ((BusinessObjectInfo.BeforeAction == false))
                        {
                            try
                            {
                                if (BusinessObjectInfo.EventType == BoEventTypes.et_FORM_DATA_ADD && BusinessObjectInfo.ActionSuccess)
                                {
                                    Procesos.EscribirLogFileTXT("179 NotaCredito: Inico");
                                    //oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                    SAPbobsCOM.CompanyService oCmpSrv;
                                    SAPbobsCOM.SeriesService oSeriesService;
                                    Series oSeries;
                                    SeriesParams oSeriesParams;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    // set the number of an existing series

                                    Form form = SBO_Application.Forms.Item(BusinessObjectInfo.FormUID);
                                    BusinessObject bisObj = form.BusinessObject;
                                    string uid = bisObj.Key;


                                    //Test DI method GetByKeys using key recived from UI (IBusinessObjectInfo.UniqueId) 
                                    SAPbobsCOM.Documents oCreditNote = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                                    //oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQuotations);
                                    oCreditNote.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    docnum = Convert.ToString(oCreditNote.DocNum);
                                    objtype = Convert.ToString(oCreditNote.DocObjectCode);
                                    docentry = Convert.ToString(oCreditNote.DocEntry);
                                    docDate = Convert.ToString(oCreditNote.DocDate);
                                    docTime = Convert.ToString(oCreditNote.DocTime);
                                    oSeriesParams.Series = oCreditNote.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);
                                    SAPbobsCOM.UserTables tbls = null;
                                    SAPbobsCOM.UserTable tbl = null;

                                    tbls = oCompany.UserTables;
                                    tbl = tbls.Item("FEDIAN_CODDOC");
                                    docDIAN = tbl.GetByKey(oSeries.Remarks);

                                    tbl = tbls.Item("FEDIAN_INTERF_CFG");
                                    tbl.GetByKey(oSeries.Remarks);
                                    estadoInterf = tbl.UserFields.Fields.Item("U_WS_Activo").Value;
                                    Procesos.EscribirLogFileTXT("179 NotaCredito: Estado" + estadoInterf + "docDian: " + docDIAN);
                                    if (docDIAN == true & estadoInterf == "Y")
                                    {
                                        int newCode = 0;
                                        //Añadir registro el tabala del Monitor Log
                                        tbls = null;
                                        tbl = null;

                                        tbls = oCompany.UserTables;
                                        tbl = tbls.Item("FEDIAN_MONITORLOG");
                                        Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                        oRecordset.DoQuery(string.Format(Querys.Default.MaxLog));

                                        newCode = oRecordset.Fields.Item("NextCode").Value;
                                        Procesos.EscribirLogFileTXT("179 NotaCredito: Code" + newCode);
                                        tbl.Code = Convert.ToString(newCode);
                                        tbl.Name = Convert.ToString(newCode);
                                        tbl.UserFields.Fields.Item("U_DocType").Value = oSeries.Remarks;
                                        tbl.UserFields.Fields.Item("U_Folio").Value = docnum;
                                        tbl.UserFields.Fields.Item("U_ObjType").Value = BusinessObjectInfo.Type;
                                        tbl.UserFields.Fields.Item("U_DocNum").Value = docentry;
                                        tbl.UserFields.Fields.Item("U_Usuario_Envio").Value = user;
                                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = docDate;
                                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = docTime;
                                        tbl.UserFields.Fields.Item("U_Resultado").Value = "";
                                        tbl.UserFields.Fields.Item("U_Status").Value = "";
                                        tbl.UserFields.Fields.Item("U_ProcessID").Value = "";
                                        tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = "";
                                        tbl.UserFields.Fields.Item("U_Det_Peticion").Value = "";
                                        tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                                        tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = "";

                                        lRetCode = tbl.Add();

                                        //Vereficar si se añade registro en la tabla
                                        if (lRetCode != 0)
                                        {
                                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                                            //oCompany.GetLastError(out lRetCode, out sErrMsg);
                                            //SBO_Application.MessageBox(sErrMsg);
                                        }
                                        else
                                        {
                                            oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                                            oRecordset.DoQuery(string.Format(Querys.Default.GetCodeLog, docentry));
                                            LogCode = Convert.ToInt32(oRecordset.Fields.Item("Code").Value);
                                            Procesos.EscribirLogFileTXT("179 NotaCredito: InicioSendFE");
                                            SendFE(docentry, LogCode, oSeries.Remarks, false);
                                            Procesos.EscribirLogFileTXT("179 NotaCredito: FinSendFE");

                                            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                                            oRecordset = null;
                                            GC.Collect();
                                        }
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                                        tbl = null;
                                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                                        tbls = null;
                                        GC.Collect();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("179-NotaCredito: " + ex.Message);
                            }
                        }
                        else
                        {
                        
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("FORM_DATA_ADD: " + ex.Message);
            }
        }

        //Definicion timer
        public static void StartMonitorSAPB1()
        {
            #region TimerVerificaEstados
            // Alternate method: create a Timer with an interval argument to the constructor.
            //aTimer = new System.Timers.Timer(2000);

            // Create a timer with a five second interval.
            aTimer = new System.Timers.Timer(Properties.Settings.Default.TimerStatus);

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEventStatus;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
            #endregion TimerVerificaEstados

            #region TimerReSend
            // Alternate method: create a Timer with an interval argument to the constructor.
            //aTimer = new System.Timers.Timer(2000);

            // Create a timer with a five second interval.
            bTimer = new System.Timers.Timer(Properties.Settings.Default.TimerResend);

            // Hook up the Elapsed event for the timer. 
            bTimer.Elapsed += OnTimedEventReSend;

            // Have the timer fire repeated events (true is the default)
            bTimer.AutoReset = true;

            // Start the timer
            bTimer.Enabled = true;
    #endregion TimerReSend

            #region TimerAddDTE
            // Alternate method: create a Timer with an interval argument to the constructor.
            //aTimer = new System.Timers.Timer(2000);

            // Create a timer with a five second interval.
            bTimer = new System.Timers.Timer(Properties.Settings.Default.TimerResend);

            // Hook up the Elapsed event for the timer. 
            bTimer.Elapsed += OnTimedEventAddDTEMonitor;

            // Have the timer fire repeated events (true is the default)
            bTimer.AutoReset = true;

            // Start the timer
            bTimer.Enabled = true;
            #endregion TimerAddDTE
        }

        //Timer verificar estado
        public static void OnTimedEventStatus(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (banderaVerificaEstados == true)
                {
                    banderaVerificaEstados = false;
                    Verifystatus();
                    banderaVerificaEstados = true;
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("TimerVerificaEstados: " + ex.Message);
                banderaVerificaEstados = true;
            }
        }

        //Timer Reenviar Fallidos
        public static void OnTimedEventReSend(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (banderaReenviar == true)
                {
                    banderaReenviar = false;
                    AutoReSend();
                    banderaReenviar = true;
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("OnTimedEventReSend: " + ex.Message);
                banderaReenviar = true;
            }
        }

        //Timer Agregar DTE al monitor
        public static void OnTimedEventAddDTEMonitor(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (banderaAgregarDoc == true && senalActiva == true)
                {
                    banderaAgregarDoc = false;
                    AddDTEMonitor();
                    banderaAgregarDoc = true;
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("OnTimedEventAddDTEMonitor: " + ex.Message);
                banderaAgregarDoc = true;
            }
        }

        //Actualizacion de DataGrid si el formulario esta abierto
        public static void UpdateFormLogOpen()
        {
            try
            {
                UpdGridLogtimer("FORM_FE_0008");
            }
            catch (Exception ex)
            {
                SBO_Application.SetStatusBarMessage("Exception " + ex.Message, BoMessageTime.bmt_Medium, false);
                Procesos.EscribirLogFileTXT("UpdateFormLogOpen: " + ex.Message);
            }
        }

        //Funcion que se ejecuta para actualizar el formualio Monitor Log
        public static void UpdGridLogtimer(string form)
        {
            string sSQL = "";
            try
            {
                oForm = SBO_Application.Forms.Item(form);
                //AddMenuItemsToFormMonitor(oForm);
                sSQL = Querys.Default.CargueMonitor;
                if (sSQL != "")
                {
                    oItem = oForm.Items.Item("Grid");
                    oGrid = oItem.Specific;
                    oForm.DataSources.DataTables.Item(0).ExecuteQuery(sSQL);
                    oGrid.DataTable = oForm.DataSources.DataTables.Item("DT_0");
                    oGrid.Columns.Item("Descripcion Estado").Width = 300;
                    oGrid.Columns.Item("Detalle Peticion").Width = 100;
                    oGrid.Columns.Item("Respuesta Integracion").Width = 100;
                    oGrid.Columns.Item("Archivo PDF").Width = 100;
                    oGrid.Columns.Item("Archivo XML").Width = 100;
                    oGrid.Item.Enabled = false;
                }
                else if (sSQL == "")
                {
                    oItem = oForm.Items.Item("Grid_Redi");
                    oGrid = oItem.Specific;

                    if (oGrid.Rows.Count > 0)
                    {
                        oGrid.DataTable.Clear();
                    }
                }

                //oCol = (EditTextColumn)oGrid.Columns.Item("U_DocNum");
                //oCol.LinkedObjectType = "13";

                CommonSetting settingGrid = oGrid.CommonSetting;

                int redBackColor = Color.Tomato.R | (Color.Tomato.G << 8) | (Color.Tomato.B << 16);
                int greenBackColor = Color.PaleGreen.R | (Color.PaleGreen.G << 8) | (Color.PaleGreen.B << 16);
                int yellowBackColor = Color.Gold.R | (Color.Gold.G << 8) | (Color.Gold.B << 16);

                // Set background color in row
                //settingGrid.SetRowBackColor(1, redBackColor);
                //settingGrid.SetRowBackColor(2, yellowBackColor);
                //settingGrid.SetRowBackColor(3, greenBackColor);

                int row = 0;
                int rowcolor = 1;

                while (row < oGrid.Rows.Count)
                {
                    oCol = (EditTextColumn)oGrid.Columns.Item("Numero Interno");
                    oCol.LinkedObjectType = Convert.ToString(oGrid.DataTable.Columns.Item("Tipo Documento").Cells.Item(row).Value);

                    settingGrid.SetRowBackColor(rowcolor, -1);
                    string estado = Convert.ToString(oGrid.DataTable.Columns.Item("Codigo Estado").Cells.Item(row).Value);
                    if (Constants.red.Contains(estado))
                    {
                        settingGrid.SetRowBackColor(rowcolor, redBackColor);
                    }
                    else if (Constants.green.Contains(estado))
                    {
                        settingGrid.SetCellBackColor(rowcolor, 8, greenBackColor);
                    }
                    else if (Constants.yellow.Contains(estado))
                    {
                        settingGrid.SetRowBackColor(rowcolor, yellowBackColor);
                    }
                    row++;
                    rowcolor++;
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("loadGridLogtimer" + ex.Message);
                //SBO_Application.MessageBox(ex.Message);
            }
        }

        //Cargue de DataGrid antes de abrir el formulario Monitor Log
        public static void LoadGridLog(string form)
        {
            string sSQL = "";
            try
            {
                oForm = SBO_Application.Forms.Item(form);
                MenuItem.AddMenuItemsToFormMonitor(oForm);
                sSQL = Querys.Default.CargueMonitor;
                if (sSQL != "")
                {
                    oItem = oForm.Items.Item("Grid");
                    oGrid = oItem.Specific;
                    oForm.DataSources.DataTables.Item(0).ExecuteQuery(sSQL);
                    oGrid.DataTable = oForm.DataSources.DataTables.Item("DT_0");
                    oGrid.Columns.Item("Descripcion Estado").Width = 300;
                    oGrid.Columns.Item("Detalle Peticion").Width = 100;
                    oGrid.Columns.Item("Respuesta Integracion").Width = 100;
                    oGrid.Columns.Item("Archivo PDF").Width = 100;
                    oGrid.Columns.Item("Archivo XML").Width = 100;
                    oGrid.Item.Enabled = false;
                }
                else if (sSQL == "")
                {
                    oItem = oForm.Items.Item("Grid_Redi");
                    oGrid = oItem.Specific;

                    if (oGrid.Rows.Count > 0)
                    {
                        oGrid.DataTable.Clear();
                    }
                }

                //oCol = (EditTextColumn)oGrid.Columns.Item("U_DocNum");
                //oCol.LinkedObjectType = "13";

                CommonSetting settingGrid = oGrid.CommonSetting;

                int redBackColor = Color.Tomato.R | (Color.Tomato.G << 8) | (Color.Tomato.B << 16);
                int greenBackColor = Color.PaleGreen.R | (Color.PaleGreen.G << 8) | (Color.PaleGreen.B << 16);
                int yellowBackColor = Color.Gold.R | (Color.Gold.G << 8) | (Color.Gold.B << 16);

                // Set background color in row
                //settingGrid.SetRowBackColor(1, redBackColor);
                //settingGrid.SetRowBackColor(2, yellowBackColor);
                //settingGrid.SetRowBackColor(3, greenBackColor);

                int row = 0;
                int rowcolor = 1;

                while (row < oGrid.Rows.Count)
                {
                    oCol = (EditTextColumn)oGrid.Columns.Item("Numero Interno");
                    oCol.LinkedObjectType = Convert.ToString(oGrid.DataTable.Columns.Item("Tipo Documento").Cells.Item(row).Value);

                    settingGrid.SetRowBackColor(rowcolor, -1);
                    string estado = Convert.ToString(oGrid.DataTable.Columns.Item("Codigo Estado").Cells.Item(row).Value);
                    if (Procesos.proveedor == "C")
                    {
                        if (Constants.red.Contains(estado))
                        {
                            settingGrid.SetRowBackColor(rowcolor, redBackColor);
                        }
                        else if (Constants.green.Contains(estado))
                        {
                            settingGrid.SetCellBackColor(rowcolor, 8, greenBackColor);
                        }
                        else if (Constants.yellow.Contains(estado))
                        {
                            settingGrid.SetRowBackColor(rowcolor, yellowBackColor);
                        }
                    }
                    else if (Procesos.proveedor == "F")
                    {
                        if (Constants.red.Contains(estado))
                        {
                            settingGrid.SetRowBackColor(rowcolor, redBackColor);
                        }
                        else if (Constants.green.Contains(estado))
                        {
                            settingGrid.SetCellBackColor(rowcolor, 8, greenBackColor);
                        }
                        else if (Constants.yellow.Contains(estado))
                        {
                            settingGrid.SetRowBackColor(rowcolor, yellowBackColor);
                        }
                    }
                    row++;
                    rowcolor++;
                }
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("LoadGridLog: " + ex.Message);
            }
        }

        //validacion de proveedor para envio de informacion
        public static void SendFE(string docentry, int codeLog, string typeObject, Boolean reSend)
        {
            senalActiva = false;
            string filestr = "";
            string sNumSegui = "";
            string sRequest = "";
            if (Procesos.proveedor == "C")
            {
                filestr = Strtxt(docentry, typeObject);
                sNumSegui = MetodosCarvajal.UploadFileFE(EncodeToBase64(filestr), docentry, codeLog.ToString());
                sRequest = requestSend;
                System.Threading.Thread.Sleep(5000);
                MetodosCarvajal.DocStatusFE(codeLog, sNumSegui, sRequest, reSend, filestr);
            }

            else if(Procesos.proveedor == "F")
            {
                string dataJSON;
                string urlFebos;
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                filestr = Strtxt(docentry, typeObject);

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_INTERF_CFG");
                tbl.GetByKey(typeObject);
                urlFebos = tbl.UserFields.Fields.Item("U_URL").Value;

                Dictionary<string, Object> dicJSON = new Dictionary<string, Object>();
                dicJSON = new Dictionary<string, object>();
                
                dicJSON.Add("payload", EncodeToBase64(filestr));
                dataJSON = JsonConvert.SerializeObject(dicJSON);
                var resultDocument = ServiceFebos.Febos_documentos(urlFebos, "POST", dataJSON, token, false);

                var resultlist = resultDocument[true];
                var res = WebRequest.Equals(System.Net.HttpStatusCode.OK, resultlist);
                responseStatus = resultlist;
                var objAPIDoc = JsonConvert.DeserializeObject<dynamic>(resultlist.ToString());
                ResultAPI resAPIDoc = ((JObject)objAPIDoc).ToObject<ResultAPI>();

                UpdateLogFebos(codeLog, resAPIDoc, dataJSON, reSend, filestr);

                System.Threading.Thread.Sleep(1000);

                if (resAPIDoc.febosID != null)
                {
                   StatusFEBOS(codeLog, resAPIDoc.febosID, "", false, "");
                }
            }

            else if (Procesos.proveedor == "D")
            {
                string urlWS = "";
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_INTERF_CFG");
                tbl.GetByKey(typeObject);
                urlWS = tbl.UserFields.Fields.Item("U_URL").Value;

                Procesos.EscribirLogFileTXT("SendFE: Inicio");
                enviarDocumentoDispape.felRespuestaEnvio respuesta;
                respuesta = null;
                System.Data.DataTable Doc = new System.Data.DataTable();
                System.Data.DataTable impDoc = new System.Data.DataTable();

                Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                if (typeObject == "1")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.FacturaVenta, docentry));
                    Doc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: doc");
                }
                if (typeObject == "1")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.impFac, docentry));
                    impDoc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: impDoc");
                }

                if (typeObject == "2")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.NotaCredito, docentry));
                    Doc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: DocNC");
                }
                if (typeObject == "2")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.impNC, docentry));
                    impDoc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: impNC");
                }

                if (typeObject == "3")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.NotaDebito, docentry));
                    Doc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: docND");
                }
                if (typeObject == "3")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.impND, docentry));
                    impDoc = RecordSet_DataTable(oRecordset);
                    Procesos.EscribirLogFileTXT("SendFE: impND");
                }

                respuesta = WebServiceDispapelesController.EnviarFactura(Doc, impDoc, urlWS);
                sRequest = requestSend;
                UpdateLogDispapeles(codeLog, respuesta, sRequest, reSend);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                tbls = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                oRecordset = null;
                GC.Collect();
            }

            senalActiva = true;
        }

        //Creacion de TXT para envio FE
        public static string Strtxt(string transaction, string typeObj)
        {
            try
            {
                System.Data.DataTable sendFile = new System.Data.DataTable();
                Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                if (typeObj == "1")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.FacturaVenta, transaction));
                }
                else if (typeObj == "2")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.FacturaConti, transaction));
                    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                }
                else if (typeObj == "3")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.FacturaExpo, transaction));
                    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                }
                else if (typeObj == "4")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.NotaCredito, transaction));
                    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                }
                else if(typeObj == "5")
                {
                    oRecordset.DoQuery(string.Format(Querys.Default.NotaDebito, transaction));
                    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                }
                
                string myStr = "";
                int i = 0;
                sendFile = RecordSet_DataTable(oRecordset);

                using (MemoryStream ms = new MemoryStream())
                {
                    StreamWriter sw = new StreamWriter(ms);
                    foreach (DataRow row in sendFile.Rows)
                    {
                        object[] array = row.ItemArray;

                        for (i = 0; i < array.Length - 1; i++)
                        {
                            sw.Write(array[i].ToString());
                        }
                        sw.WriteLine(array[i].ToString());
                        //sw.WriteLine();
                    }
                    sw.Flush();
                    ms.Position = 0;
                    StreamReader sr = new StreamReader(ms);
                    myStr = sr.ReadToEnd();
                }
                string text = myStr;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordset);
                oRecordset = null;
                GC.Collect();
                return text;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("strtxt: " + ex.Message);
                return "";
            }
        }

        //conversion de resultado recordset to DataTable
        public static System.Data.DataTable RecordSet_DataTable(SAPbobsCOM.Recordset RS)
        {

            System.Data.DataTable dtTable = new System.Data.DataTable();
            System.Data.DataColumn NewCol = default(System.Data.DataColumn);
            DataRow NewRow = default(DataRow);
            int ColCount = 0;

            //try
            //{

            while (ColCount < RS.Fields.Count)
            {
                string dataType = "System.";
                switch (RS.Fields.Item(ColCount).Type)
                {
                    case SAPbobsCOM.BoFieldTypes.db_Alpha:
                        dataType = dataType + "String";
                        break;
                    case SAPbobsCOM.BoFieldTypes.db_Date:
                        dataType = dataType + "DateTime";
                        break;
                    case SAPbobsCOM.BoFieldTypes.db_Float:
                        dataType = dataType + "Double";
                        break;
                    case SAPbobsCOM.BoFieldTypes.db_Memo:
                        dataType = dataType + "String";
                        break;
                    case SAPbobsCOM.BoFieldTypes.db_Numeric:
                        dataType = dataType + "Decimal";
                        break;
                    default:
                        dataType = dataType + "String";
                        break;
                }

                NewCol = new System.Data.DataColumn(RS.Fields.Item(ColCount).Name, System.Type.GetType(dataType));
                dtTable.Columns.Add(NewCol);
                ColCount++;
            }
            int iCol = 0;
            while (!(RS.EoF))
            {
                NewRow = dtTable.NewRow();

                dtTable.Rows.Add(NewRow);

                iCol = 0;
                ColCount = 0;
                while (ColCount < RS.Fields.Count)
                {
                    //NewRow.Item(RS.Fields.Item(ColCount).Name) = RS.Fields.Item(ColCount).Value;
                    NewRow[iCol] = RS.Fields.Item(ColCount).Value;
                    iCol++;
                    ColCount++;
                }
                RS.MoveNext();
            }
            return dtTable;
        }

        //Codificacion de archivo a Base64
        public static string EncodeToBase64(string toEncode)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncode);
            string base64 = Convert.ToBase64String(bytes);
            string returnValue = base64;
            return returnValue;
        }

        //Peticion web service estado documento
        public static void StatusFEBOS(int codeLog, string transID, string request, Boolean ReSend, string strtext)
        {
            Procesos.responseStatus = "";
            try
            {
                if (transID != "")
                {
                    string urlstatus = "";
                    SAPbobsCOM.UserTables tbls = null;
                    SAPbobsCOM.UserTable tbl = null;

                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_INTERF_CFG");
                    tbl.GetByKey("6");

                    urlstatus = string.Format(tbl.UserFields.Fields.Item("U_URL").Value, transID);
                    var resultstatus = ServiceFebos.Febos_StatusDoc(urlstatus, "GET", transID, Procesos.token, false);
                    var resultliststatus = resultstatus[true];
                    Procesos.responseStatus = resultliststatus;
                    var objAPIDocstatu = JsonConvert.DeserializeObject<dynamic>(resultliststatus.ToString());
                    ResultAPI resAPIstatusDoc = ((JObject)objAPIDocstatu).ToObject<ResultAPI>();
                    Procesos.UpdateLogFebos(codeLog, resAPIstatusDoc, "", ReSend, strtext);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("StatusFebos: " + ex.Message);
            }
        }

        //Peticion web service estado documento Dispapeles
        //public static void StatusDispapeles(int codeLog, string transID, string request, Boolean ReSend, string strtext)
        //{
        //    Procesos.responseStatus = "";
        //    try
        //    {
        //        SAPbobsCOM.UserTables tbls = null;
        //        SAPbobsCOM.UserTable tbl = null;
        //        WSDispapeles.documentoElectronicoWsDto respuestaPDF;
        //        WSDispapeles.documentoElectronicoWsDto respuestaXML;
        //        int docEntry = 0;
        //        DateTime fechaFac;
        //        string prefijo = "";
        //        int tipoDoc = 0;
        //        string cufe = "";
        //        fechaFac = DateTime.Now;

        //        tbls = oCompany.UserTables;
        //        tbl = tbls.Item("FEDIAN_MONITORLOG");

        //        tbl.GetByKey(codeLog.ToString());

        //        string valuexml = tbl.UserFields.Fields.Item("U_Respuesta_Int").Value;

        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(valuexml);
        //        XmlNodeList nodeList = null;
        //        nodeList = doc.SelectNodes("envioFacturaRespuestaDTO");
        //        foreach (XmlNode node in nodeList)
        //        {
        //            docEntry = Convert.ToInt32(node["consecutivo"].InnerText);
        //            fechaFac = Convert.ToDateTime(node["fechaFactura"].InnerText);
        //            prefijo = node["prefijo"].InnerText;
        //            tipoDoc = Convert.ToInt32(node["tipoDocumento"].InnerText);
        //            if (node["cufe"] != null)
        //            {
        //                cufe = node["cufe"].InnerText;
        //            }
        //        }

        //        string urlWS = "";
        //        SAPbobsCOM.UserTables tblscnf = null;
        //        SAPbobsCOM.UserTable tblcnf = null;

        //        tblscnf = oCompany.UserTables;
        //        tblcnf = tblscnf.Item("FEDIAN_INTERF_CFG");
        //        tblcnf.GetByKey(tipoDoc.ToString());
        //        urlWS = tblcnf.UserFields.Fields.Item("U_URL").Value;

        //        //respuestaXML = WebServiceDispapelesController.ConsultaXML(docEntry, fechaFac, prefijo, tipoDoc, urlWS);
        //        //respuestaPDF = WebServiceDispapelesController.ConsultaPDF(docEntry, fechaFac, prefijo, tipoDoc, urlWS);

        //        if (respuestaPDF.streamFile != null)
        //        {
        //            string base64 = Convert.ToBase64String(respuestaPDF.streamFile);
        //            if (base64.Length > 256000)
        //            {
        //                tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = base64.Substring(0, 256000);
        //            }
        //            else
        //            {
        //                tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = base64;
        //            }
        //        }
        //        if (respuestaXML.streamFile != null)
        //        {
        //            string base64 = Convert.ToBase64String(respuestaXML.streamFile);
        //            tbl.UserFields.Fields.Item("U_Enlace_XML").Value = base64;
        //        }
        //        if(respuestaXML.error == null & respuestaPDF.error == null)
        //        {
        //            tbl.UserFields.Fields.Item("U_Status").Value = "1";
        //            tbl.UserFields.Fields.Item("U_Resultado").Value = "OK";
        //            tbl.UserFields.Fields.Item("U_ProcessID").Value = cufe;
        //        }
        //        else
        //        {
        //            tbl.UserFields.Fields.Item("U_Status").Value = "3";
        //            tbl.UserFields.Fields.Item("U_Resultado").Value = respuestaPDF.error;
        //        }

        //        lRetCode = tbl.Update();
        //        if (lRetCode != 0)
        //        {
        //            oCompany.GetLastError(out lRetCode, out sErrMsg);
        //            Procesos.EscribirLogFileTXT("updateLogDispapelesDocs: " + sErrMsg);
        //            //oCompany.GetLastError(out lRetCode, out sErrMsg);
        //            //SBO_Application.MessageBox(sErrMsg);
        //        }
        //        else
        //        {

        //        }
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(tblscnf);
        //        tblscnf = null;
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(tblcnf);
        //        tblcnf = null;
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
        //        tbl = null;
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
        //        tbls = null;
        //        GC.Collect();
        //    }
        //    catch (Exception ex)
        //    {
        //        Procesos.EscribirLogFileTXT("StatusDispapeles: " + ex.Message);
        //    }
        //}

        //Actualizacion Log despues de envio a Carvajal
        public static void UpdateLog(int codeline, string codseg, CarvajalWS.DocumentStatusResponse response, string srequest, Boolean reSend, string textstr)
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;
                string pdfResult;
                string xmlResult;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_MONITORLOG");

                tbl.GetByKey(codeline.ToString());


                if (srequest != "")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(srequest);
                    XmlNodeList nodeList = null;
                    nodeList = doc.GetElementsByTagName("fileData");
                    foreach (XmlNode node in nodeList)
                    {
                        node.InnerText = textstr;
                    }
                    tbl.UserFields.Fields.Item("U_Det_Peticion").Value = doc.InnerXml;
                }
                tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = responseStatus;

                if (response.processStatus == "FAIL")
                {
                    tbl.UserFields.Fields.Item("U_Resultado").Value = response.errorMessage;
                    tbl.UserFields.Fields.Item("U_Status").Value = "3";
                }
                else if (response.processStatus == "PROCESSING")
                {
                    tbl.UserFields.Fields.Item("U_Resultado").Value = response.processName;
                    tbl.UserFields.Fields.Item("U_Status").Value = "2";
                }
                else if (response.processStatus == "OK")
                {
                    string tipoDoc = tbl.UserFields.Fields.Item("U_DocType").Value;
                    string documentNumber = tbl.UserFields.Fields.Item("U_Folio").Value;
                    string documentType = "";
                    switch (tipoDoc)
                    {
                        case "1":
                            documentType = "FV";
                            break;
                        case "2":
                            documentType = "FC";
                            break;
                        case "3":
                            documentType = "FE";
                            break;
                        case "4":
                            documentType = "NC";
                            break;
                        case "5":
                            documentType = "ND";
                            break;
                        default:
                            break;
                    }
                    
                    xmlResult = MetodosCarvajal.DownloadDocFE(codeline, documentType, documentNumber, "SIGNED_XML");
                    if (xmlResult == "El recurso solicitado no ha sido encontrado.")
                    {
                        tbl.UserFields.Fields.Item("U_Status").Value = "2";
                        tbl.UserFields.Fields.Item("U_Resultado").Value = xmlResult;
                    }
                    else
                    {
                        tbl.UserFields.Fields.Item("U_Status").Value = "1";
                        tbl.UserFields.Fields.Item("U_Resultado").Value = response.processName;
                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = xmlResult;
                    }
                    System.Threading.Thread.Sleep(5000);

                    pdfResult = MetodosCarvajal.DownloadDocFE(codeline, documentType, documentNumber, "PDF");
                    if (pdfResult == "El recurso solicitado no ha sido encontrado.")
                    {
                        tbl.UserFields.Fields.Item("U_Status").Value = "2";
                        tbl.UserFields.Fields.Item("U_Resultado").Value = pdfResult;
                    }
                    else
                    {
                        tbl.UserFields.Fields.Item("U_Status").Value = "1";
                        tbl.UserFields.Fields.Item("U_Resultado").Value = response.processName;
                        tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = pdfResult;
                    }
                }
                else
                {

                }

                tbl.UserFields.Fields.Item("U_ProcessID").Value = codseg;
                if (reSend == false)
                {
                    tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = dateSend.ToString("yyyy/MM/dd");
                    tbl.UserFields.Fields.Item("U_Hora_Envio").Value = dateSend.ToString("HH:mm");
                }
                else if (reSend == true)
                {
                    tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = dateSend.ToString("yyyy/MM/dd");
                    tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = dateSend.ToString("HH:mm");
                    tbl.UserFields.Fields.Item("U_Usuario_ReEnvio").Value = user;
                }

                lRetCode = tbl.Update();
                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lRetCode, out sErrMsg);
                    Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                    //oCompany.GetLastError(out lRetCode, out sErrMsg);
                    //SBO_Application.MessageBox(sErrMsg);
                }
                else
                {
                    
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                tbls = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("updateLog: " + ex.Message);
            }
        }

        //Actualizacion Log despues de envio a Febos
        public static void UpdateLogFebos(int codeline, ResultAPI response, string srequest, Boolean reSend, string textstr)
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_MONITORLOG");

                tbl.GetByKey(codeline.ToString());

                tbl.UserFields.Fields.Item("U_Resultado").Value = response.mensaje;
                tbl.UserFields.Fields.Item("U_Status").Value = response.Codigo;
                tbl.UserFields.Fields.Item("U_ProcessID").Value = response.seguimientoId;

                if (reSend == false)
                {
                    tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = dateSend.ToString("yyyy/MM/dd");
                    tbl.UserFields.Fields.Item("U_Hora_Envio").Value = dateSend.ToString("HH:mm");
                }
                else if (reSend == true)
                {
                    tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = dateSend.ToString("yyyy/MM/dd");
                    tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = dateSend.ToString("HH:mm");
                    tbl.UserFields.Fields.Item("U_Usuario_ReEnvio").Value = user;
                }
                if (srequest != "")
                {
                    XmlDocument doc = JsonConvert.DeserializeXmlNode(srequest);
                    XmlNodeList nodeList = null;
                    nodeList = doc.GetElementsByTagName("payload");
                    foreach (XmlNode node in nodeList)
                    {
                        node.InnerText = textstr;
                    }
                    tbl.UserFields.Fields.Item("U_Det_Peticion").Value = doc.InnerXml;
                }

                if (responseStatus != "")
                {
                    XmlDocument docresponse = (XmlDocument)JsonConvert.DeserializeXmlNode(responseStatus, "root");
                    tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = docresponse.InnerXml;
                }
                else
                {
                    tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                }


                if (response.febosID != null)
                {
                    tbl.UserFields.Fields.Item("U_ID_Seguimiento").Value = response.febosID;
                }
                else
                {
                    tbl.UserFields.Fields.Item("U_ID_Seguimiento").Value = "";
                }

                if (response.imagenLink != null)
                {
                    tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = response.imagenLink;
                }
                else
                {
                    tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                }

                if (response.xmlLink != null)
                {
                    tbl.UserFields.Fields.Item("U_Enlace_XML").Value = response.xmlLink;
                }
                else
                {
                    tbl.UserFields.Fields.Item("U_Enlace_XML").Value = "";
                }

                lRetCode = tbl.Update();
                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lRetCode, out sErrMsg);
                    Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                    //SBO_Application.MessageBox(sErrMsg);
                }
                else
                {

                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("updateLog: " + ex.Message);
            }
        }

        //Actualizacion Log despues de envio a Dispapeles
        public static void UpdateLogDispapeles(int codeline, enviarDocumentoDispape.enviarDocumentoResponse response, string srequest, Boolean reSend)
        {
            try
            {
                Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Inicio");
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_MONITORLOG");

                tbl.GetByKey(codeline.ToString());

                if (srequest != "")
                {
                    tbl.UserFields.Fields.Item("U_Det_Peticion").Value = srequest;
                }

                var serxml = new System.Xml.Serialization.XmlSerializer(response.GetType());
                var ms = new MemoryStream();
                serxml.Serialize(ms, response);
                string xmlresponse = Encoding.UTF8.GetString(ms.ToArray());

                tbl.UserFields.Fields.Item("U_Respuesta_Int").Value = xmlresponse;
                if (response.mensaje == "OK")
                {
                    Procesos.EscribirLogFileTXT("UpdateLogDispapeles: OK");
                    int docEntry;
                    int tipoDoc;

                    consultarArchivosDispape.felRepuestaDescargaDocumentos respuestaXML;
                    consultarArchivosDispape.felRepuestaDescargaDocumentos respuestaPDF;

                    tbl.UserFields.Fields.Item("U_Status").Value = "1";

                    tbl.UserFields.Fields.Item("U_Resultado").Value = response.mensaje;
                    if (response.cufe != null)
                    {
                        tbl.UserFields.Fields.Item("U_ProcessID").Value = response.cufe;
                        Procesos.EscribirLogFileTXT("UpdateLogDispapeles: OK " + response.cufe);
                    }
                    docEntry = Convert.ToInt32(tbl.UserFields.Fields.Item("U_Folio").Value);
                    tipoDoc = Convert.ToInt32(tbl.UserFields.Fields.Item("U_DocType").Value);
                    System.Threading.Thread.Sleep(10000);

                    SAPbobsCOM.UserTables tblscnf = null;
                    SAPbobsCOM.UserTable tblcnf = null;
                    string urlWS = "";

                    tblscnf = oCompany.UserTables;
                    tblcnf = tblscnf.Item("FEDIAN_INTERF_CFG");
                    tblcnf.GetByKey(tipoDoc.ToString());
                    urlWS = tblcnf.UserFields.Fields.Item("U_URL").Value;

                    respuestaXML = WebServiceDispapelesController.consultaArchivos(docEntry, response.fechaFactura, response.prefijo, tipoDoc, urlWS);
                    respuestaPDF = WebServiceDispapelesController.consultaArchivos(docEntry, response.fechaFactura, response.prefijo, tipoDoc, urlWS);

                    if (respuestaPDF.streamFile != null)
                    {
                        Procesos.EscribirLogFileTXT("ConsultaXML: No Null");
                        string base64 = Convert.ToBase64String(respuestaPDF.streamFile);
                        //string serverDirectory = Properties.Settings.Default.RutaPDF;
                        if (base64.Length > 256000)
                        {
                            tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = base64.Substring(0, 256000);
                        }
                        else
                        {
                            tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = base64;
                        }

                        //string valuepdf = base64;
                        //try
                        //{
                        //    byte[] bytes = Convert.FromBase64String(valuepdf);
                        //    System.IO.FileStream stream = new FileStream(serverDirectory, FileMode.CreateNew);
                        //    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                        //    writer.Write(bytes, 0, bytes.Length);
                        //    writer.Close();
                        //    tbl.UserFields.Fields.Item("U_Archivo_PDF").Value = serverDirectory;
                        //}
                        //finally
                        //{

                        //}
                    }

                    if (respuestaXML.streamFile != null)
                    {
                        Procesos.EscribirLogFileTXT("ConsultaPDF: No Null");
                        string base64 = Convert.ToBase64String(respuestaXML.streamFile);
                        tbl.UserFields.Fields.Item("U_Enlace_XML").Value = base64;
                    }
                }

                else if (response.mensaje == "La factura fue ingresada previamente")
                {
                    Procesos.EscribirLogFileTXT("UpdateLogDispapeles: La factura fue ingresada previamente");

                    tbl.UserFields.Fields.Item("U_Status").Value = "2";

                    tbl.UserFields.Fields.Item("U_Resultado").Value = response.mensaje;
                    if (response.cufe != null)
                    {
                        tbl.UserFields.Fields.Item("U_ProcessID").Value = response.cufe;
                        Procesos.EscribirLogFileTXT("UpdateLogDispapeles: OK " + response.cufe);
                    }
                }

                else
                {
                    string[] ArrLine;
                    string delimStr = ":";
                    char[] delimiter = delimStr.ToCharArray();

                    int x = 2;
                    ArrLine = response.mensaje.Split(delimiter, x);

                    if (ArrLine.Length > 1)
                    {
                        if (ArrLine[0].ToString().Substring(0, 5).ToUpper() == "ERROR")
                        {
                            Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + ArrLine[0]);
                            tbl.UserFields.Fields.Item("U_Status").Value = "3";

                            Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + ArrLine[1]);
                            tbl.UserFields.Fields.Item("U_Resultado").Value = ArrLine[1];
                        }
                        else
                        {
                            Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + ArrLine[0]);
                            tbl.UserFields.Fields.Item("U_Status").Value = ArrLine[0];

                            Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + ArrLine[1]);
                            tbl.UserFields.Fields.Item("U_Resultado").Value = ArrLine[1];
                        }
                    }
                    else
                    {
                        Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + "3");
                        tbl.UserFields.Fields.Item("U_Status").Value = "3";
                        Procesos.EscribirLogFileTXT("UpdateLogDispapeles: Error" + response.mensaje);
                        tbl.UserFields.Fields.Item("U_Resultado").Value = response.mensaje;
                    }
                }

                if (reSend == false)
                {
                    Procesos.EscribirLogFileTXT("reSend: " + reSend);
                    if (response.fechaFactura != null)
                    {
                        Procesos.EscribirLogFileTXT("FechaDispapeles: " + response.fechaFactura.ToString("yyyy/MM/dd"));
                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = response.fechaFactura.ToString("yyyy/MM/dd");//dateSend.ToString("yyyy/MM/dd");
                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = dateSend.ToString("HH:mm"); //response.fechaFactura.ToString("HH:mm");
                    }
                    else
                    {
                        Procesos.EscribirLogFileTXT("FechaSistema: " + dateSend.ToString("yyyy/MM/dd"));
                        tbl.UserFields.Fields.Item("U_Fecha_Envio").Value = dateSend.ToString("yyyy/MM/dd");
                        tbl.UserFields.Fields.Item("U_Hora_Envio").Value = dateSend.ToString("HH:mm");
                    }
                }

                else if (reSend == true)
                {
                    Procesos.EscribirLogFileTXT("reSend: " + reSend);
                    tbl.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = dateSend.ToString("yyyy/MM/dd");
                    tbl.UserFields.Fields.Item("U_Hora_ReEnvio").Value = dateSend.ToString("HH:mm");
                    tbl.UserFields.Fields.Item("U_Usuario_ReEnvio").Value = user;
                }

                lRetCode = tbl.Update();
                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lRetCode, out sErrMsg);
                    Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                    //oCompany.GetLastError(out lRetCode, out sErrMsg);
                    //SBO_Application.MessageBox(sErrMsg);
                }
                else
                {
                    Procesos.EscribirLogFileTXT("Update OK: ");
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("updateLogDispapeles: " + ex.Message);
            }
        }

        //Verificar estado archivos enviados (Timer)
        public static void Verifystatus()
        {
            try
            {
                System.Data.DataTable ResultQuery = new System.Data.DataTable();
                Recordset oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sSql = string.Format(Querys.Default.ProcessStatus, "'" + String.Join("'" + ",'", Constants.yellow.ToArray()) + "'");
                oRS.DoQuery(sSql);

                if (oRS.RecordCount > 0)
                {
                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        int idLog;
                        string numSeg;
                        string strReq;

                        if(Procesos.proveedor == "C")
                        {
                            idLog = Convert.ToInt32(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                            numSeg = Convert.ToString(ResultQuery.Rows[i]["U_ProcessID"]); //Getting value IdProcess
                            strReq = Convert.ToString(ResultQuery.Rows[i]["U_Det_Peticion"]); //Getting value Request
                            MetodosCarvajal.DocStatusFE(idLog, numSeg, "", false, strReq);
                        }
                        else if (Procesos.proveedor == "F")
                        {
                            idLog = Convert.ToInt32(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                            numSeg = Convert.ToString(ResultQuery.Rows[i]["U_ID_Seguimiento"]); //Getting value IdProcess
                            strReq = Convert.ToString(ResultQuery.Rows[i]["U_Det_Peticion"]); //Getting value Request
                            StatusFEBOS(idLog, numSeg, "", false, strReq);
                        }
                        else if (Procesos.proveedor == "D")
                        {
                            idLog = Convert.ToInt32(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                            numSeg = "";// Convert.ToString(ResultQuery.Rows[i]["ProcessID"]); //Getting value IdProcess
                            strReq = "";//Convert.ToString(ResultQuery.Rows[i]["Det_Peticion"]); //Getting value Request
                            //StatusDispapeles(idLog, numSeg, "", false, strReq);
                        }
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                oRS = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("verifystatus: " + ex.Message);
            }
        }

        //ReenvioAuto documentos (Timer)
        public static void AutoReSend()
        {
            try
            {
                System.Data.DataTable ResultQuery = new System.Data.DataTable();
                Recordset oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sSql = string.Format(Querys.Default.ReSendAuto, "'" + String.Join("'" + ",'", Constants.red.ToArray()) + "'");
                oRS.DoQuery(sSql);

                if (oRS.RecordCount > 0)
                {
                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        int LogCode;
                        string docentry;
                        string tipDoc;
                        string fechaenvio;

                        LogCode = Convert.ToInt32(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                        docentry = Convert.ToString(ResultQuery.Rows[i]["U_DocNum"]); //Getting value docentry
                        tipDoc = Convert.ToString(ResultQuery.Rows[i]["U_DocType"]); //Getting value tipDoc
                        fechaenvio = Convert.ToString(ResultQuery.Rows[i]["U_Fecha_Envio"]); //Getting value fechaenvio

                        if (fechaenvio == "")
                        {
                            SendFE(docentry, LogCode, tipDoc, false);
                        }
                        else
                        {
                            SendFE(docentry, LogCode, tipDoc, true);
                        }
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                oRS = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("AutoReSend: " + ex.Message);
            }
        }

        //Add DTE al monitor (Timer)
        public static void AddDTEMonitor()
        {
            try
            {
                Recordset oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sSql = "";

                if (oCompany.DbServerType == BoDataServerTypes.dst_HANADB)
                {
                    sSql = "Select A1.\"Remark\", A0.\"DocNum\", A0.\"ObjType\", A0.\"DocEntry\", A2.\"USER_CODE\", A0.\"DocDate\", A0.\"DocTime\" " +
                            "From OINV A0 " +
                            "Inner Join NNM1 A1 On A0.\"Series\" = A1.\"Series\" And A0.\"ObjType\" = A1.\"ObjectCode\" " +
                            "Inner Join OUSR A2 On A0.\"UserSign\" = A2.\"USERID\" " +
                            "Where A0.\"Series\" in (Select B0.\"Series\" From NNM1 B0 Inner Join \"@FEDIAN_CODDOC\" B1 on B0.\"Remark\" = B1.\"Code\" And B0.\"ObjectCode\" = '13') And " +
                            "A0.\"DocEntry\" Not In (Select \"U_DocNum\" From \"@FEDIAN_MONITORLOG\" Where \"U_ObjType\" = '13') And A0.\"DocDate\" Between ADD_DAYS(CURRENT_DATE, -1) and To_Date(Current_Date) " +
                            "Union All " +
                            "Select A1.\"Remark\", A0.\"DocNum\", A0.\"ObjType\", A0.\"DocEntry\", A2.\"USER_CODE\", A0.\"DocDate\", A0.\"DocTime\" " +
                            "From ORIN A0 " +
                            "Inner Join NNM1 A1 On A0.\"Series\" = A1.\"Series\" And A0.\"ObjType\" = A1.\"ObjectCode\" " +
                            "Inner Join OUSR A2 On A0.\"UserSign\" = A2.\"USERID\" " +
                            "Where A0.\"Series\" in (Select B0.\"Series\" From NNM1 B0 Inner Join \"@FEDIAN_CODDOC\" B1 on B0.\"Remark\" = B1.\"Code\" And B0.\"ObjectCode\" = '14') And " +
                            "A0.\"DocEntry\" Not In (Select \"U_DocNum\" From \"@FEDIAN_MONITORLOG\" Where \"U_ObjType\" = '14') And A0.\"DocDate\" Between ADD_DAYS(CURRENT_DATE, -1) and To_Date(Current_Date) ";
                }

                else
                {
                    sSql = "Select A1.Remark, A0.DocNum, A0.ObjType, A0.DocEntry, A2.USER_CODE, A0.DocDate, A0.DocTime " +
                            "From OINV A0 " +
                            "Inner Join NNM1 A1 On A0.Series = A1.Series And A0.ObjType = A1.ObjectCode " +
                            "Inner Join OUSR A2 On A0.UserSign = A2.USERID " +
                            "Where A0.Series in (Select Series From NNM1 B0 Inner Join \"@FEDIAN_CODDOC\" B1 on B0.Remark = B1.Code And B0.ObjectCode = '13') And " +
                            "A0.DocEntry Not In(Select U_DocNum From \"@FEDIAN_MONITORLOG\" Where U_ObjType = '13') And CONVERT(char(10), A0.DocDate,126) Between CONVERT(char(10), GetDate()-1,126) and CONVERT(char(10), GetDate(),126) " +
                            "Union All " +
                            "Select A1.Remark, A0.DocNum, A0.ObjType, A0.DocEntry, A2.USER_CODE, A0.DocDate, A0.DocTime " +
                            "From ORIN A0 " +
                            "Inner Join NNM1 A1 On A0.Series = A1.Series And A0.ObjType = A1.ObjectCode " +
                            "Inner Join OUSR A2 On A0.UserSign = A2.USERID " +
                            "Where A0.Series in (Select Series From NNM1 B0 Inner Join \"@FEDIAN_CODDOC\" B1 on B0.Remark = B1.Code And B0.ObjectCode = '14') And " +
                            "A0.DocEntry Not In(Select U_DocNum From \"@FEDIAN_MONITORLOG\" Where U_ObjType = '14') And CONVERT(char(10), A0.DocDate,126) Between CONVERT(char(10), GetDate()-1,126) and CONVERT(char(10), GetDate(),126) ";
                }

                oRS.DoQuery(sSql);

                if (oRS.RecordCount > 0)
                {
                    UserTables tablas = null;
                    UserTable tabla = null;

                    System.Data.DataTable ResultQuery = new System.Data.DataTable();
                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        tablas = null;
                        tabla = null;

                        tablas = oCompany.UserTables;
                        tabla = tablas.Item("FEDIAN_MONITORLOG");
                        Recordset oRs = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                        oRs.DoQuery(string.Format(Querys.Default.MaxLog));
                        int newCode;
                        newCode = oRs.Fields.Item("NextCode").Value;

                        tabla.Code = Convert.ToString(newCode);
                        tabla.Name = Convert.ToString(newCode);
                        tabla.UserFields.Fields.Item("U_DocType").Value = Convert.ToString(ResultQuery.Rows[i]["Remark"]);
                        tabla.UserFields.Fields.Item("U_Folio").Value = Convert.ToString(ResultQuery.Rows[i]["DocNum"]);
                        tabla.UserFields.Fields.Item("U_ObjType").Value = Convert.ToString(ResultQuery.Rows[i]["ObjType"]);
                        tabla.UserFields.Fields.Item("U_DocNum").Value = Convert.ToString(ResultQuery.Rows[i]["DocEntry"]);
                        tabla.UserFields.Fields.Item("U_Usuario_Envio").Value = Convert.ToString(ResultQuery.Rows[i]["USER_CODE"]);
                        tabla.UserFields.Fields.Item("U_Fecha_Envio").Value = Convert.ToString(ResultQuery.Rows[i]["DocDate"]);
                        tabla.UserFields.Fields.Item("U_Hora_Envio").Value = Convert.ToString(ResultQuery.Rows[i]["DocTime"]);
                        tabla.UserFields.Fields.Item("U_Resultado").Value = "";
                        tabla.UserFields.Fields.Item("U_Status").Value = "";
                        tabla.UserFields.Fields.Item("U_ProcessID").Value = "";
                        tabla.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = "";
                        //tabla.UserFields.Fields.Item("U_Hora_ReEnvio").Value = "";
                        tabla.UserFields.Fields.Item("U_Det_Peticion").Value = "";
                        tabla.UserFields.Fields.Item("U_Respuesta_Int").Value = "";
                        tabla.UserFields.Fields.Item("U_Archivo_PDF").Value = "";
                        tabla.UserFields.Fields.Item("U_Enlace_XML").Value = "";

                        lRetCode = tabla.Add();

                        if (lRetCode != 0)
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                            Procesos.EscribirLogFileTXT("AddDTEMonitor: " + sErrMsg);
                        }
                        else
                        {
                            Procesos.EscribirLogFileTXT("AddDTEMonitor: Se agrego registro: " + newCode + " NumeroDoc: " + Convert.ToString(ResultQuery.Rows[i]["DocNum"]));
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oRs);
                        oRs = null;
                        GC.Collect();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                    tablas = null;
                    GC.Collect();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                    tabla = null;
                    GC.Collect();
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                oRS = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("AddDTEMonitor: " + ex.Message);
            }
        }

        //Funcion para escribir log txt
        public static void EscribirLogFileTXT(string cadenalog)
        {

            string ArchivoLog = FileLog + DateTime.Today.Year.ToString() + DateTime.Today.Month.ToString("00") + DateTime.Today.Day.ToString("00") + ".txt";
            string sPath = System.IO.Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\" + ArchivoLog;

            sPath = sPath.Substring(6, sPath.Length - 6);
            System.IO.StreamWriter file = new System.IO.StreamWriter(sPath, true);
            file.WriteLine(DateTime.Now + " : " + cadenalog);
            file.Close();
        }

        //Funcion para agregar nuevos mensajes al repositorio
        public static void RepoMensajes(string codigo, string mensaje)
        {
            //string sSQL = "";

            try
            {
                //int i = 0;
                //oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                //sSQL = string.Format(Querys.Default.Msginter, mensaje.Replace("'",""));
                //oRS.DoQuery(sSQL);
                //i = oRS.RecordCount;

                //if (i > 0)
                //{
                    
                //}
                //else
                //{
                //    SAPbobsCOM.UserTables tbls = null;
                //    SAPbobsCOM.UserTable tbl = null;

                //    tbls = oCompany.UserTables;
                //    tbl = tbls.Item("FEDIAN_INTERF_ERR");
                //    tbl.UserFields.Fields.Item("U_MsgExter").Value = mensaje;

                //    lRetCode = tbl.Add();

                //    //Vereficar si se añade registro en la tabla
                //    if (lRetCode != 0)
                //    {
                //        oCompany.GetLastError(out lRetCode, out sErrMsg);
                //        Procesos.EscribirLogFileTXT("RepositorioMensajes: " + sErrMsg);
                //    }
                //    else
                //    {
                       
                //    }
                //}
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                //oRS = null;
                //GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("RepoError: " + ex.Message);
            }
        }

        //Buscar en datatable valor segun nombre columna
        public static string Buscar_ValorCab(string Data, int Dimension, System.Data.DataTable TableResult)
        {
            string sRes = "";
            int j = 0;
            foreach (System.Data.DataColumn colDet in TableResult.Columns)
            {
                if (colDet.ColumnName.ToString() == Data)
                {
                    DataRow row = TableResult.Rows[Dimension];
                    sRes = row[j].ToString();
                    goto salto;
                }
                j++;
            }
            salto:
            return sRes;
        }
    }
}