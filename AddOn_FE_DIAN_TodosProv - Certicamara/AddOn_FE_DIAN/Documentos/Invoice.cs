using System.Collections.Generic;

namespace AddOn_FE_DIAN.Documentos
{
    public class Invoice
    {
        public string tipoDocumento { get; set; }
        public string versionDocumento { get; set; }
        public bool registrar { get; set; }
        public string control { get; set; }
        public string codigoTipoDocumento { get; set; }
        public string tipoOperacion { get; set; }
        public string prefijoDocumento { get; set; }
        public int  numeroDocumento { get; set; }
        public string fechaEmision { get; set; }
        public string horaEmision { get; set; }
        public string periodoFacturacion { get; set; }
        public int numeroLineas { get; set; }
        public decimal subtotal { get; set; }
        public decimal totalBaseImponible { get; set; }
        public decimal subtotalMasTributos { get; set; }
        public decimal totalDescuentos { get; set; }
        public decimal totalCargos { get; set; }
        public decimal totalAnticipos { get; set; }
        public decimal total { get; set; }
        public string codigoMoneda { get; set; }

        public TasaCambio tasaCambio { get; set; }
        public Pago pago { get; set; }
        public List<InvoiceLine> listaProductos { get; set; }
        public List<DocRef> listaDocumentosReferenciados { get; set; }
        public DocRef notasReferenciadas { get; set; }
        public List<CargosDescuentos> listaCargosDescuentos { get; set; }
        public List<GrupoImpuestos> gruposImpuestos { get; set; }
        public List<GrupoDeducciones> gruposDeducciones { get; set; }

        public Facturador facturador { get; set; }
        public Adquiriente adquiriente { get; set; }
        public Autorizado Autorizado { get; set; }

        public string urlAnexos { get; set; }
        public string base64 { get; set; }
        public string posicionXCufe { get; set; }
        public string posicionYCufe { get; set; }
        public string rotacionCufe { get; set; }
        public string fuenteCufe { get; set; }
        public string posicionXQr { get; set; }
        public string posicionYQr { get; set; }
        public string descripcionGeneral { get; set; }
        public Resolucion resolucion { get; set; }
        public string cvcc { get; set; }
        public string formato { get; set; }
        public string fechaHoraRecepcion { get; set; }
        public string cufe { get; set; }
        public string qr { get; set; }
    }

    public class TasaCambio
    {
        public string fechaCambio { get; set; }
        public string codigoMonedaFacturado { get; set; }
        public string codigoMonedaCambio { get; set; }
        public decimal baseCambioFacturado { get; set; }
        public decimal baseCambio { get; set; }
        public decimal trm { get; set; }
    }

    public class Pago
    {
        public int id { get; set; }
        public string codigoMedioPago { get; set; }
        public string fechaVencimiento { get; set; }
    }

    public class Facturador
    {
        public string razonSocial { get; set; }
        public string nombreRegistrado { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public string digitoVerificacion { get; set; }
        public string naturaleza { get; set; }
        public string codigoRegimen { get; set; }
        public string responsabilidadFiscal { get; set; }
        public string codigoImpuesto { get; set; }
        public string nombreImpuesto { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public Contacto contacto { get; set; }
        public dirección direccion { get; set; }
        public dirección direccionFiscal { get; set; }
        public List<ResponTribu> listaResponsabilidadesTributarias { get; set; }
        public string codigoCIIU { get; set; }
        public string sucursal { get; set; }
        public string listaParticipantesConsorcio { get; set; }
    }

    public class Adquiriente
    {
        public string razonSocial { get; set; }
        public string nombreRegistrado { get; set; }
        public string tipoIdentificacion { get; set; }
        public string identificacion { get; set; }
        public string digitoVerificacion { get; set; }
        public string naturaleza { get; set; }
        public string codigoRegimen { get; set; }
        public string responsabilidadFiscal { get; set; }
        public string codigoImpuesto { get; set; }
        public string nombreImpuesto { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public Contacto contacto { get; set; }
        public dirección direccion { get; set; }
        public dirección direccionFiscal { get; set; }
        public List<ResponTribu> listaResponsabilidadesTributarias { get; set; }
        public string codigoCIIU { get; set; }
        public string sucursal { get; set; }
        public string centroCosto { get; set; }
        public string usarCertiMail { get; set; }
    }

    public class Autorizado
    {
        public string razonSocial { get; set; }
        public string tipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public string digitoVerificacion { get; set; }
    }

    public class Contacto
    {
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string observaciones { get; set; }
    }

    public class dirección
    {
        public string codigoPais { get; set; }
        public string nombrePais { get; set; }
        public string codigoLenguajePais { get; set; }
        public string codigoDepartamento { get; set; }
        public string nombreDepartamento { get; set; }
        public string codigoCiudad { get; set; }
        public string nombreCiudad { get; set; }
        public string direccionFisica { get; set; }
        public string codigoPostal { get; set; }
    }

    public class ResponTribu
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
    }

    public class Numeracion
    {
        public string prefijo { get; set; }
        public int desde { get; set; }
        public int hasta { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
    }

    public class Resolucion
    {
        public string numero { get; set; }
        public string fechaInicio { get; set; }
        public string fechaFin { get; set; }
        public Numeracion numeracion { get; set; }
    }

    public class GrupoImpuestos
    {
        public string codigo { get; set; }
        public decimal total { get; set; }
        public List<InvoiceTax> listaImpuestos { get; set; }
    }

    public class GrupoDeducciones
    {
        public string codigo { get; set; }
        public decimal total { get; set; }
        public List<InvoiceTax> listaDeducciones { get; set; }
    }
}