﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Inicalizacion para al creacion de tablas
        public Tables(SAPbobsCOM.Company oCmpn, SAPbouiCOM.Application SBO_App, bool version)
        {
            oCompany = oCmpn;
            SBO_Application = SBO_App;
            if (oCompany.Connected == true)
            {
                leerJsonTablasUsuario();
                leerJsonCamposUsuario();
                leerJsonDocDIAN();
                leerJsonCfgInter();
                leerJsonTipOpe();
                leerJsonUM();
                leerJsonRespon();
                leerJsonMedPago();
                leerJsonDescu();
                leerJsonConcepND();
                leerJsonConcepNC();
                leerJsonTributos();
                leerJsonUserQueries();
                //AddTables(version);
                //AddQueryManager();
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
                string outputJSON = File.ReadAllText("UserTables.json", Encoding.UTF8);
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

        public void leerJsonCamposUsuario()
        {
            SAPbobsCOM.UserFieldsMD oUserFieldsMD;
            try
            {
                string outputJSON = File.ReadAllText("UserFields.json", Encoding.Default), validValues = "";
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
                                        editSize = item.Value;
                                        oUserFieldsMD.Size = Convert.ToInt32(editSize);
                                        break;
                                    case "Mandatory":
                                        mandatory = item.Value;
                                        oUserFieldsMD = AddMandatoryField(oUserFieldsMD, type);
                                        break;
                                    case "LinkedSystemObject":
                                        linkedSystemObject = item.Value;
                                        break;
                                    case "LinkedTable":
                                        LinkedTable = item.Value;
                                        oUserFieldsMD.LinkedTable = LinkedTable;
                                        break;
                                    case "LinkedUDO":
                                        LinkedTable = item.Value;
                                        oUserFieldsMD.LinkedUDO = LinkedUDO;
                                        break;
                                    case "DefaultValue":
                                        defaultValue = item.Value;
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

                string inputJSON = File.ReadAllText("FEDIAN_CODDOC.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CODDOC");

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

                string inputJSON = File.ReadAllText("FEDIAN_INTERF_CFG.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_INTERF_CFG");

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

        public void leerJsonTipOpe()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                string inputJSON = File.ReadAllText("FEDIAN_TIPOPERA.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {

                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_TIPOPERA");

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

        public void leerJsonUM()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;
                
                string inputJSON = File.ReadAllText("FEDIAN_UM.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_UM");

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

        public void leerJsonRespon()
        {
            try
            {
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;
                
                string inputJSON = File.ReadAllText("FEDIAN_RESPONSA.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_RESPONSA");

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

                string inputJSON = File.ReadAllText("FEDIAN_MEDPAGO.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_MEDPAGO");

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
                
                string inputJSON = File.ReadAllText("FEDIAN_DESCU.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_DESCU");

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

                string inputJSON = File.ReadAllText("FEDIAN_CONCEP_ND.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CONCEP_ND");

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

                string inputJSON = File.ReadAllText("FEDIAN_CONCEP_NC.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_CONCEP_NC");

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

                string inputJSON = File.ReadAllText("FEDIAN_TRIBU.json", Encoding.UTF8);
                dynamic dynJson = JsonConvert.DeserializeObject(inputJSON);
                foreach (var item in dynJson)
                {
                    tbls = oCompany.UserTables;
                    tbl = tbls.Item("FEDIAN_TRIBU");

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

        public void leerJsonUserQueries()
        {
            try
            {
                string inputJSON = File.ReadAllText("UserQueries.json", Encoding.UTF8);
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