using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN.Documentos
{
    public class InvoiceTax
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public decimal baseGravable { get; set; }
        public decimal porcentaje { get; set; }
        public decimal valor { get; set; }
        public string codigoUnidad { get; set; }
    }
}
