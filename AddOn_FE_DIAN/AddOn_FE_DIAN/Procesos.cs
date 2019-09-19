﻿using SAPbouiCOM;
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
using System.Text;
using System.Reflection;

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
                //InicarTimersSAP();
                //Cargue inicial de parametrizacion
                user = SBO_Application.Company.UserName;
                CargueInicial();
                SBO_Application.FormDataEvent += new _IApplicationEvents_FormDataEventEventHandler(SBO_Application_FormDataEvent);
            }
            catch (Exception e)
            {
                SBO_Application.SetStatusBarMessage("Exception " + e.Message, BoMessageTime.bmt_Medium, false);
            }
        }

        //Cargue incial de informacion de proveedor
        public void CargueInicial()
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
                                    Documents oInvoice = null;
                                    CompanyService oCmpSrv = null;
                                    SeriesService oSeriesService = null;
                                    Series oSeries = null;
                                    SeriesParams oSeriesParams = null;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    //Get created invoice
                                    oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    // set the number of an existing series
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);

                                    SAPbobsCOM.UserTables tablas = null;
                                    SAPbobsCOM.UserTable tabla = null;
                                    tablas = oCompany.UserTables;
                                    tabla = tablas.Item("FEDIAN_NUMAUTORI");

                                    if(tabla.GetByKey(Convert.ToString(oSeries.Series)))
                                    {
                                        string docDian = "";
                                        string docType = "";
                                        docDian = tabla.UserFields.Fields.Item("U_DocDIAN").Value;
                                        docType = BusinessObjectInfo.Type;
                                        insertNewDoc(oInvoice, docDian, docType, oSeries.Prefix);
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCmpSrv);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesService);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeries);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesParams);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                                    GC.Collect();
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT(MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                            }
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
                                    Documents oInvoice = null;
                                    CompanyService oCmpSrv = null;
                                    SeriesService oSeriesService = null;
                                    Series oSeries = null;
                                    SeriesParams oSeriesParams = null;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    //Get created invoice
                                    oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    // set the number of an existing series
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);

                                    SAPbobsCOM.UserTables tablas = null;
                                    SAPbobsCOM.UserTable tabla = null;
                                    tablas = oCompany.UserTables;
                                    tabla = tablas.Item("FEDIAN_NUMAUTORI");

                                    if (tabla.GetByKey(Convert.ToString(oSeries.Series)))
                                    {
                                        string docDian = "";
                                        string docType = "";
                                        docDian = tabla.UserFields.Fields.Item("U_DocDIAN").Value;
                                        docType = BusinessObjectInfo.Type;
                                        insertNewDoc(oInvoice, docDian, docType, oSeries.Prefix);
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCmpSrv);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesService);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeries);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesParams);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                                    GC.Collect();
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
                //Formulario 65307 Factura de Exportacion
                if (oForm.Type == 65307 || oForm.Type == -65307)
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
                                    Documents oInvoice = null;
                                    CompanyService oCmpSrv = null;
                                    SeriesService oSeriesService = null;
                                    Series oSeries = null;
                                    SeriesParams oSeriesParams = null;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    //Get created invoice
                                    oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    // set the number of an existing series
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);

                                    SAPbobsCOM.UserTables tablas = null;
                                    SAPbobsCOM.UserTable tabla = null;
                                    tablas = oCompany.UserTables;
                                    tabla = tablas.Item("FEDIAN_NUMAUTORI");

                                    if (tabla.GetByKey(Convert.ToString(oSeries.Series)))
                                    {
                                        string docDian = "";
                                        string docType = "";
                                        docDian = tabla.UserFields.Fields.Item("U_DocDIAN").Value;
                                        docType = BusinessObjectInfo.Type;
                                        insertNewDoc(oInvoice, docDian, docType, oSeries.Prefix);
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCmpSrv);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesService);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeries);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesParams);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                                    GC.Collect();
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("65307-FacturaExportacion: " + ex.Message);
                            }
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
                                    Documents oInvoice = null;
                                    CompanyService oCmpSrv = null;
                                    SeriesService oSeriesService = null;
                                    Series oSeries = null;
                                    SeriesParams oSeriesParams = null;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    //Get created invoice
                                    oInvoice = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                                    oInvoice.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    // set the number of an existing series
                                    oSeriesParams.Series = oInvoice.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);

                                    SAPbobsCOM.UserTables tablas = null;
                                    SAPbobsCOM.UserTable tabla = null;
                                    tablas = oCompany.UserTables;
                                    tabla = tablas.Item("FEDIAN_NUMAUTORI");

                                    if (tabla.GetByKey(Convert.ToString(oSeries.Series)))
                                    {
                                        string docDian = "";
                                        string docType = "";
                                        docDian = tabla.UserFields.Fields.Item("U_DocDIAN").Value;
                                        docType = BusinessObjectInfo.Type;
                                        insertNewDoc(oInvoice, docDian, docType, oSeries.Prefix);
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCmpSrv);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesService);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeries);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesParams);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                                    GC.Collect();
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
                                    Documents oCreditNote = null;
                                    CompanyService oCmpSrv = null;
                                    SeriesService oSeriesService = null;
                                    Series oSeries = null;
                                    SeriesParams oSeriesParams = null;
                                    // get company service
                                    oCmpSrv = oCompany.GetCompanyService();
                                    // get series service
                                    oSeriesService = oCmpSrv.GetBusinessService(ServiceTypes.SeriesService);
                                    // get series params
                                    oSeriesParams = oSeriesService.GetDataInterface(SeriesServiceDataInterfaces.ssdiSeriesParams);
                                    //Get created invoice
                                    oCreditNote = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                                    oCreditNote.Browser.GetByKeys(BusinessObjectInfo.ObjectKey);
                                    // set the number of an existing series
                                    oSeriesParams.Series = oCreditNote.Series;
                                    // get the series
                                    oSeries = oSeriesService.GetSeries(oSeriesParams);

                                    SAPbobsCOM.UserTables tablas = null;
                                    SAPbobsCOM.UserTable tabla = null;
                                    tablas = oCompany.UserTables;
                                    tabla = tablas.Item("FEDIAN_NUMAUTORI");

                                    if (tabla.GetByKey(Convert.ToString(oSeries.Series)))
                                    {
                                        string docDian = "";
                                        string docType = "";
                                        docDian = tabla.UserFields.Fields.Item("U_DocDIAN").Value;
                                        docType = BusinessObjectInfo.Type;
                                        insertNewDoc(oCreditNote, docDian, docType, oSeries.Prefix);
                                    }
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCreditNote);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oCmpSrv);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesService);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeries);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSeriesParams);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                                    GC.Collect();
                                }
                            }
                            catch (Exception ex)
                            {
                                SBO_Application.MessageBox(ex.Message);
                                Procesos.EscribirLogFileTXT("179-NotaCredito: " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
            }
        }

        //Definicion timer
        public void InicarTimersSAP()
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
        public void OnTimedEventStatus(Object source, System.Timers.ElapsedEventArgs e)
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
        public void OnTimedEventReSend(Object source, System.Timers.ElapsedEventArgs e)
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
        public void OnTimedEventAddDTEMonitor(Object source, System.Timers.ElapsedEventArgs e)
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
        public void UpdateFormLogOpen()
        {
            try
            {
                UpdGridLogtimer("FORM_FE_0008");
            }
            catch (Exception e)
            {
                SBO_Application.SetStatusBarMessage("Exception " + e.Message, BoMessageTime.bmt_Medium, false);
            }
        }

        //Funcion que se ejecuta para actualizar el formuario Monitor Log
        public void UpdGridLogtimer(string form)
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
        public void LoadGridLog(string form)
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
                SBO_Application.MessageBox(ex.Message);
            }
        }

        // Inserta nuevo documento electronico (FEDIAN_MONITORLOG)
        public void insertNewDoc(Documents oDocument, string docDIAN, string docType, string prefijo)
        {
            try
            {
                UserTables tablas = null;
                UserTable tabla = null;

                int newCode = 0;
                //Añadir registro el tabla del Monitor Log
                tablas = null;
                tabla = null;

                tablas = oCompany.UserTables;
                tabla = tablas.Item("FEDIAN_MONITORLOG");

                Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                oRecordset.DoQuery(string.Format(Querys.Default.MaxLog));

                newCode = oRecordset.Fields.Item("NextCode").Value;

                tabla.Code = Convert.ToString(newCode);
                tabla.Name = Convert.ToString(newCode);
                tabla.UserFields.Fields.Item("U_DocType").Value = Convert.ToString(docDIAN);
                tabla.UserFields.Fields.Item("U_Folio").Value = Convert.ToString(oDocument.DocNum);
                tabla.UserFields.Fields.Item("U_Prefijo").Value = Convert.ToString(prefijo);
                tabla.UserFields.Fields.Item("U_ObjType").Value = Convert.ToString(docType);
                tabla.UserFields.Fields.Item("U_DocNum").Value = Convert.ToString(oDocument.DocEntry);
                tabla.UserFields.Fields.Item("U_Usuario_Envio").Value = Convert.ToString(user);
                tabla.UserFields.Fields.Item("U_Fecha_Envio").Value = Convert.ToString(oDocument.DocDate);
                tabla.UserFields.Fields.Item("U_Hora_Envio").Value = Convert.ToString(oDocument.DocTime);
                tabla.UserFields.Fields.Item("U_Resultado").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Status").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_ProcessID").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Fecha_ReEnvio").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Hora_ReEnvio").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Det_Peticion").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Respuesta_Int").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Archivo_PDF").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_Enlace_XML").Value = string.Empty;
                tabla.UserFields.Fields.Item("U_ID_Seguimiento").Value = string.Empty;

                lRetCode = tabla.Add();

                //Vereficar si se añade registro en la tabla
                if (lRetCode != 0)
                {
                    oCompany.GetLastError(out lRetCode, out sErrMsg);
                    Procesos.EscribirLogFileTXT("updateLog: " + sErrMsg);
                }
                else
                {
                    SendFE(Convert.ToString(oDocument.DocEntry), Convert.ToString(oDocument.DocNum), prefijo, Convert.ToString(newCode), docDIAN, false);
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT(MethodBase.GetCurrentMethod().Name + " \n Error: " + ex.Message);
            }
        }

        //validacion de proveedor para envio de informacion
        public static void SendFE(string docentry, string docNum, string prefijo, string codeLog, string typeDoc, Boolean reSend)//, string objType
        {
            try
            {
                senalActiva = false;
                string filestr = "", dataJSON = "", urlFebos = "";
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;
                Procesos.EscribirLogFileTXT("SendFE: DocEntry: " + docentry + " TipoDoc: " + typeDoc);
                filestr = Strtxt(docentry, typeDoc);//, objType

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_INTERF_CFG");
                tbl.GetByKey(typeDoc);
                urlFebos = tbl.UserFields.Fields.Item("U_URL").Value;

                urlFebos = string.Format(urlFebos, typeDoc, docNum, prefijo);

                Dictionary<string, Object> dicJSON = new Dictionary<string, Object>();
                dicJSON = new Dictionary<string, object>();
                dicJSON.Add("empresa", Procesos.nit);
                dicJSON.Add("foliar", "no");
                dicJSON.Add("folio", docNum);
                dicJSON.Add("prefijo", prefijo);
                dicJSON.Add("contenidoArchivoIntegracion", EncodeToBase64(filestr));
                //dicJSON.Add("devolverXml", "no");

                dataJSON = JsonConvert.SerializeObject(dicJSON);

                var resultDocument = ServiceFebos.Febos_documentos(urlFebos, "POST", dataJSON, Procesos.token, false);
                
                var resultlist = resultDocument[true];
                
                var res = WebRequest.Equals(System.Net.HttpStatusCode.OK, resultlist);
                
                responseStatus = resultlist;
                
                //var objAPIDoc = "";
                var objAPIDoc = JsonConvert.DeserializeObject<dynamic>(resultlist.ToString());
                
                ResultAPI resAPIDoc = null;
                resAPIDoc = ((JObject)objAPIDoc).ToObject<ResultAPI>();
                
                UpdateLogFebos(codeLog, resAPIDoc, dataJSON, reSend, filestr);

                System.Threading.Thread.Sleep(1000);

                if (resAPIDoc.febosID != null) StatusFEBOS(codeLog, resAPIDoc.febosID, "", false, "");

                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                tbls = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                GC.Collect();

                senalActiva = true;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("SendFE: " + ex.Message);
                senalActiva = true;
            }
        }

        //Creacion de TXT para envio FE
        public static string Strtxt(string transaction, string typeDoc)//, string objType
        {
            try
            {
                string sSQL = "";
                System.Data.DataTable DTDocFile = new System.Data.DataTable();
                Recordset oRecordset = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);

                //sSQL = System.Configuration.ConfigurationManager.AppSettings[typeDoc];
                //sSQL = string.Format(sSQL, transaction);
                switch (typeDoc)
                {
                    case "01":
                        sSQL = string.Format(Querys.Default.FacturaVenta, transaction);
                        break;
                    case "02":
                        sSQL = string.Format(Querys.Default.FacturaExpo, transaction);
                        break;
                    case "03":
                        sSQL = string.Format(Querys.Default.FacturaConti, transaction);
                        break;
                    case "91":
                        sSQL = string.Format(Querys.Default.NotaCredito, transaction);
                        break;
                    case "92":
                        sSQL = string.Format(Querys.Default.NotaDebito, transaction);
                        break;
                }
                oRecordset.DoQuery(sSQL);
                //if (typeDoc == "1")
                //{
                //    oRecordset.DoQuery(string.Format(Querys.Default.FacturaVenta, transaction));
                //}
                ////else if(typeDoc == "4" & objType == "13")
                ////{
                ////    oRecordset.DoQuery(string.Format(Querys.Default.FacturaVenta, transaction));
                ////}
                //else if (typeDoc == "2")
                //{
                //    oRecordset.DoQuery(string.Format(Querys.Default.FacturaExpo, transaction));
                //    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                //}
                //else if (typeDoc == "3")
                //{
                //    oRecordset.DoQuery(string.Format(Querys.Default.FacturaConti, transaction));
                //    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                //}
                //else if (typeDoc == "4")
                //{
                //    Procesos.EscribirLogFileTXT("strtxt: " + typeDoc);
                //    oRecordset.DoQuery(string.Format(Querys.Default.NotaCredito, transaction));
                //    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                //}
                //else if(typeDoc == "5")
                //{
                //    oRecordset.DoQuery(string.Format(Querys.Default.NotaDebito, transaction));
                //    //oRecordset.DoQuery(string.Format(Constants.CarvajalTXTCredit, transaction));
                //}

                string myStr = "";
                int i = 0;
                DTDocFile = RecordSet_DataTable(oRecordset);

                using (MemoryStream ms = new MemoryStream())
                {
                    StreamWriter sw = new StreamWriter(ms);
                    foreach (DataRow row in DTDocFile.Rows)
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
        public static void StatusFEBOS(string codeLog, string transID, string request, Boolean ReSend, string strtext)
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
                    //var objAPIDocstatu = "";
                    var objAPIDocstatu = JsonConvert.DeserializeObject<dynamic>(resultliststatus.ToString());
                    ResultAPI resAPIstatusDoc = null;
                    resAPIstatusDoc = ((JObject)objAPIDocstatu).ToObject<ResultAPI>();

                    Procesos.UpdateLogFebos(codeLog, resAPIstatusDoc, "", ReSend, strtext);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    GC.Collect();
                }

                else
                {
                    SAPbobsCOM.UserTables tbls = null;
                    SAPbobsCOM.UserTable tbl = null;

                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_MONITORLOG");

                    tbl.GetByKey(codeLog.ToString());

                    ResultAPI febosID;
                    febosID = FebosId(tbl.UserFields.Fields.Item("U_Folio").Value);

                    if (febosID.Codigo == "3")
                    {
                        Procesos.UpdateLogFebos(codeLog, febosID, "", ReSend, strtext);
                    }
                    else if (febosID.documentos.Count > 0)
                    {
                        string urlstatus = "";
                        SAPbobsCOM.UserTables tablas = null;
                        SAPbobsCOM.UserTable tabla = null;

                        tablas = oCompany.UserTables;
                        tabla = tablas.Item("FEDIAN_INTERF_CFG");
                        tabla.GetByKey("6");

                        urlstatus = string.Format(tabla.UserFields.Fields.Item("U_URL").Value, febosID.documentos[0].febosId);
                        var resultstatus = ServiceFebos.Febos_StatusDoc(urlstatus, "GET", febosID.documentos[0].febosId, Procesos.token, false);
                        var resultliststatus = resultstatus[true];
                        Procesos.responseStatus = resultliststatus;
                        //var objAPIDocstatu = "";
                        var objAPIDocstatu = JsonConvert.DeserializeObject<dynamic>(resultliststatus.ToString());
                        ResultAPI resAPIstatusDoc = null;
                        resAPIstatusDoc = ((JObject)objAPIDocstatu).ToObject<ResultAPI>();

                        Procesos.UpdateLogFebos(codeLog, resAPIstatusDoc, "", ReSend, strtext);

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                        tabla = null;
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                        tablas = null;
                    }
                    else
                    {
                        febosID.mensaje = "No Existe el documento";
                        febosID.Codigo = "147";

                        Procesos.UpdateLogFebos(codeLog, febosID, "", ReSend, strtext);
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    GC.Collect();
                }

            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("StatusFebos: " + ex.Message);
            }
        }

        //Peticion obtener FebosID por Folio
        public static ResultAPI FebosId(string folio)
        {
            Procesos.responseStatus = "";
            try
            {
                string urlstatus = "";
                SAPbobsCOM.UserTables tablas = null;
                SAPbobsCOM.UserTable tabla = null;

                tablas = oCompany.UserTables;
                tabla = tablas.Item("FEDIAN_INTERF_CFG");
                tabla.GetByKey("7");

                urlstatus = string.Format(tabla.UserFields.Fields.Item("U_URL").Value, Procesos.nit, folio);
                var resultstatus = ServiceFebos.Febos_folio(urlstatus, "GET", Procesos.token, false);
                var resultliststatus = resultstatus[true];
                Procesos.responseStatus = resultliststatus;
                //var objAPIDocstatu = "";
                var objAPIDocstatu = JsonConvert.DeserializeObject<dynamic>(resultliststatus.ToString());
                ResultAPI resAPIstatusFolio = null;
                resAPIstatusFolio = ((JObject)objAPIDocstatu).ToObject<ResultAPI>();


                System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                tablas = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                tabla = null;
                GC.Collect();

                return resAPIstatusFolio;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("ObtenerFebosID: " + ex.Message);
                return null;
            }
        }

        //Actualizacion Log despues de envio a Febos
        public static void UpdateLogFebos(string codeline, ResultAPI response, string srequest, Boolean reSend, string textstr)
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

                if (response.Codigo == "137")
                {
                    ResultAPI febosID;
                    febosID = FebosId(tbl.UserFields.Fields.Item("U_Folio").Value);
                    if (febosID.Codigo == "3")
                    {
                        //tbl.UserFields.Fields.Item("U_Resultado").Value = febosID.mensaje;
                        //tbl.UserFields.Fields.Item("U_Status").Value = febosID.Codigo;
                        //tbl.UserFields.Fields.Item("U_ProcessID").Value = febosID.seguimientoId;
                    }
                    else if (febosID.documentos.Count > 0)
                    {
                        tbl.UserFields.Fields.Item("U_ID_Seguimiento").Value = febosID.documentos[0].febosId;
                    }
                    else
                    {
                        tbl.UserFields.Fields.Item("U_Resultado").Value = "No Existe el documento";
                        tbl.UserFields.Fields.Item("U_Status").Value = "147";
                    }
                }

                else
                {
                    if (response.febosID != null)
                    {
                        tbl.UserFields.Fields.Item("U_ID_Seguimiento").Value = response.febosID;
                    }
                    else
                    {
                        tbl.UserFields.Fields.Item("U_ID_Seguimiento").Value = "";
                    }
                }
                
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
                    XmlDocument doc = JsonConvert.DeserializeXmlNode(srequest, "root"); //JsonConvert.DeserializeXmlNode(srequest);
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
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                tbls = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("updateLog: " + ex.Message);
            }
        }

        //Verificar estado archivos enviados (Timer)
        public void Verifystatus()
        {
            try
            {
                System.Data.DataTable ResultQuery = new System.Data.DataTable();
                Recordset oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sSql = string.Format(Querys.Default.ProcessStatus, String.Join(",", Constants.yellow.ToArray()));
                oRS.DoQuery(sSql);
                
                if (oRS.RecordCount > 0)
                {
                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        string idLog = "";
                        string numSeg = "";
                        string strReq = "";

                        idLog = Convert.ToString(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                        numSeg = Convert.ToString(ResultQuery.Rows[i]["U_ID_Seguimiento"]); //Getting value IdProcess
                        strReq = Convert.ToString(ResultQuery.Rows[i]["U_Det_Peticion"]); //Getting value Request
                        StatusFEBOS(idLog, numSeg, "", true, strReq);
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

        //Reenviar documentos (Timer)
        public void AutoReSend()
        {
            try
            {
                System.Data.DataTable ResultQuery = new System.Data.DataTable();
                Recordset oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                string sSql = string.Format(Querys.Default.ReSendAuto, String.Join(",", Constants.red.ToArray()));
                oRS.DoQuery(sSql);

                if (oRS.RecordCount > 0)
                {
                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        string LogCode;
                        string docentry;
                        string docnum;
                        string prefijo;
                        string tipDoc;
                        string fechaenvio;

                        LogCode = Convert.ToString(ResultQuery.Rows[i]["Code"]); //Getting value CodeLog
                        docentry = Convert.ToString(ResultQuery.Rows[i]["U_DocNum"]); //Getting value docentry
                        docnum = Convert.ToString(ResultQuery.Rows[i]["U_Folio"]);
                        prefijo = Convert.ToString(ResultQuery.Rows[i]["U_Prefijo"]);
                        tipDoc = Convert.ToString(ResultQuery.Rows[i]["U_DocType"]); //Getting value tipDoc
                        fechaenvio = Convert.ToString(ResultQuery.Rows[i]["U_Fecha_Envio"]); //Getting value fechaenvio
                        //objType = Convert.ToString(ResultQuery.Rows[i]["U_ObjType"]); //Getting value fechaenvio

                        if (fechaenvio == "")
                        {
                            Procesos.SendFE(docentry, docnum, prefijo, LogCode, tipDoc, false);//, objType
                        }
                        else
                        {
                            Procesos.SendFE(docentry, docnum, prefijo, LogCode, tipDoc, true);//, objType
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
        public void AddDTEMonitor()
        {
            try
            {
                Recordset oRS = null;
                oRS = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
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
                    System.Data.DataTable ResultQuery = null;
                    ResultQuery = new System.Data.DataTable();

                    ResultQuery = RecordSet_DataTable(oRS);

                    for (int i = 0; i < ResultQuery.Rows.Count; i++) //Looping through rows
                    {
                        UserTables tablas = null;
                        UserTable tabla = null;

                        tablas = oCompany.UserTables;
                        tabla = tablas.Item("FEDIAN_MONITORLOG");

                        Recordset oRs = null;
                        oRs = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
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
                            Procesos.EscribirLogFileTXT("AddDTEMonitor: Se agrego registro: " + newCode + " DocNum: " + Convert.ToString(ResultQuery.Rows[i]["DocNum"]) + " DocEntry: " + Convert.ToString(ResultQuery.Rows[i]["DocEntry"]));
                        }
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oRs);
                        oRs = null;
                        GC.Collect();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tablas);
                        tablas = null;
                        GC.Collect();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(tabla);
                        tabla = null;
                        GC.Collect();
                    }
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
    }
}