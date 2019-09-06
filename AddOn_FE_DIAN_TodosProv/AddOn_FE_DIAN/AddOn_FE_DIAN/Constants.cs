using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class Constants
    {
        public static string categorQuery = "SELECT \"CategoryId\" FROM \"OQCN\" WHERE \"CatName\" = 'FE_DIAN'";
        public static string[] green = new string[] { "1", "10", "7200001", "7200002" };
        public static string[] yellow = new string[] { "2", "7", "99", "127", "7200003", "10010" };
        public static string[] red = new string[] {
                                                    "3","5", "118", "102", "106", "116",
                                                    "10001", "10002", "10003", "10004", "10005", "10006", "10009", "10013",
                                                    "7200004", "7200005"
                                                  };
        public static string urlstatusFebos = "https://cenflab.cen.biz/";
        public static string urlstatusFolio = "https://cenflab.cen.biz/";


        public static string[] CodDIAN_01 = { "01", "Factura de Venta Nacional" };
        public static string[] CodDIAN_02 = { "02", "Factura de Exportacion" };
        public static string[] CodDIAN_03 = { "03", "Factura de Contingencia Facturador" };
        public static string[] CodDIAN_04 = { "04", "Factura de Contingencia DIAN" };
        public static string[] CodDIAN_05 = { "91", "Nota de Credito" };
        public static string[] CodDIAN_06 = { "92", "Nota de Debito" };
    }
}
