using System;
using System.Windows.Forms;

namespace AddOn_FE_DIAN
{
    static class Program
    {
        //public static SAPbobsCOM.Company oCompany;

        public static SAPbouiCOM.ProgressBar oProgBar;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {//Conexion con SAP
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Connection oConnection = new Connection();
                oConnection.SetApplication();

                int LimiteBar;
                int AvanceBar;
                int CantClases;
                bool CargueIni;

                CargueIni = PreCarga.version(oConnection.oCompany);

                LimiteBar = 20;
                CantClases = 4;
                AvanceBar = LimiteBar / CantClases;

                //Barra de progreso en SAP
                oProgBar = oConnection.SBO_Application.StatusBar.CreateProgressBar("Inicio " + "AddOnFE_DIAN", LimiteBar, true);

                oProgBar.Text = "Inicio Carga AddOn ... Conexión Satisfactoria. ";
                oProgBar.Value += AvanceBar;
                //Creacion de tablas y campos de usuario
                if (CargueIni == true)
                {
                    Tables oTables = null;
                    oTables = new Tables(oConnection.oCompany, oConnection.SBO_Application, CargueIni);
                }

                oProgBar.Text = "Cargando AddOn ... Proceso (1) ";
                oProgBar.Value += AvanceBar;
                //Creacion de menus y formularios
                MenuItem oMenus = null;
                oMenus = new MenuItem(oConnection.oCompany, oConnection.SBO_Application);
                //MenuItem oMenuReports = new MenuItem(oConnection.oCompany, oConnection.SBO_Application);

                oProgBar.Text = "Cargando AddOn ... Proceso (2) ";
                oProgBar.Value += AvanceBar;
                //clase donde se realizan algunos procesos del addon
                Procesos oProcesos = null;
                oProcesos = new Procesos(oConnection.oCompany, oConnection.SBO_Application);

                oProgBar.Text = "Cargando AddOn ... Proceso (3) ";
                oProgBar.Value += AvanceBar;
                //PreCarga Tablas y configuracion
                if (CargueIni == true)
                {
                    PreCarga oPreCarga = null;
                    oPreCarga = new PreCarga(oConnection.oCompany, oConnection.SBO_Application);
                }

                oProgBar.Text = "Carga Satisfactoria " + "AddOnFE_DIAN";
                oProgBar.Value = LimiteBar;

                try
                {
                    //Finalizacion de la barra de proceso
                    oProgBar.Stop();
                }
                catch (Exception)
                {
                }
                oConnection.SBO_Application.SetStatusBarMessage("Carga Satisfactoria AddOnFE_DIAN", (SAPbouiCOM.BoMessageTime)SAPbouiCOM.BoMessageTime.bmt_Medium, false);
                Application.Run();
            }
            catch (Exception ex)
            {
                MessageBox.Show("(70) - " + ex.Message);
            }
        }
    }
}
