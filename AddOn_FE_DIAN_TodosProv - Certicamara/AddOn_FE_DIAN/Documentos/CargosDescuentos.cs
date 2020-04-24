using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddOn_FE_DIAN.Documentos
{
    public class CargosDescuentos
    {
        public int id { get; set; }
        public bool esCargo { get; set; }
        public string codigo { get; set; }
        public string Razon { get; set; }
        public decimal Base { get; set; }
        public decimal porcentaje { get; set; }
        public decimal valor { get; set; }
    }
}
