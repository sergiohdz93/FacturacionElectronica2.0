using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class PreCarga
    {
        private SAPbobsCOM.Company oCompany;
        public static int lRetCode;
        public static string sErrMsg;

        public PreCarga(SAPbobsCOM.Company oCmpn, SAPbouiCOM.Application SBO_App)
        {
            oCompany = oCmpn;
            if (oCompany.Connected == true)
            {
                //PreCarga Lista Docuemntos DIAN
                ListaInter();
                //PreCarga Configuracion de Interfaces
                cfgInter();
            }
            else
            {
                //No hay conexión con SAP B1
            }
        }

        private void ListaInter()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_CODDOC");

                tbl.Code = Constants.CodDIAN_01[0];
                tbl.Name = Constants.CodDIAN_01[1];

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = Constants.CodDIAN_02[0];
                tbl.Name = Constants.CodDIAN_02[1];

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = Constants.CodDIAN_03[0];
                tbl.Name = Constants.CodDIAN_03[1];

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = Constants.CodDIAN_04[0];
                tbl.Name = Constants.CodDIAN_04[1];

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = Constants.CodDIAN_05[0];
                tbl.Name = Constants.CodDIAN_05[1];

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("CargueInicial: " + ex.Message);
            }
        }

        private void cfgInter()
        {
            string urlFebos = "";
            urlFebos = "https://api.febos.co/pruebas/documentos?simular=no&debug=si&tipo={0}&entrada=txt&foliar=si&firmar=si&obtenerXml=si&obtenerPdf=si";
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_INTERF_CFG");

                tbl.Code = "1";
                tbl.Name = "Factura de Venta";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = string.Format(urlFebos, tbl.Code);

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = "2";
                tbl.Name = "Factura de Contingencia";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = string.Format(urlFebos, tbl.Code);

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = "3";
                tbl.Name = "Factura de Exportacion";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = string.Format(urlFebos, tbl.Code);

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = "4";
                tbl.Name = "Nota de Credito";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = string.Format(urlFebos, tbl.Code);

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = "5";
                tbl.Name = "Nota de Debito";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = string.Format(urlFebos, tbl.Code);

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }

                tbl.Code = "6";
                tbl.Name = "Lectura de Respuesta DIAN";
                tbl.UserFields.Fields.Item("U_WS_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_Job_Activo").Value = "Y";
                tbl.UserFields.Fields.Item("U_URL").Value = Constants.urlstatusFebos;

                lRetCode = tbl.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("PreCarga: " + ex.Message);
            }
        }

        public static bool version(SAPbobsCOM.Company oCmpn)
        {
            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
                string version = fvi.FileVersion;
                bool resultado;

                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = oCmpn.UserTables;
                tbl = tbls.Item("FEDIAN_VERSION");

                if (tbl.GetByKey("1") == true & tbl.Name == version)
                {
                    resultado = false;
                }
                else if (tbl.GetByKey("1") == true & tbl.Name != version)
                {
                    resultado = true;
                    tbl.Name = version;

                    lRetCode = tbl.Update();

                    if (lRetCode != 0)
                    {
                        if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                        { }
                        else
                        {
                            oCmpn.GetLastError(out lRetCode, out sErrMsg);
                            Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                        }
                    }
                }
                else
                {
                    resultado = true;
                    tbl.Code = "1";
                    tbl.Name = version;

                    lRetCode = tbl.Add();

                    if (lRetCode != 0)
                    {
                        if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                        { }
                        else
                        {
                            oCmpn.GetLastError(out lRetCode, out sErrMsg);
                            Procesos.EscribirLogFileTXT("PreCarga: " + lRetCode + " > " + sErrMsg);
                        }
                    }
                }

                return resultado;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("Version: " + ex.Message);
                return true;
            }
        }
    }
}
