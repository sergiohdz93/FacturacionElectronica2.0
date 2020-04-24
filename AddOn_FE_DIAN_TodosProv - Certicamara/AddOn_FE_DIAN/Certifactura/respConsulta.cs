using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN.Certifactura
{
    class respConsulta
    {
        public String codigo { get; set; }
        public String descCodigo { get; set; }
        public String cufe { get; set; }
        public String qr { get; set; }
        public List<listaErrores> listaErrores { get; set; }
        public String documento { get; set; }
    }

    public class listaErrores
    {
        public string codigo
        { get; set; }
        public string valor
        { get; set; }
    }
}
