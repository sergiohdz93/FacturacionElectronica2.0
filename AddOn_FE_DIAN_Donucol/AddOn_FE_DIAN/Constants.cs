using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class Constants
    {
        public static string categorQuery = "SELECT \"CategoryId\" FROM \"OQCN\" WHERE \"CatName\" = 'FEDIAN'";
        public static string[] green = new string[] { "1", "10", "7200001", "7200002" };
        public static string[] yellow = new string[] { "2", "7", "127", "7200003", "10010" };
        public static string[] red = new string[] {
                                                    "3","5", "118", "102", "106", "116",
                                                    "10001", "10002", "10003", "10004", "10005", "10006", "10009", "10013",
                                                    "7200004", "7200005"
                                                  };
        public static string urlstatusFebos = "https://api.febos.co/pruebas/documentos/{0}?xml=si&imagen=si&tipoImagen=0&regenerar=si&incrustar=no&xmlFirmado=si";

        public static string[] CodDIAN_01 = { "1", "Factura de Venta" };
        public static string[] CodDIAN_04 = { "2", "Nota de Credito" };
        public static string[] CodDIAN_05 = { "3", "Nota de Debito" };
        public static string[] CodDIAN_03 = { "4", "Factura de Exportacion" };
        public static string[] CodDIAN_02 = { "5", "Factura de Contingencia" };       
    }
}
