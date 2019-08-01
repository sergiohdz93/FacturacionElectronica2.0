using System;
using System.Windows.Forms;
using System.Data;
using System.Globalization;
using System.Text;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AddOn_FE_DIAN
{
    class MenuItem
    {
        public static SAPbouiCOM.Application SBO_Application;
        public static SAPbouiCOM.Form oForm;
        public static SAPbouiCOM.Item oItem, oButton;
        public static SAPbouiCOM.EditText txt_01;
        public static SAPbouiCOM.Button o_Button;
        public static SAPbouiCOM.ComboBox cmb_01;
        public static SAPbobsCOM.Company oCompany;
        public static SAPbouiCOM.StaticText lbl_01;
        public static SAPbouiCOM.Grid oGrid;
        public static SAPbouiCOM.ComboBox combo1;
        public static SAPbouiCOM.EditText camposedit;
        public static SAPbouiCOM.MenuItem oMenu;
        public static SAPbobsCOM.Recordset oRS;
        public static SAPbouiCOM.EditTextColumn oCol;
        public static int lRetCode;
        public static string sErrMsg;
        public static string sSql;
        public static SAPbouiCOM.ProgressBar progressBar;

        //Inicializacion de eventos y creacion de menus
        public MenuItem(SAPbobsCOM.Company oCmpn, SAPbouiCOM.Application SBO_App)
        {
            oCompany = oCmpn;
            SBO_Application = SBO_App;
            AddMenuItems();
            SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBO_Application_MenuEvent);
            SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
            SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
        }

        //Creacion de obejtos en el formulario de parametrizacion
        public static void AddMenuItemsToFormParamGen(SAPbouiCOM.Form oMenuForm)
        {
            try
            {
                //---------------------------------------------
                //Creation Fields
                oForm.DataSources.DBDataSources.Add("@FEDIAN_PARAMG");
                oItem = oForm.Items.Add("Codigo", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 48;
                oItem.Top = 9;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.Enabled = false;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                //txt_01.DataBind.SetBound(true, "@FEDIAN_PARAMG", "Code");
                //----------------------------------------------
                oItem = oForm.Items.Add("cmb_prov", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                oItem.Left = 138;
                oItem.Top = 33;
                oItem.Height = 14;
                oItem.Width = 148;
                cmb_01 = (SAPbouiCOM.ComboBox)oItem.Specific;
                cmb_01.DataBind.SetBound(true, "@FEDIAN_PARAMG", "U_Proveedor");
                //----------------------------------------------
                oItem = oForm.Items.Add("txt_NIT", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 138;
                oItem.Top = 62;
                oItem.Height = 14;
                oItem.Width = 148;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "@FEDIAN_PARAMG", "U_NIT_Emisor");
                //---------------------------------------------
                oItem = oForm.Items.Add("txt_Email", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 138;
                oItem.Top = 94;
                oItem.Height = 14;
                oItem.Width = 148;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "@FEDIAN_PARAMG", "U_Email_Usuario");
                //---------------------------------------------
                oItem = oForm.Items.Add("txt_Clave", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 138;
                oItem.Top = 127;
                oItem.Height = 14;
                oItem.Width = 148;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "@FEDIAN_PARAMG", "U_Clave_Usuario");
                txt_01.IsPassword = true;
                //----------------------------------------------
                //Create Static
                oItem = oForm.Items.Add("StcProv", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 48;
                oItem.Top = 33;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Proveedor";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcNIT", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 48;
                oItem.Top = 62;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "NIT Emisor";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcEmail", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 48;
                oItem.Top = 94;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Usuario (e-mail)";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcClave", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 48;
                oItem.Top = 127;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Clave";
                //----------------------------------------------
                //Create Button 
                oButton = null;
                oButton = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 48;
                oButton.Top = 161;
                oButton.Height = 20;
                oButton.Width = 65;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                o_Button.Caption = "OK";
                // ----------------------------------------------
                oButton = null;
                oButton = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 138;
                oButton.Top = 161;
                oButton.Height = 20;
                oButton.Width = 65;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                o_Button.Caption = "Cancelar";
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("AddMenuItemsToFormParamGen: " + ex.Message);
            }
        }

        //Creacion de obejtos en el formulario del monitor log
        public static void AddMenuItemsToFormMonitor(SAPbouiCOM.Form oMenuForm)
        {
            try
            {
                //Create Static
                oItem = oForm.Items.Add("StcFecha", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Fecha Envio";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcDoc", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 46;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Tipo Documento";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcEstado", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 72;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Estado";
                //---------------------------------------------
                //Creation Fields
                oForm.DataSources.UserDataSources.Add("dt_fIni", SAPbouiCOM.BoDataType.dt_DATE, 0);
                oItem = oForm.Items.Add("FechaIni", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 116;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.Enabled = true;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "", "dt_fIni");
                //----------------------------------------------
                oForm.DataSources.UserDataSources.Add("dt_ffIN", SAPbouiCOM.BoDataType.dt_DATE, 0);
                oItem = oForm.Items.Add("FechaFin", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 232;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.Enabled = true;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "", "dt_ffIN");
                //----------------------------------------------
                oItem = null;
                oItem = oForm.Items.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                oItem.Left = 116;
                oItem.Top = 46;
                oItem.Height = 14;
                oItem.Width = 80;
                combo1 = oItem.Specific;
                SAPbobsCOM.Recordset oRecPro = null;
                oRecPro = ((SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                oRecPro.DoQuery(string.Format(Querys.Default.cmbTipodoc));
                for (int i = 0; i <= combo1.ValidValues.Count - 1; i++)
                {
                    combo1.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
                combo1.ValidValues.Add("", "");
                while (oRecPro.EoF == false)
                {
                    combo1.ValidValues.Add(oRecPro.Fields.Item(0).Value, oRecPro.Fields.Item(1).Value);
                    oRecPro.MoveNext();
                }
                //----------------------------------------------
                oItem = null;
                oItem = oForm.Items.Add("Estado", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                oItem.Left = 116;
                oItem.Top = 72;
                oItem.Height = 14;
                oItem.Width = 80;
                combo1 = oItem.Specific;
                oRecPro = null;
                oRecPro = ((SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                oRecPro.DoQuery(string.Format(Querys.Default.cmbEstado));
                for (int i = 0; i <= combo1.ValidValues.Count - 1; i++)
                {
                    combo1.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
                combo1.ValidValues.Add("", "");
                while (oRecPro.EoF == false)
                {
                    combo1.ValidValues.Add(oRecPro.Fields.Item(0).Value, oRecPro.Fields.Item(1).Value);
                    oRecPro.MoveNext();
                }
                //---------------------------------------------
                //Creation Grid
                oForm.DataSources.DataTables.Add("DT_0");
                oItem = oForm.Items.Add("Grid", SAPbouiCOM.BoFormItemTypes.it_GRID);
                oItem.Left = 30;
                oItem.Top = 107;
                oItem.Height = 200;
                oItem.Width = 555;
                oGrid = (SAPbouiCOM.Grid)oItem.Specific;
                oGrid.SelectionMode = SAPbouiCOM.BoMatrixSelect.ms_Auto;
                //----------------------------------------------
                //Create Button 
                //oButton = null;
                //oButton = oForm.Items.Add("1", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                //oButton.Left = 35;
                //oButton.Top = 325;
                //oButton.Height = 20;
                //oButton.Width = 65;
                //o_Button = (SAPbouiCOM.Button)oButton.Specific;
                //o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                //o_Button.Caption = "OK";
                // ----------------------------------------------
                oButton = null;
                oButton = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 35;
                oButton.Top = 325;
                oButton.Height = 20;
                oButton.Width = 65;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                o_Button.Caption = "Cerrar";
                //----------------------------------------------
                //Create Button Update Form
                oButton = null;
                oButton = oForm.Items.Add("UPD_form", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 232;
                oButton.Top = 72;
                oButton.Height = 14;
                oButton.Width = 80;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                //o_Button.Image = "C:\\Users\\Usuario01\\Desktop\\if_update_64935.jpg";
                o_Button.Caption = "Generar";
                //----------------------------------------------
                // Create Static
                oButton = null;
                oButton = oForm.Items.Add("ReSend", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 354;
                oButton.Top = 27;
                oButton.Height = 14;
                oButton.Width = 80;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                //o_Button.Image = "C:\\Users\\Usuario01\\Desktop\\if_cassette_arrow_11646.jpg";
                o_Button.Caption = "Re-Enviar";
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("AddMenuItemsToFormMonitor: " + ex.Message);
            }
        }

        //Creacion de obejtos en el formulario del monitor log
        public static void AddMenuItemsToFormParametros(SAPbouiCOM.Form oMenuForm)
        {
            try
            {
                //Create Static
                oItem = oForm.Items.Add("StcFecha", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Fecha Envio";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcDoc", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 46;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Tipo Documento";
                //----------------------------------------------
                oItem = oForm.Items.Add("StcEstado", SAPbouiCOM.BoFormItemTypes.it_STATIC);
                oItem.Left = 30;
                oItem.Top = 72;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.TextStyle = (int)SAPbouiCOM.BoTextStyle.ts_UNDERLINE;
                lbl_01 = (SAPbouiCOM.StaticText)oItem.Specific;
                lbl_01.Caption = "Estado";
                //---------------------------------------------
                //Creation Fields
                oForm.DataSources.UserDataSources.Add("Fecha", SAPbouiCOM.BoDataType.dt_DATE, 0);
                oItem = oForm.Items.Add("FechaIni", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 116;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.Enabled = true;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "", "Fecha");
                //----------------------------------------------
                oItem = oForm.Items.Add("FechaFin", SAPbouiCOM.BoFormItemTypes.it_EDIT);
                oItem.Left = 232;
                oItem.Top = 20;
                oItem.Height = 14;
                oItem.Width = 80;
                oItem.Enabled = true;
                txt_01 = (SAPbouiCOM.EditText)oItem.Specific;
                txt_01.DataBind.SetBound(true, "", "Fecha");
                //----------------------------------------------
                oItem = null;
                oItem = oForm.Items.Add("TipoDoc", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                oItem.Left = 116;
                oItem.Top = 46;
                oItem.Height = 14;
                oItem.Width = 80;
                combo1 = oItem.Specific;
                SAPbobsCOM.Recordset oRecPro = null;
                oRecPro = ((SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                oRecPro.DoQuery(string.Format(Querys.Default.cmbTipodoc));
                for (int i = 0; i <= combo1.ValidValues.Count - 1; i++)
                {
                    combo1.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
                while (oRecPro.EoF == false)
                {
                    combo1.ValidValues.Add(oRecPro.Fields.Item(0).Value, oRecPro.Fields.Item(1).Value);
                    oRecPro.MoveNext();
                }
                //----------------------------------------------
                oItem = null;
                oItem = oForm.Items.Add("Estado", SAPbouiCOM.BoFormItemTypes.it_COMBO_BOX);
                oItem.Left = 116;
                oItem.Top = 72;
                oItem.Height = 14;
                oItem.Width = 80;
                combo1 = oItem.Specific;
                oRecPro = null;
                oRecPro = ((SAPbobsCOM.Recordset)(oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)));
                oRecPro.DoQuery(string.Format(Querys.Default.cmbEstado));
                for (int i = 0; i <= combo1.ValidValues.Count - 1; i++)
                {
                    combo1.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
                while (oRecPro.EoF == false)
                {
                    combo1.ValidValues.Add(oRecPro.Fields.Item(0).Value, oRecPro.Fields.Item(1).Value);
                    oRecPro.MoveNext();
                }
                //----------------------------------------------
                //Create Button 
                oButton = null;
                oButton = oForm.Items.Add("btnOK", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 30;
                oButton.Top = 119;
                oButton.Height = 20;
                oButton.Width = 65;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                o_Button.Caption = "OK";
                // ----------------------------------------------
                oButton = null;
                oButton = oForm.Items.Add("2", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                oButton.Left = 131;
                oButton.Top = 119;
                oButton.Height = 20;
                oButton.Width = 65;
                o_Button = (SAPbouiCOM.Button)oButton.Specific;
                o_Button.Type = SAPbouiCOM.BoButtonTypes.bt_Caption;
                o_Button.Caption = "Cancelar";
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("AddMenuItemsToFormParametros: " + ex.Message);
            }
        }

        //Creacion de Menu y SubMenus
        public void AddMenuItems()
        {
            try
            {
                SAPbouiCOM.Menus oMenus = null;
                SAPbouiCOM.MenuItem oMenuItem = null;

                // Get the menus collection from the application
                oMenus = SBO_Application.Menus;

                SAPbouiCOM.MenuCreationParams oCreationPackage = null;
                oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
                oMenuItem = SBO_Application.Menus.Item("43520");

                string sPath = null;
                //Primer Menu
                sPath = Application.StartupPath;
                //sPath = sPath.Remove(sPath.Length - 9, 9);
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
                oCreationPackage.UniqueID = "FE_DIAN";
                oCreationPackage.String = "Facturacion Electronica";
                oCreationPackage.Enabled = true;
                oCreationPackage.Image = sPath + "\\UI.bmp";
                oCreationPackage.Position = -1;
                oMenus = oMenuItem.SubMenus;
                if (!oMenus.Exists("FE_DIAN"))
                {
                    //  If the manu already exists this code will fail
                    oMenus.AddEx(oCreationPackage);

                    // Get the menu collection of the newly added pop-up item 
                    oMenuItem = SBO_Application.Menus.Item("FE_DIAN");
                    oMenus = oMenuItem.SubMenus;

                    // Create s sub menu
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "FE_0001";
                    oCreationPackage.String = "Parametrizacion";
                    oCreationPackage.Image = "";
                    oMenus.AddEx(oCreationPackage);

                    //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    //oCreationPackage.UniqueID = "FE_0002";
                    //oCreationPackage.String = "Lista de Interfaces";
                    //oMenus.AddEx(oCreationPackage);

                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "FE_0004";
                    oCreationPackage.String = "Tipos Doc. DIAN";
                    oCreationPackage.Image = "";
                    oMenus.AddEx(oCreationPackage);

                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "FE_0003";
                    oCreationPackage.String = "Configuracion de Interfaces";
                    oCreationPackage.Image = "";
                    oMenus.AddEx(oCreationPackage);

                    //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    //oCreationPackage.UniqueID = "FE_0005";
                    //oCreationPackage.String = "Clase Doc DIAN/SAP";
                    //oMenus.AddEx(oCreationPackage);

                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "FE_0006";
                    oCreationPackage.String = "Mensajes Interfaces";
                    oCreationPackage.Image = "";
                    oMenus.AddEx(oCreationPackage);

                    //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    //oCreationPackage.UniqueID = "FE_0007";
                    //oCreationPackage.String = "Notificacion Errores";
                    //oMenus.AddEx(oCreationPackage);

                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "FE_0008";
                    oCreationPackage.String = "Monitor Facturacion Electronica";
                    oCreationPackage.Image = "";
                    oMenus.AddEx(oCreationPackage);
                }
            }
            catch (Exception ex)
            {
                SBO_Application.MessageBox(ex.Message);
                Procesos.EscribirLogFileTXT("AddMenuItems: " + ex.Message);
            }
        }

        //Captura de eventos del menu
        private void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //Creación de Formularios
            if ((pVal.MenuUID == "FE_0001") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0001");
                    oForm.Visible = true;
                }
                catch
                {
                    oForm = null;
                    SAPbouiCOM.FormCreationParams oCreationParams = null;
                    oCreationParams = ((SAPbouiCOM.FormCreationParams)(SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams)));
                    
                    oCreationParams.UniqueID = "FORM_FE_0001";
                    oCreationParams.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Fixed;

                    oForm = SBO_Application.Forms.AddEx(oCreationParams);
                    oForm.Title = "Parametrizacion General";
                    oForm.DefButton = "1";
                    oForm.AutoManaged = true;
                    oForm.Left = 300;
                    oForm.Top = 75;
                    oForm.Height = 241;
                    oForm.Width = 337;
                    oItem = oForm.Items.Item("Codigo");
                    oItem.Enabled = false;
                    oForm.Visible = true;
                }
            }

            if ((pVal.MenuUID == "FE_0002") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0002");
                    oForm.Visible = true;
                }
                catch
                {
                    oMenu = SBO_Application.Menus.Item("51200");
                    int i;
                    string MenuUID = "";
                    for (i = 0; (i <= (oMenu.SubMenus.Count - 1)); i++)
                    {
                        string tablaname = oMenu.SubMenus.Item(i).String;
                        if (tablaname.Contains("FEDIAN_INTERF_LIST"))
                        {
                            MenuUID = oMenu.SubMenus.Item(i).UID;
                            break;
                        }

                    }
                    SBO_Application.ActivateMenuItem(MenuUID);
                }
            }

            if ((pVal.MenuUID == "FE_0003") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0003");
                    oForm.Visible = true;
                }
                catch
                {
                    oMenu = SBO_Application.Menus.Item("51200");
                    int i;
                    string MenuUID = "";
                    for (i = 0; (i <= (oMenu.SubMenus.Count - 1)); i++)
                    {
                        string tablaname = oMenu.SubMenus.Item(i).String;
                        if (tablaname.Contains("FEDIAN_INTERF_CFG"))
                        {
                            MenuUID = oMenu.SubMenus.Item(i).UID;
                            break;
                        }

                    }
                    SBO_Application.ActivateMenuItem(MenuUID);
                }
            }

            if ((pVal.MenuUID == "FE_0004") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0004");
                    oForm.Visible = true;
                }
                catch
                {
                    oMenu = SBO_Application.Menus.Item("51200");
                    int i;
                    string MenuUID = "";
                    for (i = 0; (i <= (oMenu.SubMenus.Count - 1)); i++)
                    {
                        string tablaname = oMenu.SubMenus.Item(i).String;
                        if (tablaname.Contains("FEDIAN_CODDOC"))
                        {
                            MenuUID = oMenu.SubMenus.Item(i).UID;
                            break;
                        }

                    }
                    SBO_Application.ActivateMenuItem(MenuUID);
                }
            }

            if ((pVal.MenuUID == "FE_0006") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0006");
                    oForm.Visible = true;
                }
                catch
                {
                    oMenu = SBO_Application.Menus.Item("51200");
                    int i;
                    string MenuUID = "";
                    for (i = 0; (i <= (oMenu.SubMenus.Count - 1)); i++)
                    {
                        string tablaname = oMenu.SubMenus.Item(i).String;
                        if (tablaname.Contains("FEDIAN_INTERF_ERR"))
                        {
                            MenuUID = oMenu.SubMenus.Item(i).UID;
                            break;
                        }

                    }
                    SBO_Application.ActivateMenuItem(MenuUID);
                }
            }

            if ((pVal.MenuUID == "FE_0007") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0007");
                    oForm.Visible = true;
                }
                catch
                {
                    oMenu = SBO_Application.Menus.Item("51200");
                    int i;
                    string MenuUID = "";
                    for (i = 0; (i <= (oMenu.SubMenus.Count - 1)); i++)
                    {
                        string tablaname = oMenu.SubMenus.Item(i).String;
                        if (tablaname.Contains("FEDIAN_MAIL_INTERF"))
                        {
                            MenuUID = oMenu.SubMenus.Item(i).UID;
                            break;
                        }

                    }
                    SBO_Application.ActivateMenuItem(MenuUID);
                }
            }

            if ((pVal.MenuUID == "FE_0008") & (pVal.BeforeAction == false))
            {
                try
                {
                    oForm = SBO_Application.Forms.Item("FORM_FE_0008");
                    oForm.Visible = true;
                }
                catch
                {
                    oForm = null;
                    SAPbouiCOM.FormCreationParams oCreationParams = null;
                    oCreationParams = SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_FormCreationParams);

                    oCreationParams.UniqueID = "FORM_FE_0008";
                    oCreationParams.BorderStyle = SAPbouiCOM.BoFormBorderStyle.fbs_Sizable;

                    oForm = SBO_Application.Forms.AddEx(oCreationParams);
                    
                    oForm.Title = "Monitor Facturacion Electronica DIAN";
                    oForm.Left = 300;
                    oForm.Top = 75;
                    oForm.Height = 403;
                    oForm.Width = 615;
                    oForm.State = SAPbouiCOM.BoFormStateEnum.fs_Maximized;
                    oForm.AutoManaged = true;
                    oForm.Visible = true;
                    oButton = oForm.Items.Item("ReSend");
                    oButton.Left = 354;
                    oButton.Top = 72;
                }
            }
        }

        //Captura de eventos items o objetos de los formularios
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (((pVal.FormUID == "FORM_FE_0001") & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) & (pVal.Before_Action == true)))
            {
                //Se realzia cargue de info en el formulario si existe registro
                try
                {
                    oForm = SBO_Application.Forms.Item(FormUID);
                    MenuItem.AddMenuItemsToFormParamGen(oForm);
                    int i = 0;

                    oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    sSql = Querys.Default.PARAMG;
                    oRS.DoQuery(sSql);

                    i = oRS.RecordCount;

                    if (i > 0)
                    {
                        string prov = "";
                        string nit = "";
                        string email = "";
                        string clave = "";

                        camposedit = null;
                        oItem = oForm.Items.Item("Codigo");
                        camposedit = oForm.Items.Item("Codigo").Specific;
                        camposedit.Value = oRS.Fields.Item("Code").Value.ToString();

                        prov = oRS.Fields.Item("U_Proveedor").Value.ToString();
                        camposedit = null;
                        combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("cmb_prov").Specific;
                        combo1.Select(prov, SAPbouiCOM.BoSearchKey.psk_ByValue);

                        camposedit = null;
                        camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_NIT").Specific;
                        nit = oRS.Fields.Item("U_NIT_Emisor").Value.ToString();
                        camposedit.Value = nit;

                        camposedit = null;
                        camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_Email").Specific;
                        email = oRS.Fields.Item("U_Email_Usuario").Value.ToString();
                        camposedit.Value = email;

                        camposedit = null;
                        camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_Clave").Specific;
                        clave = oRS.Fields.Item("U_Clave_Usuario").Value.ToString();
                        camposedit.Value = clave;

                        oItem = oForm.Items.Item("1");
                        oItem.Specific.Caption = "OK";
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                    }
                    else
                    {
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE;
                        oItem = oForm.Items.Item("1");
                        oItem.Specific.Caption = "Crear";
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRS);
                    oRS = null;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("Cargar_FORM_FE_0001: " + ex.Message);
                }
            }

            if (pVal.FormUID == "FORM_FE_0001" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID == "1" && pVal.BeforeAction == true)
            {
                //Actualizacion o Creacion de registro - click boton
                try
                {
                    string codigo = "";
                    string prov = "";
                    string nit = "";
                    string email = "";
                    string clave = "";
                    string msg = "";

                    oForm = SBO_Application.Forms.Item(FormUID);

                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("Codigo").Specific;
                    codigo = camposedit.Value;

                    combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("cmb_prov").Specific;
                    if (combo1.Selected != null)
                    {
                        prov = combo1.Selected.Value;
                    }

                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_NIT").Specific;
                    nit = camposedit.Value;

                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_Email").Specific;
                    email = camposedit.Value;

                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("txt_Clave").Specific;
                    clave = camposedit.Value;

                    //Procesos.SHA256Encrypt(clave);

                    SAPbobsCOM.UserTables tbls = oCompany.UserTables;
                    SAPbobsCOM.UserTable tbl = tbls.Item("FEDIAN_PARAMG");

                    if (codigo == "")
                    {
                        tbl.Code = "1";
                        tbl.Name = "1";
                    }
                    else
                    {
                        //Obtener llave si existe en tabla
                        tbl.GetByKey(codigo);
                    }

                    tbl.UserFields.Fields.Item("U_Proveedor").Value = prov;
                    tbl.UserFields.Fields.Item("U_NIT_Emisor").Value = nit;
                    tbl.UserFields.Fields.Item("U_Email_Usuario").Value = email;
                    tbl.UserFields.Fields.Item("U_Clave_Usuario").Value = clave;
                    tbl.UserFields.Fields.Item("U_Token").Value = clave;

                    switch (oForm.Mode)
                    {
                        case SAPbouiCOM.BoFormMode.fm_ADD_MODE:
                            lRetCode = tbl.Add();
                            msg = "Se creo un registo, ";
                            oForm.Refresh();
                            break;
                        case SAPbouiCOM.BoFormMode.fm_UPDATE_MODE:
                            lRetCode = tbl.Update();
                            msg = "Se actualizo un registo, ";
                            oForm.Refresh();
                            break;
                        default:
                            break;
                    }

                    //Validacion de registros añadidos o actualizados
                    if (lRetCode != 0)
                    {
                        oCompany.GetLastError(out lRetCode, out sErrMsg);
                        SBO_Application.MessageBox(sErrMsg);
                    }
                    else
                    {
                        SBO_Application.SetStatusBarMessage(msg + "Operación finalizada con éxito", SAPbouiCOM.BoMessageTime.bmt_Short, false);
                        oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
                        //Funcion para recargar datos almacenados en cargue inicial
                        Procesos.CargueInicial();
                    }
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                    tbl = null;
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                    tbls = null;
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("Grabar_FORM_FE_0001: " + ex.Message);
                }
            }

            if (((pVal.FormUID == "FORM_FE_0008") & (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD) & (pVal.Before_Action == true)))
            {
                //Cargue cuando se abre formulario de Monitor Log
                try
                {
                    oForm = SBO_Application.Forms.Item(FormUID);
                    MenuItem.AddMenuItemsToFormMonitor(oForm);
                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("Load_FORM_FE_0008: " + ex.Message);
                }
            }

            if (pVal.FormUID == "FORM_FE_0008" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID == "UPD_form" && pVal.BeforeAction == true)
            {
                try
                {
                    string fechaini = "";
                    string fechafin = "";
                    string tipodoc = "";
                    string estado = "";

                    oForm = SBO_Application.Forms.Item(FormUID);
                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("FechaIni").Specific;
                    fechaini = camposedit.Value;

                    camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("FechaFin").Specific;
                    fechafin = camposedit.Value;

                    combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("TipoDoc").Specific;
                    if (combo1.Selected != null)
                    {
                        tipodoc = combo1.Selected.Value;
                    }

                    combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("Estado").Specific;
                    if (combo1.Selected != null)
                    {
                        estado = combo1.Selected.Value;
                    }

                    if (fechaini != "" & fechafin != "")
                    {
                        LoadGridLog("FORM_FE_0008", string.Format(Querys.Default.CargueMonitor, fechaini, fechafin, tipodoc, estado));
                    }
                    else
                    {
                        SBO_Application.MessageBox("Debe ingresar parametros de fecha");
                    }
                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("UPD_form_FORM_FE_0008: " + ex.Message);
                }
            }

            if (pVal.FormUID == "FORM_FE_0008" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_DOUBLE_CLICK && pVal.ItemUID == "Grid" && pVal.BeforeAction == true)
            {
                string tempDirectory = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".xml";
                try
                {
                    if (pVal.ColUID == "Detalle Peticion")
                    {
                        SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                        int index = grd.GetDataTableRowIndex(pVal.Row);
                        SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                        string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                        // Create the XmlDocument.
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(valuexml);
                        doc.Save(tempDirectory);
                        System.Diagnostics.Process.Start("iexplore.exe", tempDirectory);
                    }
                    else if(pVal.ColUID == "Respuesta Integracion")
                    {
                        SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                        int index = grd.GetDataTableRowIndex(pVal.Row);
                        SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                        string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                        // Create the XmlDocument.
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(valuexml);
                        doc.Save(tempDirectory);
                        System.Diagnostics.Process.Start("iexplore.exe", tempDirectory);
                    }
                    else if (pVal.ColUID == "Archivo XML")
                    {
                        if (Procesos.proveedor == "C")
                        {
                            SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            int index = grd.GetDataTableRowIndex(pVal.Row);
                            SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                            // Create the XmlDocument.
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(DecodeTo64(valuexml));
                            doc.Save(tempDirectory);
                            if (valuexml != "")
                            {
                                System.Diagnostics.Process.Start("iexplore.exe", tempDirectory);
                            }
                        }
                        else if (Procesos.proveedor == "F")
                        {
                            SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            int index = grd.GetDataTableRowIndex(pVal.Row);
                            SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                            // Create the XmlDocument.
                            //XmlDocument doc = new XmlDocument();
                            //doc.LoadXml(valuexml);
                            //doc.Save(tempDirectory);
                            if (valuexml != "")
                            {
                                System.Diagnostics.Process.Start("iexplore.exe", valuexml);
                            }
                        }
                        else if(Procesos.proveedor == "D")
                        {
                            SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            int index = grd.GetDataTableRowIndex(pVal.Row);
                            SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                            // Create the XmlDocument.
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(DecodeTo64(valuexml));
                            doc.Save(tempDirectory);
                            if (valuexml != "")
                            {
                                System.Diagnostics.Process.Start(tempDirectory);
                                //System.Diagnostics.Process.Start("iexplore.exe", tempDirectory);
                            }
                        }
                    }
                    else if (pVal.ColUID == "Archivo PDF")
                    {
                        if (Procesos.proveedor == "C")
                        {
                            tempDirectory = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
                            SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            int index = grd.GetDataTableRowIndex(pVal.Row);
                            SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            string valuepdf = myDataTable.GetValue(pVal.ColUID, index);

                            byte[] bytes = Convert.FromBase64String(valuepdf);
                            System.IO.FileStream stream = new FileStream(tempDirectory, FileMode.CreateNew);
                            System.IO.BinaryWriter writer = new BinaryWriter(stream);
                            writer.Write(bytes, 0, bytes.Length);
                            writer.Close();
                            if (valuepdf != "")
                            {
                                System.Diagnostics.Process.Start("iexplore.exe", tempDirectory);
                            }
                        }
                        else if (Procesos.proveedor == "F")
                        {
                            SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            int index = grd.GetDataTableRowIndex(pVal.Row);
                            SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            string valuexml = myDataTable.GetValue(pVal.ColUID, index);
                            // Create the XmlDocument.
                            //XmlDocument doc = new XmlDocument();
                            //doc.LoadXml(valuexml);
                            //doc.Save(tempDirectory);
                            if (valuexml != "")
                            {
                                System.Diagnostics.Process.Start("iexplore.exe", valuexml);
                            }
                        }
                        else if(Procesos.proveedor == "D")
                        {
                            //WSDispapeles.documentoElectronicoWsDto respuestaPDF;
                            //SAPbouiCOM.Grid grd = SBO_Application.Forms.ActiveForm.Items.Item("Grid").Specific;
                            //int index = grd.GetDataTableRowIndex(pVal.Row);
                            //SAPbouiCOM.DataTable myDataTable = oGrid.DataTable;
                            //string valuexml = myDataTable.GetValue("Respuesta Integracion", index);
                            //int docEntry = 0;
                            //DateTime fechaFac;
                            //string prefijo = "";
                            //int tipoDoc = 0;
                            //string cufe = "";
                            //fechaFac = DateTime.Now;

                            //XmlDocument doc = new XmlDocument();
                            //doc.LoadXml(valuexml);
                            //XmlNodeList nodeList = null;
                            //nodeList = doc.SelectNodes("envioFacturaRespuestaDTO");
                            //foreach (XmlNode node in nodeList)
                            //{
                            //    docEntry = Convert.ToInt32(node["consecutivo"].InnerText);
                            //    fechaFac = Convert.ToDateTime(node["fechaFactura"].InnerText);
                            //    prefijo = node["prefijo"].InnerText;
                            //    tipoDoc = Convert.ToInt32(node["tipoDocumento"].InnerText);
                            //    if (node["cufe"] != null)
                            //    {
                            //        cufe = node["cufe"].InnerText;
                            //    }
                            //}

                            //SAPbobsCOM.UserTables tblscnf = null;
                            //SAPbobsCOM.UserTable tblcnf = null;
                            //string urlWS = "";

                            //tblscnf = oCompany.UserTables;
                            //tblcnf = tblscnf.Item("FEDIAN_INTERF_CFG");
                            //tblcnf.GetByKey(tipoDoc.ToString());
                            //urlWS = tblcnf.UserFields.Fields.Item("U_URL").Value;

                            //respuestaPDF = Controllers.WebServiceDispapelesController.ConsultaPDF(docEntry, fechaFac, prefijo, tipoDoc, urlWS);

                            //if (respuestaPDF.streamFile != null)
                            //{
                            //    string valuepdf = Convert.ToBase64String(respuestaPDF.streamFile);
                            //    tempDirectory = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".pdf";
                            //    byte[] bytes = Convert.FromBase64String(valuepdf);
                            //    System.IO.FileStream stream = new FileStream(tempDirectory, FileMode.CreateNew);
                            //    System.IO.BinaryWriter writer = new BinaryWriter(stream);
                            //    writer.Write(bytes, 0, bytes.Length);
                            //    writer.Close();
                            //    if (valuepdf != "")
                            //    {
                            //        System.Diagnostics.Process.Start(tempDirectory);
                            //    }
                            //}
                            //else
                            //{
                            //    SBO_Application.MessageBox(respuestaPDF.error);
                            //}
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(tblscnf);
                            //tblscnf = null;
                            //System.Runtime.InteropServices.Marshal.ReleaseComObject(tblcnf);
                            //tblcnf = null;
                            //GC.Collect();
                        }
                    }
                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("DobleClick_FORM_FE_0008: " + ex.Message);
                }
            }

            if (pVal.FormUID == "FORM_FE_0008" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_CLICK && pVal.ItemUID == "ReSend" && pVal.BeforeAction == true)
            {
                try
                {
                    Procesos.senalActiva = false;
                    oItem = oForm.Items.Item("Grid");
                    oGrid = oItem.Specific;
                    SAPbouiCOM.DataTable oDT = oGrid.DataTable;
                    if (oGrid.Rows.SelectedRows.Count > 0)
                    {
                        int LimiteBar;
                        int AvanceBar;
                        int CantClases;

                        LimiteBar = 20;
                        CantClases = oGrid.Rows.SelectedRows.Count;
                        AvanceBar = LimiteBar / CantClases;
                        progressBar = SBO_Application.StatusBar.CreateProgressBar("Procesando Reenvios", LimiteBar, true);

                        for (int i = 0; (i <= (oGrid.Rows.SelectedRows.Count - 1)); i++)
                        {
                            int sCodeLog = Convert.ToInt32(oDT.GetValue("Code", oGrid.GetDataTableRowIndex(oGrid.Rows.SelectedRows.Item(i, SAPbouiCOM.BoOrderType.ot_RowOrder))));
                            string sDocentry = oDT.GetValue("Numero Interno", oGrid.GetDataTableRowIndex(oGrid.Rows.SelectedRows.Item(i, SAPbouiCOM.BoOrderType.ot_RowOrder)));
                            string sStatus = oDT.GetValue("Codigo Estado", oGrid.GetDataTableRowIndex(oGrid.Rows.SelectedRows.Item(i, SAPbouiCOM.BoOrderType.ot_RowOrder)));
                            string sObject = oDT.GetValue("Tipo Documento", oGrid.GetDataTableRowIndex(oGrid.Rows.SelectedRows.Item(i, SAPbouiCOM.BoOrderType.ot_RowOrder)));
                            if (!Constants.green.Contains(sStatus))
                            {
                                Procesos.SendFE(sDocentry, sCodeLog, sObject, true);
                            }
                            progressBar.Value += AvanceBar;
                        }
                        progressBar.Value = LimiteBar;
                        string fechaini = "";
                        string fechafin = "";
                        string tipodoc = "";
                        string estado = "";

                        oForm = SBO_Application.Forms.Item(FormUID);
                        camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("FechaIni").Specific;
                        fechaini = camposedit.Value;

                        camposedit = (SAPbouiCOM.EditText)oForm.Items.Item("FechaFin").Specific;
                        fechafin = camposedit.Value;

                        combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("TipoDoc").Specific;
                        if (combo1.Selected != null)
                        {
                            tipodoc = combo1.Selected.Value;
                        }

                        combo1 = (SAPbouiCOM.ComboBox)oForm.Items.Item("Estado").Specific;
                        if (combo1.Selected != null)
                        {
                            estado = combo1.Selected.Value;
                        }

                        if (fechaini != "" & fechafin != "")
                        {
                            LoadGridLog("FORM_FE_0008", string.Format(Querys.Default.CargueMonitor, fechaini, fechafin, tipodoc, estado));
                        }
                        else
                        {
                            SBO_Application.MessageBox("Debe ingresar parametros de fecha");
                        }
                        progressBar.Stop();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(progressBar);
                        GC.Collect();
                        SBO_Application.SetStatusBarMessage("Reenvio Finalizado", (SAPbouiCOM.BoMessageTime)SAPbouiCOM.BoMessageTime.bmt_Medium, false);
                        Procesos.senalActiva = true;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {
                    progressBar.Stop();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(progressBar);
                    GC.Collect();
                    SBO_Application.MessageBox(ex.Message);
                    Procesos.EscribirLogFileTXT("ReSend_FORM_FE_0008: " + ex.Message);
                    Cursor.Current = Cursors.Default;
                    Procesos.senalActiva = true;
                }
            }

            if (((pVal.ItemUID.Equals("Grid") && pVal.EventType == SAPbouiCOM.BoEventTypes.et_MATRIX_LINK_PRESSED && pVal.Before_Action == true)))
            {
                BubbleEvent = false;
                string ObjectLinkType = Convert.ToString(oGrid.DataTable.Columns.Item("Tipo Objeto").Cells.Item(pVal.Row).Value);
                SAPbouiCOM.EditTextColumn col = (SAPbouiCOM.EditTextColumn)oGrid.Columns.Item("Numero Interno");

                switch (ObjectLinkType)
                {
                    case "13":
                        {
                            col.LinkedObjectType = "13";
                            break;
                        }
                    case "14":
                        {
                            col.LinkedObjectType = "14";
                            break;
                        }

                }
                BubbleEvent = true;
            }

            if (pVal.FormUID == "FORM_FE_0008" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_CLOSE && pVal.BeforeAction == false)
            {
                var dir = new DirectoryInfo(System.IO.Path.GetTempPath());
                foreach (var file in Directory.GetFiles(dir.ToString()))
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                        
                    }
                }
            }

            if (pVal.FormUID == "FORM_FE_0008" && pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_RESIZE && pVal.BeforeAction == false)
            {
                try
                {
                    int posicion = 0;
                    oForm = SBO_Application.Forms.Item(FormUID);
                    oButton = oForm.Items.Item("UPD_form");
                    posicion = oButton.Left;

                    oButton = oForm.Items.Item("ReSend");
                    oButton.Left = posicion + 122;

                }
                catch (Exception ex)
                {
                    SBO_Application.MessageBox(ex.Message);
                }
            }
        }

        //Captura de eventos aplicacion
        private void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {

            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    System.Environment.Exit(0);

                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:

                    if (SBO_Application.Language == SAPbouiCOM.BoLanguages.ln_English | SBO_Application.Language == SAPbouiCOM.BoLanguages.ln_English_Cy | SBO_Application.Language == SAPbouiCOM.BoLanguages.ln_English_Gb | SBO_Application.Language == SAPbouiCOM.BoLanguages.ln_English_Sg)
                    {

                        AddMenuItems();

                    }
                    break;
            }
        }

        //Cargue de DataGrid antes de abrir el formulario Monitor Log
        private void LoadGridLog(string form, string sSQL)
        {
            try
            {
                oForm = SBO_Application.Forms.Item(form);
                //MenuItem.AddMenuItemsToFormMonitor(oForm);
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

                //oCol = (SAPbouiCOM.EditTextColumn)oGrid.Columns.Item("U_DocNum");
                //oCol.LinkedObjectType = "13";

                SAPbouiCOM.CommonSetting settingGrid = oGrid.CommonSetting;

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
                    oCol = (SAPbouiCOM.EditTextColumn)oGrid.Columns.Item("Numero Interno");
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
                Procesos.EscribirLogFileTXT("LoadGridLog: " + ex.Message);
            }
        }

        public static string DecodeTo64(string toDecode)
        {
            byte[] data = Convert.FromBase64String(toDecode);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}