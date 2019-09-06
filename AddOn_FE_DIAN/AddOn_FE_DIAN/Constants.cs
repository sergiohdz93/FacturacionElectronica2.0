using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN
{
    class Constants
    {
        public static string categorQuery = "SELECT \"CategoryId\" FROM OQCN WHERE \"CatName\" = 'FE_DIAN'";
        public static string[] green = new string[] { "1", "10"};
        public static string[] yellow = new string[] { "7", "99", "127", "137" };
        public static string[] red = new string[] { "2", "3", "5", "102", "106", "116", "117", "118", "126", "160", "147"};
        //public static string urlstatusFebos = "https://api.febos.co/pruebas/documentos/{0}?xml=si&imagen=si&tipoImagen=0&regenerar=si&incrustar=no&xmlFirmado=si";
        //public static string urlstatusFolio = "https://api.febos.co/pruebas/documentos?campos=tipoDocumento,folio&itemsPorPagina=20&pagina=1&filtros=nitEmisor:{0}|folio:{1}";

        //public static string[] CodDIAN_01 = { "1", "Factura de Venta" };
        //public static string[] CodDIAN_02 = { "2", "Factura de Exportacion" };
        //public static string[] CodDIAN_03 = { "3", "Factura de Contingencia" };
        //public static string[] CodDIAN_04 = { "4", "Nota de Credito" };
        //public static string[] CodDIAN_05 = { "5", "Nota de Debito" };

        //public static string[] concepto_ND1 = { "1", "Intereses" };
        //public static string[] concepto_ND2 = { "2", "Gastos por cobrar" };
        //public static string[] concepto_ND3 = { "3", "Cambio del valor" };

        //public static string[] concepto_NC1 = { "1", "Devolución de parte de los bienes, no aceptación de partes del servicio" };
        //public static string[] concepto_NC2 = { "2", "Anulación de factura electrónica" };
        //public static string[] concepto_NC3 = { "3", "Rebaja total aplicada" };
        //public static string[] concepto_NC4 = { "4", "Descuento total aplicado" };
        //public static string[] concepto_NC5 = { "5", "Rescisión: nulidad por falta de requisitos" };
        //public static string[] concepto_NC6 = { "6", "Otros" };
    }
}
