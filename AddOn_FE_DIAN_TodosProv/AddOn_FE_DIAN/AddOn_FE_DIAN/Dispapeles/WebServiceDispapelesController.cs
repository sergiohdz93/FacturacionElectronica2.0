using System;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Channels;
using System.Data;
using System.IO;
using System.Globalization;

namespace AddOn_FE_DIAN.Controllers
{
    public class WebServiceDispapelesController
    {
        public static BasicHttpsBinding ObtenerBindingsHttps()
        {
            BasicHttpsBinding binding = new BasicHttpsBinding();

            binding.CloseTimeout = new TimeSpan(0, 1, 0);//1 minutos
            binding.OpenTimeout = new TimeSpan(0, 1, 0);//1 minutos
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);//10 minutos
            binding.SendTimeout = new TimeSpan(0, 1, 0);//1 minutos
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 655360;
            binding.MaxBufferPoolSize = 524288;
            binding.MaxReceivedMessageSize = 655360;
            binding.MessageEncoding = WSMessageEncoding.Text;
            binding.TextEncoding = Encoding.UTF8;
            binding.TransferMode = TransferMode.Buffered;
            binding.UseDefaultWebProxy = true;
            binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            return binding;
        }

        public static WSDispapeles.envioFacturaRespuestaDTO EnviarFactura(DataTable Fac, DataTable impFactura, string wsURL)
        {
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                Procesos.EscribirLogFileTXT("envioFacturaRespuestaDTO: Inicio");
                string urlServicio;
                urlServicio = wsURL;

                WSDispapeles.eBfelEncabezadofactura Factura = new WSDispapeles.eBfelEncabezadofactura();
                WSDispapeles.InterSoapClient clienteServicio;
                WSDispapeles.envioFacturaRespuestaDTO response;

                clienteServicio = new WSDispapeles.InterSoapClient(ObtenerBindingsHttps(), new EndpointAddress(urlServicio));
                using (new OperationContextScope(clienteServicio.InnerChannel))
                {
                    //Add SOAP Header (Header property in the envelope) to an outgoing request.

                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["username"] = Procesos.username;
                    requestMessage.Headers["password"] = Procesos.password;
                    requestMessage.Headers["token"] = Procesos.token;

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    int i = 0;
                    Factura.tokenempresa = Procesos.token;
                    Factura.tipodocumento = Procesos.Buscar_ValorCab("tipodocumento", i, Fac);
                    Factura.codigoPlantillaPdf = Convert.ToInt32(Procesos.Buscar_ValorCab("codigoPlantillaPdf", i, Fac));
                    Factura.tiponota = Procesos.Buscar_ValorCab("tiponota", i, Fac);
                    Factura.prefijo = Procesos.Buscar_ValorCab("prefijo", i, Fac);
                    Factura.consecutivo = Convert.ToInt32(Procesos.Buscar_ValorCab("consecutivo", i, Fac));
                    Factura.fechafacturacion = Convert.ToDateTime(Procesos.Buscar_ValorCab("fechafacturacion", i, Fac));
                    Factura.fechafacturacionSpecified = true;// fechaFacturaEspecify
                    Factura.fechavencimiento = Convert.ToDateTime(Procesos.Buscar_ValorCab("fechavencimiento", i, Fac));
                    Factura.fechavencimientoSpecified = true;// fechaFacturaEspecify
                    Factura.moneda = Procesos.Buscar_ValorCab("moneda", i, Fac);
                    Factura.totalimportebruto = Convert.ToDouble(Procesos.Buscar_ValorCab("totalimportebruto", i, Fac));
                    Factura.totalbaseimponible = Convert.ToDouble(Procesos.Buscar_ValorCab("totalbaseimponible", i, Fac));
                    Factura.descuento = Convert.ToDouble(Procesos.Buscar_ValorCab("descuento", i, Fac));
                    Factura.totalfactura = Convert.ToDouble(Procesos.Buscar_ValorCab("totalfactura", i, Fac));
                    Factura.tiponota = Procesos.Buscar_ValorCab("tiponota", i, Fac);
                    Factura.tipopersona = Procesos.Buscar_ValorCab("tipopersona", i, Fac);
                    Factura.razonsocial = Procesos.Buscar_ValorCab("razonsocial", i, Fac);
                    Factura.descripcion = Procesos.Buscar_ValorCab("descripcion", i, Fac);

                    Factura.primerapellido = Procesos.Buscar_ValorCab("primerapellido", i, Fac);
                    Factura.segundoapellido = Procesos.Buscar_ValorCab("segundoapellido", i, Fac);
                    Factura.primernombre = Procesos.Buscar_ValorCab("primernombre", i, Fac);
                    Factura.segundonombre = Procesos.Buscar_ValorCab("segundonombre", i, Fac);
                    Factura.tipoidentificacion = Convert.ToInt32(Procesos.Buscar_ValorCab("tipoidentificacion", i, Fac));
                    Factura.numeroidentificacion = Procesos.Buscar_ValorCab("numeroidentificacion", i, Fac);
                    Factura.aplicafel = Procesos.Buscar_ValorCab("aplicafel", i, Fac);
                    Factura.envioPorEmailPlataforma = Procesos.Buscar_ValorCab("envioPorEmailPlataforma", i, Fac);
                    Factura.email = Procesos.Buscar_ValorCab("email", i, Fac);

                    Factura.pais = Procesos.Buscar_ValorCab("pais", i, Fac);
                    Factura.departamento = Procesos.Buscar_ValorCab("departamento", i, Fac);
                    Factura.ciudad = Procesos.Buscar_ValorCab("ciudad", i, Fac);
                    Factura.direccion = Procesos.Buscar_ValorCab("direccion", i, Fac);
                    Factura.telefono = Procesos.Buscar_ValorCab("telefono", i, Fac);
                    Factura.tipocompra = Convert.ToInt32(Procesos.Buscar_ValorCab("tipocompra", i, Fac));
                    Factura.periododepagoaSpecified = true;
                    Factura.periododepagoa = Convert.ToInt32(Procesos.Buscar_ValorCab("periododepagoa", i, Fac));


                    Factura.despachadoANombre = Convert.ToString(Procesos.Buscar_ValorCab("despachadoANombre", i, Fac));
                    Factura.despachadoATelefono = Convert.ToString(Procesos.Buscar_ValorCab("despachadoATelefono", i, Fac));
                    Factura.despachadoADireccion = Convert.ToString(Procesos.Buscar_ValorCab("despachadoADireccion", i, Fac));
                    Factura.documentoEmitidoEn = Convert.ToString(Procesos.Buscar_ValorCab("documentoEmitidoEn", i, Fac));
                    Factura.condicionPagoReferencia = Procesos.Buscar_ValorCab("nombreCondPago", i, Fac);

                    Factura.campoAdicional1 = Procesos.Buscar_ValorCab("campoAdicional1", i, Fac);
                    Factura.campoAdicional3 = Procesos.Buscar_ValorCab("campoAdicional3", i, Fac);
                    Factura.campoAdicional6 = Procesos.Buscar_ValorCab("campoAdicional6", i, Fac);

                    int NumDetalles = Fac.Rows.Count;
                    WSDispapeles.eBfelDetallefactura[] Factura_Detalles = new WSDispapeles.eBfelDetallefactura[NumDetalles];
                    //WSDispapeles.eBfelDetallefactura LineaDetalle = new WSDispapeles.eBfelDetallefactura();

                    i = 0;
                    if (Fac.Rows.Count > 0)
                    {
                        foreach (DataRow _row in Fac.Rows)
                        {
                            Procesos.EscribirLogFileTXT("eBfelDetallefactura: Inicio");
                            WSDispapeles.eBfelDetallefactura LineaDetalle = new WSDispapeles.eBfelDetallefactura();
                            LineaDetalle.cantidadSpecified = true;
                            LineaDetalle.cantidad = Convert.ToDouble(Procesos.Buscar_ValorCab("cantidad", i, Fac));
                            LineaDetalle.unidadmedida = Convert.ToString(Procesos.Buscar_ValorCab("unidadmedida", i, Fac));
                            LineaDetalle.valorunitario = Convert.ToDouble(Procesos.Buscar_ValorCab("valorunitario", i, Fac));
                            LineaDetalle.porcentajedescuento = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentajedescuento", i, Fac));
                            //LineaDetalle.descuento = Convert.ToDouble(Procesos.Buscar_ValorCab("descuentoLin", i, Fac));
                            LineaDetalle.preciosinimpuestos = Convert.ToDouble(Procesos.Buscar_ValorCab("preciosinimpuestos", i, Fac));
                            LineaDetalle.preciototal = Convert.ToDouble(Procesos.Buscar_ValorCab("preciototal", i, Fac));
                            LineaDetalle.codigoproducto = Procesos.Buscar_ValorCab("codigoproducto", i, Fac);
                            LineaDetalle.descripcion = Procesos.Buscar_ValorCab("descripcionLine", i, Fac);
                            LineaDetalle.referencia = Procesos.Buscar_ValorCab("referencia", i, Fac);
                            LineaDetalle.campoadicional1 = Convert.ToString(Procesos.Buscar_ValorCab("detcampoadicional1", i, Fac));
                            LineaDetalle.campoadicional9 = Convert.ToString(Procesos.Buscar_ValorCab("detcampoadicional9", i, Fac));
                            LineaDetalle.campoadicional10 = Convert.ToString(Procesos.Buscar_ValorCab("detcampoadicional10", i, Fac));

                            Factura_Detalles[i] = LineaDetalle;
                            i++;
                        }
                        Factura.listaDetalle = Factura_Detalles;
                    }



                    int NumImpuestos = impFactura.Rows.Count;
                    WSDispapeles.eBfelImpuestos[] Factura_Impuestos = new WSDispapeles.eBfelImpuestos[NumImpuestos];
                    //WSDispapeles.eBfelImpuestos LineaImpuestos = new WSDispapeles.eBfelImpuestos();

                    i = 0;
                    if (impFactura.Rows.Count > 0)
                    {
                        foreach (DataRow _row in impFactura.Rows)
                        {
                            Procesos.EscribirLogFileTXT("eBfelImpuestos: Inicio");
                            WSDispapeles.eBfelImpuestos LineaImpuestos = new WSDispapeles.eBfelImpuestos();
                            LineaImpuestos.codigoproducto = Procesos.Buscar_ValorCab("codigoproducto", i, impFactura);
                            LineaImpuestos.codigoImpuestoRetencion = Procesos.Buscar_ValorCab("codigoImpuestoRetencion", i, impFactura);
                            LineaImpuestos.porcentaje = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentaje", i, impFactura));
                            LineaImpuestos.valorImpuestoRetencion = Convert.ToDouble(Procesos.Buscar_ValorCab("valorImpuestoRetencion", i, impFactura));
                            LineaImpuestos.baseimponible = Convert.ToDouble(Procesos.Buscar_ValorCab("baseimponible", i, impFactura));

                            Factura_Impuestos[i] = LineaImpuestos;
                            i++;
                        }
                        Factura.listaImpuestos = Factura_Impuestos;
                    }

                    i = 0;
                    int docBase = 1;
                    if (impFactura.Rows.Count > 0)
                    {
                        if (Procesos.Buscar_ValorCab("consecutivofacturamodificada", i, Fac) != "")
                        {
                            Procesos.EscribirLogFileTXT("eBfelFacturamodificada: Inicio" + Procesos.Buscar_ValorCab("tipodocumento", i, Fac));
                            WSDispapeles.eBfelFacturamodificada[] Notas_DocBase = new WSDispapeles.eBfelFacturamodificada[docBase];

                            WSDispapeles.eBfelFacturamodificada LineadocBase = new WSDispapeles.eBfelFacturamodificada();
                            LineadocBase.consecutivofacturamodificada = Procesos.Buscar_ValorCab("consecutivofacturamodificada", i, Fac);
                            LineadocBase.cufefacturamodificada = Procesos.Buscar_ValorCab("cufefacturamodificada", i, Fac);
                            LineadocBase.fechafacturamodificadaSpecified = true;
                            LineadocBase.fechafacturamodificada = DateTime.Parse(Procesos.Buscar_ValorCab("fechafacturamodificada", i, Fac));

                            Notas_DocBase[i] = LineadocBase;

                            Factura.listaFacturasmodificadas = Notas_DocBase;
                        }
                    }
                    Procesos.EscribirLogFileTXT("Consumo: Inicio");
                    response = clienteServicio.enviarFactura(Factura);
                    Procesos.EscribirLogFileTXT("Consumo: Fin");
                }
                var serxml = new System.Xml.Serialization.XmlSerializer(Factura.GetType());
                var ms = new MemoryStream();
                serxml.Serialize(ms, Factura);
                string xml = Encoding.UTF8.GetString(ms.ToArray());
                Procesos.requestSend = xml;

                clienteServicio.Close();
                return response;
            }
            catch (Exception ex)
            {
                WSDispapeles.envioFacturaRespuestaDTO response = null;
                Procesos.EscribirLogFileTXT("SendDispapeles: " + ex.Message);
                return response;
            }
        }

        public static WSDispapeles.documentoElectronicoWsDto ConsultaPDF(int numDoc, DateTime fechaFac, string prefijo, int tipoDoc, string wsURL)
        {
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                Procesos.EscribirLogFileTXT("ConsultaPDF: Inicio");
                string urlServicio;
                urlServicio = wsURL;

                WSDispapeles.ebFelConsultaFacturaWS consultaPDF = new WSDispapeles.ebFelConsultaFacturaWS();
                WSDispapeles.InterSoapClient clienteServicio;
                WSDispapeles.documentoElectronicoWsDto response;

                clienteServicio = new WSDispapeles.InterSoapClient(ObtenerBindingsHttps(), new EndpointAddress(urlServicio));
                using (new OperationContextScope(clienteServicio.InnerChannel))
                {
                    //Add SOAP Header (Header property in the envelope) to an outgoing request.

                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["username"] = Procesos.username;
                    requestMessage.Headers["password"] = Procesos.password;
                    requestMessage.Headers["token"] = Procesos.token;


                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;
                    var dateAndTime = fechaFac;
                    var date = dateAndTime.Date;
                    consultaPDF.consecutivo = numDoc;
                    consultaPDF.fechafacturacionString = fechaFac.ToString("yyyyMMdd");
                    consultaPDF.prefijo = prefijo;
                    consultaPDF.tipodocumento = tipoDoc;
                    consultaPDF.tokenempresa = Procesos.token;

                    response = clienteServicio.consultarPdfFactura(consultaPDF);

                }
                //var serxml = new System.Xml.Serialization.XmlSerializer(consultaPDF.GetType());
                //var ms = new MemoryStream();
                //serxml.Serialize(ms, consultaPDF);
                //string xml = Encoding.UTF8.GetString(ms.ToArray());
                //Procesos.requestSend = xml;

                clienteServicio.Close();
                Procesos.EscribirLogFileTXT("ConsultaPDF: Fin");
                return response;
            }
            catch (Exception ex)
            {
                WSDispapeles.documentoElectronicoWsDto response = null;
                Procesos.EscribirLogFileTXT("PDFDispapeles: " + ex.Message);
                return response;
            }
        }

        public static WSDispapeles.documentoElectronicoWsDto ConsultaXML(int numDoc, DateTime fechaFac, string prefijo, int tipoDoc, string wsURL)
        {
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                Procesos.EscribirLogFileTXT("ConsultaXML: Inicio");
                string urlServicio;
                urlServicio = wsURL;

                WSDispapeles.ebFelConsultaFacturaWS consultaPDF = new WSDispapeles.ebFelConsultaFacturaWS();
                WSDispapeles.InterSoapClient clienteServicio;
                WSDispapeles.documentoElectronicoWsDto response;

                clienteServicio = new WSDispapeles.InterSoapClient(ObtenerBindingsHttps(), new EndpointAddress(urlServicio));
                using (new OperationContextScope(clienteServicio.InnerChannel))
                {
                    //Add SOAP Header (Header property in the envelope) to an outgoing request.

                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["username"] = Procesos.username;
                    requestMessage.Headers["password"] = Procesos.password;
                    requestMessage.Headers["token"] = Procesos.token;


                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    consultaPDF.consecutivo = numDoc;
                    consultaPDF.fechafacturacionString = fechaFac.ToString("yyyyMMdd");
                    consultaPDF.prefijo = prefijo;
                    consultaPDF.tipodocumento = tipoDoc;
                    consultaPDF.tokenempresa = Procesos.token;

                    response = clienteServicio.consultarXmlFactura(consultaPDF);

                }
                //var serxml = new System.Xml.Serialization.XmlSerializer(Factura.GetType());
                //var ms = new MemoryStream();
                //serxml.Serialize(ms, Factura);
                //string xml = Encoding.UTF8.GetString(ms.ToArray());
                //Procesos.requestSend = xml;

                clienteServicio.Close();
                Procesos.EscribirLogFileTXT("ConsultaXML: Fin");
                return response;
            }
            catch (Exception ex)
            {
                WSDispapeles.documentoElectronicoWsDto response = null;
                Procesos.EscribirLogFileTXT("XMLDispapeles: " + ex.Message);
                return response;
            }
        }
    }
}