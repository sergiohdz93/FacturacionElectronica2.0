using System.Collections.Generic;

namespace AddOn_FE_DIAN.Documentos
{
    public class InvoiceLine
    {
        public int numeroLinea { get; set; }
        public string informacion { get; set; }
        public decimal cantidad { get; set; }
        public decimal valorTotal { get; set; }
        public string idProducto { get; set; }
        public string codigoPrecio { get; set; }
        public decimal valorUnitario { get; set; }
        public decimal cantidadReal { get; set; }
        public string codigoUnidad { get; set; }
        public bool esMuestraComercial { get; set; }
        public Item item { get; set; }

        public List<CargosDescuentos> listaCargosDescuentos { get; set; }
        public List<InvoiceTax> listaImpuestos { get; set; }

        public class Item
        {
            public string marca { get; set; }
            public string modelo { get; set; }
            public string codigoArticuloVendedor { get; set; }
            public string codigoExtendidoVendedor { get; set; }
            public string codigoEstandar { get; set; }
            public string nombreEstandar { get; set; }
            public string descripcion { get; set; }
            //public decimal cantidadPaquete { get; set; }
        }
    }
}
