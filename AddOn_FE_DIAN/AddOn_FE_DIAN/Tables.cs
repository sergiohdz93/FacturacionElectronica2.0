using System;
using System.Collections.Generic;
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

        private string[] utTable_01 = { "FEDIAN_PARAMG", "Parametrizacion General" };
        private string[] utTable_02 = { "FEDIAN_OBLIGACIONES", "Obligaciones Emisor" };
        private string[] utTable_03 = { "FEDIAN_INTERF_CFG", "Configuración Interfaces" };
        private string[] utTable_04 = { "FEDIAN_CODDOC", "Códigos de documento DIAN" };
        private string[] utTable_05 = { "FEDIAN_INTERF_ERR", "Códigos de Error Interfaces" };
        //private string[] utTable_06 = { "FEDIAN_MAIL_INTERF", "Correos Notificación Errores" };
        private string[] utTable_07 = { "FEDIAN_MONITORLOG", "Monitor FE DIAN" };
        private string[] utTable_08 = { "FEDIAN_VERSION", "Version FE DIAN" };

        //Inicalizacion para al creacion de tablas
        public Tables(SAPbobsCOM.Company oCmpn, SAPbouiCOM.Application SBO_App, bool version)
        {
            oCompany = oCmpn;
            SBO_Application = SBO_App;
            if (oCompany.Connected == true)
            {
                //SBO_Application.MessageBox(" AddOn DI Connected To: " + oCompany.CompanyName, 1, "Ok", "", "");
                // events handled by SBO_Application_ItemEvent
                //bool tmpB = false;
                AddTables(version);
                //SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
                AddQueryManager();
            }
            else
            {
                //No hay conexión con SAP B1
            }
        }

        //Creacion de tablas de usuario
        private void AddTables(bool versionIni)
        {
            bool Valid = true;

            try
            {
                AddUserTables(utTable_08[0].ToString(), utTable_08[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                if (Valid == true & versionIni == true)
                {
                    // Tablas Especificas del AddOn
                    AddUserTables(utTable_01[0].ToString(), utTable_01[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                    AddUserTables(utTable_02[0].ToString(), utTable_02[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                    AddUserTables(utTable_03[0].ToString(), utTable_03[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                    AddUserTables(utTable_04[0].ToString(), utTable_04[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                    AddUserTables(utTable_05[0].ToString(), utTable_05[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);
                    //AddUserTables(utTable_06[0].ToString(), utTable_06[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObjectAutoIncrement);
                    AddUserTables(utTable_07[0].ToString(), utTable_07[1].ToString(), SAPbobsCOM.BoUTBTableType.bott_NoObject);

                    AddFieldsUserTables();
                }
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
            }
        }

        //Clase que crea las tablas de usuario
        private void AddUserTables(string Name, string Description, SAPbobsCOM.BoUTBTableType Type)
        {
            SAPbobsCOM.UserTablesMD oUserTablesMD = default(SAPbobsCOM.UserTablesMD);
            oUserTablesMD = (SAPbobsCOM.UserTablesMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables);

            if (!oUserTablesMD.GetByKey(Name))
            {
                oUserTablesMD.TableType = Type;
                oUserTablesMD.TableName = Name;
                oUserTablesMD.TableDescription = Description;
                lRetCode = oUserTablesMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 | lRetCode == -2035 | lRetCode == -5002)
                    {
                    }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        SBO_Application.MessageBox(sErrMsg);
                    }
                }
                else
                {
                    //SBO_Application.MessageBox("Table: " & oUserTablesMD.TableName & " was added successfully")
                }

            }
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD);
            oUserTablesMD = null;
            GC.Collect();
            //Release the handle to the table
        }

        //Creacion de campos de usuario
        private bool AddFieldsUserTables()
        {
            bool res = true;

            SAPbobsCOM.UserFieldsMD oUserFieldsMD;
            string NameTable;

            try
            {
                NameTable = "@FEDIAN_PARAMG";
                #region campos FEDIAN_PARAMG
                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Proveedor";
                oUserFieldsMD.Description = "Proveedor";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                oUserFieldsMD.ValidValues.Value = "F";
                oUserFieldsMD.ValidValues.Description = "Febos";
                lRetCode = oUserFieldsMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "NIT_Emisor";
                oUserFieldsMD.Description = "NIT Emisor";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 17;
                lRetCode = oUserFieldsMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Email_Usuario";
                oUserFieldsMD.Description = "Usuario Portal";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 60;
                lRetCode = oUserFieldsMD.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Clave_Usuario";
                oUserFieldsMD.Description = "Clave Portal";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 100;

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Token";
                oUserFieldsMD.Description = "Token";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 100;

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos FEDIAN_PARAMG

                NameTable = "@FEDIAN_INTERF_CFG";
                #region campos FEDIAN_INTERF_CFG

                //oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                //oUserFieldsMD.TableName = NameTable;
                //oUserFieldsMD.Name = "Sistema";
                //oUserFieldsMD.Description = "Sistema Destino";
                //oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                //oUserFieldsMD.EditSize = 30;
                //lRetCode = oUserFieldsMD.Add();
                //if (lRetCode != 0)
                //{
                //    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                //    { }
                //    else
                //    {
                //        oCompany.GetLastError(out lRetCode, out sErrMsg);
                //        oUserFieldsMD = null;
                //        GC.Collect();
                //        return false;
                //    }
                //}
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                //oUserFieldsMD = null;
                //GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "WS_Activo";
                oUserFieldsMD.Description = "Servicio Activo";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                oUserFieldsMD.ValidValues.Value = "Y";
                oUserFieldsMD.ValidValues.Description = "Y";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = "N";
                oUserFieldsMD.ValidValues.Description = "N";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.DefaultValue = "Y";

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Job_Activo";
                oUserFieldsMD.Description = "Job Reenvio Activo";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                oUserFieldsMD.ValidValues.Value = "Y";
                oUserFieldsMD.ValidValues.Description = "Y";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = "N";
                oUserFieldsMD.ValidValues.Description = "N";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.DefaultValue = "Y";

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "URL";
                oUserFieldsMD.Description = "URL Proveedor";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 245;

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Proxy";
                oUserFieldsMD.Description = "Clase Proxy";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "LP_Name";
                oUserFieldsMD.Description = "Nombre Puerta Logica";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos FEDIAN_INTERF_CFG

                NameTable = "@FEDIAN_INTERF_ERR";
                #region campos FEDIAN_INTERF_ERR

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "MsgExter";
                oUserFieldsMD.Description = "Mensaje Externo";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                oUserFieldsMD.EditSize = 150;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos FEDIAN_INTERF_ERR

                NameTable = "@FEDIAN_MAIL_INTERF";
                #region campos FEDIAN_MAIL_INTERF

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Cod_Interf";
                oUserFieldsMD.Description = "Codigo Interfaz";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Email";
                oUserFieldsMD.Description = "Correo Electronico";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 100;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Send_CC";
                oUserFieldsMD.Description = "C.C.";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                oUserFieldsMD.ValidValues.Value = "Y";
                oUserFieldsMD.ValidValues.Description = "Y";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = "N";
                oUserFieldsMD.ValidValues.Description = "N";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.DefaultValue = "Y";

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Send_CCO";
                oUserFieldsMD.Description = "C.C.O.";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                oUserFieldsMD.ValidValues.Value = "Y";
                oUserFieldsMD.ValidValues.Description = "Y";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = "N";
                oUserFieldsMD.ValidValues.Description = "N";
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.DefaultValue = "Y";

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
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos FEDIAN_MAIL_INTERF

                NameTable = "@FEDIAN_MONITORLOG";
                #region campos FEDIAN_MONITORLOG

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "DocType";
                oUserFieldsMD.Remove();
                oUserFieldsMD.Description = "Tipo Documento";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 15;

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Folio";
                oUserFieldsMD.Description = "Numero Documento";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 10;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "ObjType";
                oUserFieldsMD.Description = "Tipo Objeto";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 15;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "DocNum";
                oUserFieldsMD.Description = "Numero Interno";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Resultado";
                oUserFieldsMD.Description = "Descripcion Estado";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                //oUserFieldsMD.EditSize = 100;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Status";
                oUserFieldsMD.Description = "Codigo Estado";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 4;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "ProcessID";
                oUserFieldsMD.Description = "ID Proceso";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Fecha_Envio";
                oUserFieldsMD.Description = "Fecha Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Date;
                //oUserFieldsMD.EditSize = 40;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();


                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Hora_Envio";
                oUserFieldsMD.Description = "Hora Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Date;
                oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Time;
                //oUserFieldsMD.EditSize = 40;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Usuario_Envio";
                oUserFieldsMD.Description = "Usaurio Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Fecha_ReEnvio";
                oUserFieldsMD.Description = "Fecha Re-Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Date;
                //oUserFieldsMD.EditSize = 40;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();


                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Hora_ReEnvio";
                oUserFieldsMD.Description = "Hora Re-Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Date;
                oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Time;
                //oUserFieldsMD.EditSize = 40;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Usuario_ReEnvio";
                oUserFieldsMD.Description = "Usaurio Re-Envio";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Det_Peticion";
                oUserFieldsMD.Description = "Detalle Peticion";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Respuesta_Int";
                oUserFieldsMD.Description = "Respuesta Interfaz";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                //oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Link;
                //oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Archivo_PDF";
                oUserFieldsMD.Description = "Archivo PDF";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                //oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Link;
                //oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "Enlace_XML";
                oUserFieldsMD.Description = "Enlace XML";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                //oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Link;
                //oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);
                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "ID_Seguimiento";
                oUserFieldsMD.Description = "ID Seguimiento";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Memo;
                //oUserFieldsMD.SubType = SAPbobsCOM.BoFldSubTypes.st_Link;
                //oUserFieldsMD.EditSize = 30;
                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos FEDIAN_MONITORLOG

                NameTable = "OINV";
                #region campos OINV

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "ConcepNC";
                oUserFieldsMD.Remove();
                oUserFieldsMD.Description = "Concepto Nota Credito";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 15;
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC1[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC1[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC2[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC2[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC3[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC3[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC4[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC4[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC5[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC5[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_NC6[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_NC6[1];
                oUserFieldsMD.ValidValues.Add();

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                oUserFieldsMD = (SAPbobsCOM.UserFieldsMD)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields);

                oUserFieldsMD.TableName = NameTable;
                oUserFieldsMD.Name = "ConcepND";
                oUserFieldsMD.Remove();
                oUserFieldsMD.Description = "Concepto Nota Debito";
                oUserFieldsMD.Type = SAPbobsCOM.BoFieldTypes.db_Alpha;
                oUserFieldsMD.EditSize = 15;
                oUserFieldsMD.ValidValues.Value = Constants.concepto_ND1[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_ND1[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_ND2[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_ND2[1];
                oUserFieldsMD.ValidValues.Add();
                oUserFieldsMD.ValidValues.Value = Constants.concepto_ND3[0];
                oUserFieldsMD.ValidValues.Description = Constants.concepto_ND3[1];
                oUserFieldsMD.ValidValues.Add();

                lRetCode = oUserFieldsMD.Add();
                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        oUserFieldsMD = null;
                        GC.Collect();
                        return false;
                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserFieldsMD);
                oUserFieldsMD = null;
                GC.Collect();

                #endregion campos OINV
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
            }
            return res;
        }

        //Creacion de consultas en QueryManager
        private void AddQueryManager()
        {
            try
            {
                int i = 0;
                int codigoSubCat = 0;
                SAPbobsCOM.QueryCategories sboQryCategory;
                sboQryCategory = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oQueryCategories);

                SAPbobsCOM.UserQueries sboUserQuery;
                sboUserQuery = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserQueries);

                oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRS);
                sSql = Constants.categorQuery;

                oRS.DoQuery(sSql);

                i = oRS.RecordCount;

                if (i < 1)
                {
                    sboQryCategory.Name = "FE_DIAN";
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
                }

                codigoSubCat = oRS.Fields.Item("CategoryId").Value;

                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = Constants.CodDIAN_01[0] + " - " + Constants.CodDIAN_01[1];
                sboUserQuery.Query = "Select * From OINV Where \"DocEntry\" = {0}";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }

                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = Constants.CodDIAN_02[0] + " - " + Constants.CodDIAN_02[1];
                sboUserQuery.Query = "Select * From OINV Where \"DocEntry\" = {0}";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }


                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = Constants.CodDIAN_03[0] + " - " + Constants.CodDIAN_03[1];
                sboUserQuery.Query = "Select * From OINV Where \"DocEntry\" = {0}";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }


                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = Constants.CodDIAN_04[0] + " - " + Constants.CodDIAN_04[1];
                sboUserQuery.Query = "Select * From ORIN Where \"DocEntry\" = {0}";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }


                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = Constants.CodDIAN_05[0] + " - " + Constants.CodDIAN_05[1];
                sboUserQuery.Query = "Select * From OINV Where \"DocSubType\" = '--' Where \"DocEntry\" = {0}";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }

                sboUserQuery.QueryCategory = codigoSubCat;
                sboUserQuery.QueryDescription = "ListaDocDIAN";
                sboUserQuery.Query = "Select * from \"@FEDIAN_CODDOC\" Order By \"Code\"";
                lRetCode = sboUserQuery.Add();

                if (lRetCode != 0)
                {
                    if (lRetCode == -1 || lRetCode == -2035 || lRetCode == -5002)
                    { }
                    else
                    {

                    }
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(sboUserQuery);
                sboUserQuery = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("AddQueryManager: " + ex.Message);
            }
        }
    }
}