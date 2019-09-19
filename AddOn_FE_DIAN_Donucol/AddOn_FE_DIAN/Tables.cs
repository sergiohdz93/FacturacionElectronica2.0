using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace AddOn_FE_DIAN
{
    class Tables
    {
        private SAPbouiCOM.Application SBO_Application;
        private SAPbobsCOM.Company oCompany;
        public static SAPbobsCOM.Recordset oRS;
        public static string sSql;
        public static int lRetCode;
        public static string sErrMsg;

        public Tables(SAPbobsCOM.Company oCmpn, SAPbouiCOM.Application SBO_App, bool version)
        {
            oCompany = oCmpn;
            SBO_Application = SBO_App;
            if (oCompany.Connected == true)
            {
                //CreateUDO();
                leerJsonTablasUsuario();

                leerJsonTipOpe();
                leerJsonRespon();
                leerJsonMedPago();
                leerJsonDescu();
                leerJsonConcepND();
                leerJsonConcepNC();
                leerJsonTributos();
                leerJsonIdentArti();

                leerJsonCamposUsuario();
                leerJsonDocDIAN();
                leerJsonCfgInter();
                leerJsonUM();
                leerJsonUserQueries();
                CreateUDO();
                addfieldLinkToUDO();
                leerJsonFormattedSearches();
                //CreateUDO();
            }
            else
            {
                //No hay conexión con SAP B1
            }
        }

        public void leerJsonTablasUsuario()
        {
            try
            {
                string outputJSON = File.ReadAllText("UserTables.json", System.Text.Encoding.Default);
                JArray parsedArray = JArray.Parse(outputJSON);
                int cantidad = parsedArray.Count;
                Console.WriteLine("Cantidad de tablas " + cantidad);
                dynamic dynJson = JsonConvert.DeserializeObject(outputJSON);
                foreach (var item in dynJson)
                {
                    addUserTables(Convert.ToString(item.TableDescription), Convert.ToString(item.TableName), Convert.ToString(item.TableType));
                }
                SBO_Application.StatusBar.SetText("Tablas Creadas", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {

            }
        }

        public void leerJsonTipOpe()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_TIPOPERA.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {

                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_TIPOPERA");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion tipo operacion añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo TipPre\n" + ex.Message);
            }
        }

        public void leerJsonRespon()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_RESPONSA.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_RESPONSA");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion responsabilidades añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Respon\n" + ex.Message);
            }
        }

        public void leerJsonMedPago()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_MEDPAGO.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_MEDPAGO");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion medios de pago añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Medpago\n" + ex.Message);
            }
        }

        public void leerJsonDescu()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_DESCU.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_DESCU");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion descuentos añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo descuentos\n" + ex.Message);
            }
        }

        public void leerJsonConcepND()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_CONCEP_ND.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CONCEP_ND");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion Conceptos ND añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Conceptos ND\n" + ex.Message);
            }
        }

        public void leerJsonConcepNC()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_CONCEP_NC.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CONCEP_NC");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion Conceptos ND añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Conceptos NC\n" + ex.Message);
            }
        }

        public void leerJsonTributos()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_TRIBU.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_TRIBU");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion Tributos añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Tributos\n" + ex.Message);
            }
        }

        public void leerJsonIdentArti()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_IDENT_ARTI.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_IDENT_ARTI");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion Identidad Articulos ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo Identidad Articulos\n" + ex.Message);
            }
        }

        public void leerJsonCamposUsuario()
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD;
            try
            {
                string outputJSON = File.ReadAllText("UserFields.json", System.Text.Encoding.Default), validValues = "";
                JArray parsedArray = JArray.Parse(outputJSON);
                var bodyField = "";
                string name = "";
                int i = 1;
                int cantidad = parsedArray.Count;

                foreach (JObject parsedObject in parsedArray.Children<JObject>())
                {
                    oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                    foreach (JProperty parsedProperty in parsedObject.Properties())
                    {
                        string description = "", table = "", subtype = "", type = "", size = "", editSize = "", mandatory = "", defaultValue = "", linkedSystemObject = "", LinkedTable = "", LinkedUDO = "";
                        string tag = parsedProperty.Name;
                        string value = Convert.ToString(parsedProperty.Value);
                        var bodyValues = "";
                        if (tag == "UserFieldsMD")
                        {
                            dynamic dynJson = JsonConvert.DeserializeObject(Convert.ToString(parsedProperty.Value));
                            foreach (var item in dynJson)
                            {
                                string tg = item.Name;
                                switch (tg)
                                {
                                    case "Description":
                                        description = item.Value;
                                        oUserFieldsMD.Description = Convert.ToString(description);
                                        break;
                                    case "Name":
                                        name = item.Value;
                                        oUserFieldsMD.Name = Convert.ToString(name);
                                        break;
                                    case "TableName":
                                        table = item.Value;
                                        oUserFieldsMD.TableName = Convert.ToString(table);
                                        break;
                                    case "SubType":
                                        subtype = item.Value;
                                        oUserFieldsMD = AddSubTypeField(oUserFieldsMD, subtype);
                                        break;
                                    case "Type":
                                        type = item.Value;
                                        oUserFieldsMD = AddTypeField(oUserFieldsMD, type);
                                        break;
                                    case "Size":
                                        size = item.Value;
                                        oUserFieldsMD.Size = Convert.ToInt32(size);
                                        break;
                                    case "EditSize":
                                        editSize = Convert.ToString(item.Value);
                                        oUserFieldsMD.Size = Convert.ToInt32(editSize);
                                        break;
                                    case "Mandatory":
                                        mandatory = Convert.ToString(item.Value);
                                        oUserFieldsMD = AddMandatoryField(oUserFieldsMD, type);
                                        break;
                                    case "LinkedSystemObject":
                                        linkedSystemObject = item.Value;
                                        break;
                                    case "LinkedTable":
                                        LinkedTable = Convert.ToString(item.Value);
                                        oUserFieldsMD.LinkedTable = LinkedTable;
                                        break;
                                    case "LinkedUDO":
                                        LinkedTable = Convert.ToString(item.Value);
                                        oUserFieldsMD.LinkedUDO = LinkedUDO;
                                        break;
                                    case "DefaultValue":
                                        defaultValue = Convert.ToString(item.Value);
                                        oUserFieldsMD.DefaultValue = defaultValue;
                                        break;
                                }
                            }
                        }
                        else if (tag == "ValidValuesMD")
                        {
                            dynamic dynJson = JsonConvert.DeserializeObject(value);
                            foreach (var item in dynJson)
                            {
                                //Dictionary<string, string> Values = new Dictionary<string, string>();

                                oUserFieldsMD.ValidValues.Value = Convert.ToString(item.Value);
                                oUserFieldsMD.ValidValues.Description = Convert.ToString(item.Description);
                                oUserFieldsMD.ValidValues.Add();
                            }
                        }

                    }

                    lRetCode = oUserFieldsMD.Add();

                    if (lRetCode != 0)
                    {
                        if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                        }
                        else
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                        }
                        Procesos.EscribirLogFileTXT(oUserFieldsMD.Name + ": " + sErrMsg);
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                    oUserFieldsMD = null;
                    GC.Collect();

                    i++;
                }
                SBO_Application.StatusBar.SetText("Campos creados", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Campos\n" + ex.Message);
            }
        }

        public void leerJsonDocDIAN()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_CODDOC.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CODDOC");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Documentos DIAN añadidos ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo CODDOC\n" + ex.Message);
            }
        }

        public void leerJsonCfgInter()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_INTERF_CFG.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_INTERF_CFG");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = Convert.ToString(item.Nombre);
                        tbl.UserFields.Fields.Item("U_URL").Value = string.Format(Convert.ToString(item.U_URL), tbl.Code);

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion de Interfaces añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo CfgInter\n" + ex.Message);
            }
        }

        public void leerJsonUM()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_UM.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_UM");

                    if (!tbl.GetByKey(Convert.ToString(item.Codigo)))
                    {
                        string s_unicode = Convert.ToString(item.Nombre);

                        tbl.Code = Convert.ToString(item.Codigo);
                        tbl.Name = s_unicode.Length <= 100 ? s_unicode : s_unicode.Substring(0, 99);
                        tbl.UserFields.Fields.Item("U_Descripcion").Value = s_unicode;

                        lRetCode = tbl.Add();

                        if (lRetCode != 0)
                        {
                            if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            else
                            {
                                oCompany.GetLastError(out lRetCode, out sErrMsg);
                            }
                            Procesos.EscribirLogFileTXT(tbl.TableName + " - " + tbl.Code + ": " + sErrMsg);
                        }
                    }

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Configuracion unidades de medida añadidas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Metodo UM\n" + ex.Message);
            }
        }

        public void leerJsonUserQueries()
        {
            try
            {
                string inputJSON = File.ReadAllText("UserQueries.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    AddQueryManager(Convert.ToString(item.QueryName), Convert.ToString(item.QueryCategory));
                }
                SBO_Application.StatusBar.SetText("Consultas creadas", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Agregar consultas de ususario\n" + ex.Message);
            }
        }

        public void leerJsonFormattedSearches()
        {
            try
            {
                string QueryId = "";
                string formID = "";
                SAPbobsCOM.Recordset oRecordSet = null;
                SAPbobsCOM.FormattedSearches oFormattedSearches = null;
                string inputJSON = File.ReadAllText("FormattedSearches.json", System.Text.Encoding.Default);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);

                foreach (var item in dynJson)
                {
                    oRecordSet = ((SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                    oFormattedSearches = ((SAPbobsCOM.FormattedSearches)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oFormattedSearches)));
                    if (Convert.ToString(item.FormStand) == "tNO")
                    {
                        string sSQL = "SELECT \"TblNum\" + 11000 as \"FormId\" FROM OUTB WHERE \"TableName\"  = '{0}' ";
                        oRecordSet.DoQuery(string.Format(sSQL, Convert.ToString(item.FormID)));
                        if (oRecordSet.RecordCount > 0)
                        {
                            formID = Convert.ToString(oRecordSet.Fields.Item("FormId").Value);
                        }
                        else formID = Convert.ToString(item.FormID);
                    }
                    else formID = Convert.ToString(item.FormID);

                    oRecordSet.DoQuery("SELECT \"IntrnalKey\" FROM OUQR WHERE \"QName\" = '" + Convert.ToString(item.QueryName) + "'");
                    if (oRecordSet.RecordCount > 0)
                    {
                        QueryId = Convert.ToString(oRecordSet.Fields.Item("IntrnalKey").Value);
                    }

                    oFormattedSearches.Action = SAPbobsCOM.BoFormattedSearchActionEnum.bofsaQuery;
                    switch (Convert.ToString(item.ByField))
                    {
                        case "tNO":
                            oFormattedSearches.ByField = SAPbobsCOM.BoYesNoEnum.tNO;
                            break;
                        case "tYES":
                            oFormattedSearches.ByField = SAPbobsCOM.BoYesNoEnum.tYES;
                            break;
                    }
                    oFormattedSearches.ColumnID = Convert.ToString(item.CollumID);
                    oFormattedSearches.ItemID = Convert.ToString(item.ItemID);
                    oFormattedSearches.FormID = formID;
                    oFormattedSearches.QueryID = Convert.ToInt32(QueryId);

                    switch (Convert.ToString(item.Refresh))
                    {
                        case "tNO":
                            oFormattedSearches.Refresh = SAPbobsCOM.BoYesNoEnum.tNO;
                            break;
                        case "tYES":
                            oFormattedSearches.Refresh = SAPbobsCOM.BoYesNoEnum.tYES;
                            oFormattedSearches.FieldID = Convert.ToString(item.FieldID);
                            break;
                    }

                    lRetCode = oFormattedSearches.Add();

                    if (lRetCode != 0)
                    {
                        if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                        }
                        else
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                        }
                        Procesos.EscribirLogFileTXT("formID: " + item.FormID + " QueryName: " + item.QueryName + ": " + sErrMsg);
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRecordSet);
                    oRecordSet = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oFormattedSearches);
                    oFormattedSearches = null;
                    GC.Collect();
                }
                SBO_Application.StatusBar.SetText("Busquedas formateadas asignadas ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("busquedasFormateadas\n" + ex.Message);
            }
        }

        public void addUserTables(string description, string table, string type)
        {

            SAPbobsCOM.UserTablesMD oUserTablesMD = default(SAPbobsCOM.UserTablesMD);
            oUserTablesMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            if (!oUserTablesMD.GetByKey(table))
            {
                oUserTablesMD.TableName = table;
                oUserTablesMD.TableDescription = description;
                switch (type)
                {
                    case "bott_NoObject":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_NoObject;
                        break;
                    case "bott_MasterData":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_MasterData;
                        break;
                    case "bott_MasterDataLines":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_MasterDataLines;
                        break;
                    case "bott_Document":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_Document;
                        break;
                    case "bott_DocumentLines":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_DocumentLines;
                        break;
                    case "bott_NoObjectAutoIncrement":
                        oUserTablesMD.TableType = SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement;
                        break;
                }

                lRetCode = oUserTablesMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    Procesos.EscribirLogFileTXT(oUserTablesMD.TableName + ": " + sErrMsg);
                }
                else
                {
                    //SBO_Application.MessageBox("Table: " & oUserTablesMD.TableName & " was added successfully")
                }
            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD);
            oUserTablesMD = null;
            GC.Collect();
        }

        public void AddQueryManager(string nombreQuery, string categoria)
        {
            try
            {
                int i = 0;
                int codigoSubCat = 0;
                SAPbobsCOM.QueryCategories sboQryCategory;
                sboQryCategory = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQueryCategories);

                SAPbobsCOM.UserQueries sboUserQuery;
                sboUserQuery = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);

                oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                sSql = string.Format(Constants.categorQuery, categoria);

                oRS.DoQuery(sSql);

                i = oRS.RecordCount;

                if (i < 1)
                {
                    sboQryCategory.Name = "FEDIAN";
                    lRetCode = sboQryCategory.Add();

                    if (lRetCode != 0)
                    {
                        if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                        { }
                        else
                        {
                            oCompany.GetLastError(out lRetCode, out sErrMsg);
                            sboQryCategory = null;
                            GC.Collect();
                        }
                    }
                    else
                    {
                        sSql = string.Format(Constants.categorQuery, categoria);
                        oRS.DoQuery(sSql);
                    }
                }
                codigoSubCat = oRS.Fields.Item("CategoryId").Value;
                string query = String.Format(Properties.Resources.ResourceManager.GetString(nombreQuery));

                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = nombreQuery;
                sboUserQuery.Query = query;

                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(sboQryCategory);
                sboQryCategory = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sboUserQuery);
                sboUserQuery = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                oRS = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("AddQueryManager: " + ex.Message);
            }
        }

        public void CreateUDO()
        {
            SAPbobsCOM.UserObjectsMD oUserObjectMD;
            SAPbobsCOM.UserObjectMD_FindColumns oUDOFind;
            SAPbobsCOM.UserObjectMD_FormColumns oUDOForm;
            SAPbobsCOM.UserObjectMD_EnhancedFormColumns oUDOEnhancedForm;
            oUserObjectMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD);
            oUDOFind = oUserObjectMD.FindColumns;
            oUDOForm = oUserObjectMD.FormColumns;
            oUDOEnhancedForm = oUserObjectMD.EnhancedFormColumns;

            //GC.Collect();
            //oUserObjectMD = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD) as SAPbobsCOM.UserObjectsMD;

            var retval = oUserObjectMD.GetByKey("FEDIAN_SN");
            if (!retval)
            {
                oUserObjectMD.Code = "FEDIAN_SN";
                oUserObjectMD.Name = "Responsabilidades y Tributos SN";
                oUserObjectMD.TableName = "FEDIAN_SN";
                oUserObjectMD.ObjectType = SAPbobsCOM.BoUDOObjType.boud_MasterData;

                oUserObjectMD.ManageSeries = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanDelete = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanClose = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanCancel = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanFind = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanYearTransfer = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanLog = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.OverwriteDllfile = SAPbobsCOM.BoYesNoEnum.tYES;
                oUserObjectMD.CanArchive = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.MenuItem = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.CanApprove = SAPbobsCOM.BoYesNoEnum.tNO;
                oUserObjectMD.EnableEnhancedForm = SAPbobsCOM.BoYesNoEnum.tYES;

                // ==================================
                oUDOFind.ColumnAlias = "Code";
                oUDOFind.ColumnDescription = "Codgio SN";
                oUDOFind.Add();
                oUDOFind.ColumnAlias = "Name";
                oUDOFind.ColumnDescription = "Nombre SN";
                oUDOFind.Add();
                //========================================


                //=========================================

                oUDOForm.FormColumnAlias = "Code";
                oUDOForm.FormColumnDescription = "Codgio SN";
                oUDOForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOForm.Add();

                oUDOForm.FormColumnAlias = "Name";
                oUDOForm.FormColumnDescription = "Nombre SN";
                oUDOForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOForm.Add();


                //=========================================


                oUserObjectMD.ChildTables.SetCurrentLine(0);
                oUserObjectMD.ChildTables.TableName = "FEDIAN_SN_RESPO";
                oUserObjectMD.ChildTables.Add();

                oUserObjectMD.ChildTables.SetCurrentLine(1);
                oUserObjectMD.ChildTables.TableName = "FEDIAN_SN_TRIB";
                //oUserObjectMD.ChildTables.Add();


                //=========================================

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(0);
                oUDOEnhancedForm.ColumnAlias = "Code";
                oUDOEnhancedForm.ColumnDescription = "Code";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 1;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(1);
                oUDOEnhancedForm.ColumnAlias = "LineId";
                oUDOEnhancedForm.ColumnDescription = "LineId";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 2;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(2);
                oUDOEnhancedForm.ColumnAlias = "Object";
                oUDOEnhancedForm.ColumnDescription = "Object";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 3;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(3);
                oUDOEnhancedForm.ColumnAlias = "LogInst";
                oUDOEnhancedForm.ColumnDescription = "LogInst";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 4;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(4);
                oUDOEnhancedForm.ColumnAlias = "U_Codigo";
                oUDOEnhancedForm.ColumnDescription = "Codigo Responsabilidad";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.ColumnNumber = 5;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 1;
                oUDOEnhancedForm.SetCurrentLine(5);
                oUDOEnhancedForm.ColumnAlias = "U_Desc";
                oUDOEnhancedForm.ColumnDescription = "Descripcion Responsabilidad";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.ColumnNumber = 6;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(6);
                oUDOEnhancedForm.ColumnAlias = "Code";
                oUDOEnhancedForm.ColumnDescription = "Code";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 1;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(7);
                oUDOEnhancedForm.ColumnAlias = "LineId";
                oUDOEnhancedForm.ColumnDescription = "LineId";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 2;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(8);
                oUDOEnhancedForm.ColumnAlias = "Object";
                oUDOEnhancedForm.ColumnDescription = "Object";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 3;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(9);
                oUDOEnhancedForm.ColumnAlias = "LogInst";
                oUDOEnhancedForm.ColumnDescription = "LogInst";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.ColumnNumber = 4;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tNO;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(10);
                oUDOEnhancedForm.ColumnAlias = "U_Codigo";
                oUDOEnhancedForm.ColumnDescription = "Codigo Tributo";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.ColumnNumber = 5;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.Add();

                oUDOEnhancedForm.ChildNumber = 2;
                oUDOEnhancedForm.SetCurrentLine(11);
                oUDOEnhancedForm.ColumnAlias = "U_Desc";
                oUDOEnhancedForm.ColumnDescription = "Descripcion Tributo";
                oUDOEnhancedForm.ColumnIsUsed = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.ColumnNumber = 6;
                oUDOEnhancedForm.Editable = SAPbobsCOM.BoYesNoEnum.tYES;
                oUDOEnhancedForm.Add();

                if (!retval)
                {
                    if ((oUserObjectMD.Add() != 0))
                    {
                        SBO_Application.MessageBox(oCompany.GetLastErrorDescription());
                    }
                    else
                    {
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserObjectMD);
                GC.Collect();
            }
        }

        public void addfieldLinkToUDO()
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD;

            oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
            oUserFieldsMD = null;
            GC.Collect();

            try
            {
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.TableName = "OCRD";
                oUserFieldsMD.Name = "FEDIAN_RYT";
                oUserFieldsMD.Description = "(FE) Responsabilidades y Tributos";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                oUserFieldsMD.Size = 50;
                oUserFieldsMD.EditSize = 50;
                oUserFieldsMD.LinkedUDO = "FEDIAN_SN";

                lRetCode = oUserFieldsMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    Procesos.EscribirLogFileTXT(oUserFieldsMD.Name + ": " + sErrMsg);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();



                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = "OADM";
                oUserFieldsMD.Name = "FEDIAN_RYT";
                oUserFieldsMD.Description = "(FE) Responsabilidades y Tributos";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                oUserFieldsMD.Size = 50;
                oUserFieldsMD.EditSize = 50;
                oUserFieldsMD.LinkedUDO = "FEDIAN_SN";

                lRetCode = oUserFieldsMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                    }
                    Procesos.EscribirLogFileTXT(oUserFieldsMD.Name + ": " + sErrMsg);
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();
                //SBO_Application.StatusBar.SetText("Campos creados", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox("Campos\n" + ex.Message);
            }
        }

        public SAPbobsCOM.UserFieldsMD AddTypeField(SAPbobsCOM.UserFieldsMD oUserField, string type)
        {
            switch (type)
            {
                case "db_Alpha":
                    oUserField.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                    break;
                case "db_Date":
                    oUserField.Type = SAPbobsCOM.BoFieldTypes.db_Date;
                    break;
                case "db_Float":
                    oUserField.Type = SAPbobsCOM.BoFieldTypes.db_Float;
                    break;
                case "db_Memo":
                    oUserField.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                    break;
                case "db_Numeric":
                    oUserField.Type = SAPbobsCOM.BoFieldTypes.db_Numeric;
                    break;
            }

            return oUserField;
        }

        public SAPbobsCOM.UserFieldsMD AddSubTypeField(SAPbobsCOM.UserFieldsMD oUserField, string subType)
        {
            switch (subType)
            {
                case "st_Address":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Address;
                    break;
                case "st_Image":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Image;
                    break;
                case "st_Link":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Link;
                    break;
                case "st_Measurement":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Measurement;
                    break;
                case "st_None":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_None;
                    break;
                case "st_Percentage":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Percentage;
                    break;
                case "st_Phone":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Phone;
                    break;
                case "st_Price":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Price;
                    break;
                case "st_Quantity":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Quantity;
                    break;
                case "st_Rate":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Rate;
                    break;
                case "st_Sum":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Sum;
                    break;
                case "st_Time":
                    oUserField.SubType = SAPbobsCOM.BoFldSubTypes.st_Time;
                    break;
            }

            return oUserField;
        }

        public SAPbobsCOM.UserFieldsMD AddMandatoryField(SAPbobsCOM.UserFieldsMD oUserField, string mandatory)
        {
            switch (mandatory)
            {
                case "tNO":
                    oUserField.Mandatory = SAPbobsCOM.BoYesNoEnum.tNO;
                    break;
                case "tYES":
                    oUserField.Mandatory = SAPbobsCOM.BoYesNoEnum.tYES;
                    break;

            }

            return oUserField;
        }
    }
}