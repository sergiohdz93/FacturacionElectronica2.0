using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class Connection
    {
        public SAPbouiCOM.Application SBO_Application;
        public SAPbobsCOM.Company oCompany;
        public void SetApplication()
        {
            SAPbouiCOM.SboGuiApi SboGuiApi = null;
            string sConnectionString = null;
            SboGuiApi = new SAPbouiCOM.SboGuiApi();
            sConnectionString = System.Convert.ToString(Environment.GetCommandLineArgs().GetValue(1));
            // connect to a running SBO Application
            try
            {
                SboGuiApi.Connect(sConnectionString);
                // get an initialized application object
                SBO_Application = SboGuiApi.GetApplication();
                SBO_Application.SetStatusBarMessage("Se ha iniciado el Add-on de FE_DIAN", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                oCompany = new SAPbobsCOM.Company();
                //get DI company (via UI)
                oCompany = (SAPbobsCOM.Company)SBO_Application.Company.GetDICompany();
            }
            catch (Exception ex)
            { //  Connection failed
                System.Windows.Forms.MessageBox.Show("Error al iniciar el Add-on de Youse" + ex.Message, "Error de conexión",
                    System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Error,
                    System.Windows.Forms.MessageBoxDefaultButton.Button1, System.Windows.Forms.MessageBoxOptions.DefaultDesktopOnly);
                System.Environment.Exit(0);
            }
        }
    }
}
