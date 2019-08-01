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
        public static string urlstatusFebos = "https://api.febos.co/pruebas/documentos/{0}?xml=si&imagen=si&tipoImagen=0&regenerar=si&incrustar=no&xmlFirmado=si";
        public static string urlstatusFolio = "https://api.febos.co/pruebas/documentos?campos=tipoDocumento,folio&itemsPorPagina=20&pagina=1&filtros=nitEmisor:{0}|folio:{1}";


        public static string[] CodDIAN_01 = { "1", "Factura de Venta" };
        public static string[] CodDIAN_02 = { "2", "Factura de Contingencia" };
        public static string[] CodDIAN_03 = { "3", "Factura de Exportacion" };
        public static string[] CodDIAN_04 = { "4", "Nota de Credito" };
        public static string[] CodDIAN_05 = { "5", "Nota de Debito" };
    }
}
