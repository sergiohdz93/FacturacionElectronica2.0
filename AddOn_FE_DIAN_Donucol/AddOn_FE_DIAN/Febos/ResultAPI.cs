using System;
using System.Collections.Generic;

namespace AddOn_FE_DIAN
{
    class ResultAPI
    {
        public String Codigo { get; set; }
        public String mensaje { get; set; }
        public String seguimientoId { get; set; }
        public String hora { get; set; }
        public String febosID { get; set; }
        public String xmlLink { get; set; }
        public String imagenLink { get; set; }
        public List<document> documentos { get; set; }
    }
    public class document
    {
        public string febosId
        { get; set; }
        public string folio
        { get; set; }
    }
}