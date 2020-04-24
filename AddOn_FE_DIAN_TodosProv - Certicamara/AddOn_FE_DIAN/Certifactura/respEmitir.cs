using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN.Certifactura
{
    class respEmtir
    {
        public String tipoDocumento { get; set; }
        public String versionDocumento { get; set; }
        public String registrar { get; set; }
        public String cvcc { get; set; }
        public String formato { get; set; }
        public String momentoPeticion { get; set; }
        public String momentoRespuesta { get; set; }
        public String codigoEstado { get; set; }
        public List<listaError> listaErrores { get; set; }
        public String xmlEnviado { get; set; }
        public String respuestaDian { get; set; }
    }
    public class listaError
    {
        public string codigo
        { get; set; }
        public string valor
        { get; set; }
    }
}