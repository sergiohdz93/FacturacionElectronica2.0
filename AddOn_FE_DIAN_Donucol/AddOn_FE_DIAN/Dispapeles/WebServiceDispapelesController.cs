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
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
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

        public static enviarDocumentoDispape.felRespuestaEnvio EnviarFactura(DataTable Fac, DataTable impFactura, DataTable listAdi, string wsURL)
        {
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                Procesos.EscribirLogFileTXT("envioFacturaRespuestaDTO: Inicio");
                string urlServicio;
                urlServicio = wsURL;

                enviarDocumentoDispape.enviarDocumentoResponse response = new enviarDocumentoDispape.enviarDocumentoResponse();
                enviarDocumentoDispape.felRespuestaEnvio respuesta = new enviarDocumentoDispape.felRespuestaEnvio();
                enviarDocumentoDispape.felCabezaDocumento Factura = new enviarDocumentoDispape.felCabezaDocumento();
                enviarDocumentoDispape.WsEnviarDocumentoClient clienteServicio;

                clienteServicio = new enviarDocumentoDispape.WsEnviarDocumentoClient(ObtenerBindingsHttps(), new EndpointAddress(urlServicio));
                using (new OperationContextScope(clienteServicio.InnerChannel))
                {
                    //Add SOAP Header (Header property in the envelope) to an outgoing request.

                    HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
                    requestMessage.Headers["username"] = Procesos.username;
                    requestMessage.Headers["password"] = Procesos.password;
                    requestMessage.Headers["token"] = Procesos.token;

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

                    int i = 0;
                    int NumDetalles = Fac.Rows.Count;

                    Factura.idEmpresa = Convert.ToInt32(Procesos.idEmpresa);
                    Factura.idEmpresaSpecified = true;
                    Factura.usuario = Procesos.username;
                    Factura.contrasenia = Procesos.password;
                    Factura.token = Procesos.token;
                    Factura.version = Procesos.Buscar_ValorCab("version", i, Fac);
                    Factura.tipodocumento = Procesos.Buscar_ValorCab("tipodocumento", i, Fac);
                    Factura.prefijo = Procesos.Buscar_ValorCab("prefijo", i, Fac);
                    Factura.consecutivo = Convert.ToInt32(Procesos.Buscar_ValorCab("consecutivo", i, Fac));
                    Factura.fechafacturacion = Convert.ToDateTime(Procesos.Buscar_ValorCab("fechafacturacion", i, Fac));
                    Factura.fechafacturacionSpecified = true;
                    Factura.codigoPlantillaPdf = Convert.ToInt32(Procesos.Buscar_ValorCab("codigoPlantillaPdf", i, Fac));
                    Factura.cantidadLineas = NumDetalles;
                    Factura.tiponota = Procesos.Buscar_ValorCab("tiponota", i, Fac);
                    Factura.aplicafel = Procesos.Buscar_ValorCab("aplicafel", i, Fac);
                    Factura.tipoOperacion = Procesos.Buscar_ValorCab("tipoOperacion", i, Fac);

                    enviarDocumentoDispape.felPagos pago = new enviarDocumentoDispape.felPagos();
                    pago.moneda = Procesos.Buscar_ValorCab("moneda", i, Fac);
                    pago.totalimportebruto = Convert.ToDouble(Procesos.Buscar_ValorCab("totalimportebruto", i, Fac));
                    pago.totalimportebrutoSpecified = true;
                    pago.totalbaseimponible = Convert.ToDouble(Procesos.Buscar_ValorCab("totalbaseimponible", i, Fac));
                    pago.totalbaseimponibleSpecified = true;
                    pago.totalbaseconimpuestos = Convert.ToDouble(Procesos.Buscar_ValorCab("totalbaseconimpuestos", i, Fac));
                    pago.totalbaseconimpuestosSpecified = true;
                    pago.totalfactura = Convert.ToDouble(Procesos.Buscar_ValorCab("totalfactura", i, Fac));
                    pago.totalfacturaSpecified = true;
                    pago.pagoanticipado = Convert.ToDouble(Procesos.Buscar_ValorCab("pagoanticipado", i, Fac));
                    pago.pagoanticipadoSpecified = true;
                    pago.tipocompra = Convert.ToInt32(Procesos.Buscar_ValorCab("tipocompra", i, Fac));
                    pago.periododepagoa = Convert.ToInt32(Procesos.Buscar_ValorCab("periododepagoa", i, Fac));
                    pago.periododepagoaSpecified = true;
                    pago.fechavencimiento = Convert.ToDateTime(Procesos.Buscar_ValorCab("fechavencimiento", i, Fac));
                    pago.fechavencimientoSpecified = true;
                    pago.totalDescuento = Convert.ToDouble(Procesos.Buscar_ValorCab("totalDescuento", i, Fac));
                    pago.totalDescuentoSpecified = true;
                    Factura.pago = pago;

                    if(Convert.ToDouble(Procesos.Buscar_ValorCab("porcentajeDescuentoCab", i, Fac)) > 0)
                    {
                        enviarDocumentoDispape.felDescuento[] cabDescuentos = new enviarDocumentoDispape.felDescuento[1];
                        enviarDocumentoDispape.felDescuento cabDescuento = new enviarDocumentoDispape.felDescuento();
                        cabDescuento.codigoDescuento = Procesos.Buscar_ValorCab("codigoDescuentoCab", i, Fac);
                        cabDescuento.descuento = Convert.ToDouble(Procesos.Buscar_ValorCab("descuentoCab", i, Fac));
                        cabDescuento.descuentoSpecified = true;
                        cabDescuento.porcentajeDescuento = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentajeDescuentoCab", i, Fac));
                        cabDescuento.porcentajeDescuentoSpecified = true;
                        //cabDescuento.descripcion = Procesos.Buscar_ValorCab("DescripDescuentoCan", i, Fac);

                        cabDescuentos[0] = cabDescuento;
                        Factura.listaDescuentos = cabDescuentos;
                    }


                    i = 0;
                    if (listAdi.Rows.Count > 0)
                    {
                        enviarDocumentoDispape.felCampoAdicional[] camposAdi = new enviarDocumentoDispape.felCampoAdicional[listAdi.Rows.Count];
                        foreach (DataRow _row in listAdi.Rows)
                        {
                            enviarDocumentoDispape.felCampoAdicional campoAdi = new enviarDocumentoDispape.felCampoAdicional();
                            if (Procesos.Buscar_ValorCab("nombreCampo", i, listAdi) == "subtotal")
                            {
                                double valorMoney = 0;
                                valorMoney = double.Parse(Procesos.Buscar_ValorCab("valorCampo", i, listAdi));
                                campoAdi.nombreCampo = Procesos.Buscar_ValorCab("nombreCampo", i, listAdi);
                                campoAdi.valorCampo = string.Format(CultureInfo.InvariantCulture, "{0:N0}", valorMoney);
                            }
                            else
                            {
                                campoAdi.nombreCampo = Procesos.Buscar_ValorCab("nombreCampo", i, listAdi);
                                campoAdi.valorCampo = Procesos.Buscar_ValorCab("valorCampo", i, listAdi);
                            }
                            camposAdi[i] = campoAdi;
                            i++;
                        }
                        Factura.listaCamposAdicionales = camposAdi;
                    }

                    enviarDocumentoDispape.felDetalleDocumento[] Factura_Detalles = new enviarDocumentoDispape.felDetalleDocumento[NumDetalles];
                    i = 0;
                    if (Fac.Rows.Count > 0)
                    {
                        foreach (DataRow _row in Fac.Rows)
                        {
                            enviarDocumentoDispape.felDetalleDocumento LineaDetalle = new enviarDocumentoDispape.felDetalleDocumento();
                            LineaDetalle.codigoproducto = Procesos.Buscar_ValorCab("codigoproducto", i, Fac);
                            LineaDetalle.tipocodigoproducto = Procesos.Buscar_ValorCab("tipocodigoproducto", i, Fac);
                            LineaDetalle.nombreProducto = Procesos.Buscar_ValorCab("nombreProducto", i, Fac);
                            LineaDetalle.descripcion = Procesos.Buscar_ValorCab("descripcionLine", i, Fac);
                            LineaDetalle.referencia = Procesos.Buscar_ValorCab("referencia", i, Fac);
                            LineaDetalle.cantidad = Convert.ToDouble(Procesos.Buscar_ValorCab("cantidad", i, Fac));
                            LineaDetalle.cantidadSpecified = true;
                            LineaDetalle.unidadmedida = Convert.ToString(Procesos.Buscar_ValorCab("unidadmedida", i, Fac));
                            LineaDetalle.valorunitario = Convert.ToDouble(Procesos.Buscar_ValorCab("valorunitario", i, Fac));
                            LineaDetalle.preciosinimpuestos = Convert.ToDouble(Procesos.Buscar_ValorCab("preciosinimpuestos", i, Fac));
                            LineaDetalle.preciototal = Convert.ToDouble(Procesos.Buscar_ValorCab("preciototal", i, Fac));
                            LineaDetalle.tipoImpuesto = Convert.ToInt32(Procesos.Buscar_ValorCab("tipoImpuesto", i, Fac));

                            if(Convert.ToDouble(Procesos.Buscar_ValorCab("porcentajeDescuentoLin", i, Fac)) > 0)
                            {
                                enviarDocumentoDispape.felDescuento[] DetDescuentos = new enviarDocumentoDispape.felDescuento[1];
                                enviarDocumentoDispape.felDescuento DetDescuento = new enviarDocumentoDispape.felDescuento();
                                DetDescuento.codigoDescuento = Procesos.Buscar_ValorCab("codigoDescuentoLin", i, Fac);
                                DetDescuento.descuento = Convert.ToDouble(Procesos.Buscar_ValorCab("descuentoLin", i, Fac));
                                DetDescuento.descuentoSpecified = true;
                                DetDescuento.porcentajeDescuento = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentajeDescuentoLin", i, Fac));
                                DetDescuento.porcentajeDescuentoSpecified = true;
                                //DetDescuento.descripcion = Procesos.Buscar_ValorCab("DescripDescuentoLin", i, Fac);

                                DetDescuentos[0] = DetDescuento;
                                LineaDetalle.listaDescuentos = DetDescuentos;
                            }

                            enviarDocumentoDispape.felImpuesto[] DetImpuestos = new enviarDocumentoDispape.felImpuesto[1];
                            enviarDocumentoDispape.felImpuesto DetImpuesto = new enviarDocumentoDispape.felImpuesto();
                            DetImpuesto.codigoImpuestoRetencion = Procesos.Buscar_ValorCab("codigoImpuestoRetencion", i, Fac);
                            DetImpuesto.porcentaje = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentaje", i, Fac));
                            DetImpuesto.porcentajeSpecified = true;
                            DetImpuesto.valorImpuestoRetencion = Convert.ToDouble(Procesos.Buscar_ValorCab("valorImpuestoRetencion", i, Fac));
                            DetImpuesto.valorImpuestoRetencionSpecified = true;
                            DetImpuesto.baseimponible = Convert.ToDouble(Procesos.Buscar_ValorCab("baseimponible", i, Fac));
                            DetImpuesto.baseimponibleSpecified = true;
                            DetImpuesto.isAutoRetenido = Convert.ToBoolean(Procesos.Buscar_ValorCab("isAutoRetenido", i, Fac));
                            DetImpuesto.isAutoRetenidoSpecified = true;


                            DetImpuestos[0] = DetImpuesto;
                            LineaDetalle.listaImpuestos = DetImpuestos;

                            Factura_Detalles[i] = LineaDetalle;
                            i++;
                        }
                        Factura.listaDetalle = Factura_Detalles;
                    }

                    int NumImpuestos = impFactura.Rows.Count;
                    enviarDocumentoDispape.felImpuesto[] Factura_Impuestos = new enviarDocumentoDispape.felImpuesto[NumImpuestos];

                    i = 0;
                    if (impFactura.Rows.Count > 0)
                    {
                        foreach (DataRow _row in impFactura.Rows)
                        {
                            enviarDocumentoDispape.felImpuesto LineaImpuestos = new enviarDocumentoDispape.felImpuesto();
                            LineaImpuestos.codigoImpuestoRetencion = Procesos.Buscar_ValorCab("codigoImpuestoRetencion", i, impFactura);
                            LineaImpuestos.porcentaje = Convert.ToDouble(Procesos.Buscar_ValorCab("porcentaje", i, impFactura));
                            LineaImpuestos.porcentajeSpecified = true;
                            LineaImpuestos.valorImpuestoRetencion = Convert.ToDouble(Procesos.Buscar_ValorCab("valorImpuestoRetencion", i, impFactura));
                            LineaImpuestos.valorImpuestoRetencionSpecified = true;
                            LineaImpuestos.baseimponible = Convert.ToDouble(Procesos.Buscar_ValorCab("baseimponible", i, impFactura));
                            LineaImpuestos.baseimponibleSpecified = true;
                            LineaImpuestos.isAutoRetenido = Convert.ToBoolean(Procesos.Buscar_ValorCab("isAutoRetenido", i, impFactura));
                            LineaImpuestos.isAutoRetenidoSpecified = true;
                            Factura_Impuestos[i] = LineaImpuestos;
                            i++;
                        }
                        Factura.listaImpuestos = Factura_Impuestos;
                    }

                    i = 0;
                    int docBase = 1;
                    if (impFactura.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Procesos.Buscar_ValorCab("consecutivofacturamodificada", i, Fac)))
                        {
                            enviarDocumentoDispape.felFacturaModificada[] Notas_DocBase = new enviarDocumentoDispape.felFacturaModificada[docBase];
                            enviarDocumentoDispape.felFacturaModificada LineadocBase = new enviarDocumentoDispape.felFacturaModificada();

                            LineadocBase.consecutivoFacturaModificada = Procesos.Buscar_ValorCab("consecutivofacturamodificada", i, Fac);
                            LineadocBase.prefijoFacturaModificada = Procesos.Buscar_ValorCab("prefijoFacturaModificada", i, Fac);
                            LineadocBase.tipoDocumentoFacturaModificada = Procesos.Buscar_ValorCab("tipoDocumentoFacturaModificada", i, Fac);
                            LineadocBase.cufeFacturaModificada = Procesos.Buscar_ValorCab("cufefacturamodificada", i, Fac);                            
                            if(!string.IsNullOrEmpty(Procesos.Buscar_ValorCab("fechafacturamodificada", i, Fac)))
                            {
                                LineadocBase.fechaFacturaModificadaSpecified = true;
                                LineadocBase.fechaFacturaModificada = DateTime.Parse(Procesos.Buscar_ValorCab("fechafacturamodificada", i, Fac));
                            }
                            
                            Notas_DocBase[i] = LineadocBase;
                            Factura.listaFacturasModificadas = Notas_DocBase;
                        }
                    }

                    i = 0;

                    enviarDocumentoDispape.felAdquirente[] adquirentes = new enviarDocumentoDispape.felAdquirente[1];
                    enviarDocumentoDispape.felAdquirente adquirente = new enviarDocumentoDispape.felAdquirente();

                    adquirente.tipoPersona = Procesos.Buscar_ValorCab("tipopersona", i, Fac);
                    adquirente.nombreCompleto = Procesos.Buscar_ValorCab("nombreCompleto", i, Fac);
                    adquirente.tipoIdentificacion = Convert.ToInt32(Procesos.Buscar_ValorCab("tipoidentificacion", i, Fac));
                    adquirente.numeroIdentificacion = Procesos.Buscar_ValorCab("numeroidentificacion", i, Fac);
                    adquirente.digitoverificacion = Procesos.Buscar_ValorCab("digitoverificacion", i, Fac);
                    adquirente.regimen = Procesos.Buscar_ValorCab("regimen", i, Fac);
                    adquirente.email = Procesos.Buscar_ValorCab("email", i, Fac);
                    adquirente.pais = Procesos.Buscar_ValorCab("pais", i, Fac);
                    adquirente.paisnombre = Procesos.Buscar_ValorCab("paisnombre", i, Fac);
                    adquirente.departamento = Procesos.Buscar_ValorCab("departamento", i, Fac);
                    adquirente.nombredepartamento = Procesos.Buscar_ValorCab("nombredepartamento", i, Fac);
                    adquirente.ciudad = Procesos.Buscar_ValorCab("codigoCiudad", i, Fac);
                    adquirente.descripcionCiudad = Procesos.Buscar_ValorCab("descripcionCiudad", i, Fac);
                    adquirente.direccion = Procesos.Buscar_ValorCab("direccion", i, Fac);
                    adquirente.telefono = Procesos.Buscar_ValorCab("telefono", i, Fac);
                    adquirente.envioPorEmailPlataforma = Procesos.Buscar_ValorCab("envioPorEmailPlataforma", i, Fac);
                    adquirente.tipoobligacion = Procesos.Buscar_ValorCab("tipoobligacion", i, Fac);

                    adquirentes[i] = adquirente;
                    Factura.listaAdquirentes = adquirentes;


                    enviarDocumentoDispape.felDatoEntrega[] entregas = new enviarDocumentoDispape.felDatoEntrega[1];
                    enviarDocumentoDispape.felDatoEntrega entrega = new enviarDocumentoDispape.felDatoEntrega();
                    entrega.direccionEntrega = Procesos.Buscar_ValorCab("direccionEntrega", i, Fac);
                    entrega.telefonoEntrega = Procesos.Buscar_ValorCab("telefonoEntrega", i, Fac);
                    entrega.paisEntrega = Procesos.Buscar_ValorCab("paisEntrega", i, Fac);
                    entregas[i] = entrega;
                    Factura.listaDatosEntrega = entregas;


                    enviarDocumentoDispape.felMedioPago[] mediosPago = new enviarDocumentoDispape.felMedioPago[1];
                    enviarDocumentoDispape.felMedioPago medioPago = new enviarDocumentoDispape.felMedioPago();
                    medioPago.medioPago = Procesos.Buscar_ValorCab("medioPago", i, Fac);
                    mediosPago[i] = medioPago;
                    Factura.listaMediosPagos = mediosPago;


                    Procesos.EscribirLogFileTXT("Consumo: Inicio");
                    respuesta = clienteServicio.enviarDocumento(Factura);
                    Procesos.EscribirLogFileTXT("Consumo: Fin");
                }
                var serxml = new System.Xml.Serialization.XmlSerializer(Factura.GetType());
                var ms = new MemoryStream();
                serxml.Serialize(ms, Factura);
                string xml = Encoding.UTF8.GetString(ms.ToArray());
                Procesos.requestSend = xml;

                clienteServicio.Close();
                return respuesta;
            }
            catch (Exception ex)
            {
                enviarDocumentoDispape.felRespuestaEnvio response = null;
                Procesos.EscribirLogFileTXT("SendDispapeles: " + ex.Message);
                return response;
            }
        }

        public static consultarArchivosDispape.felRepuestaDescargaDocumentos consultaArchivos(string numDoc, string prefijo, string tipoDoc)
        {
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                Procesos.EscribirLogFileTXT("ConsultaPDF: Inicio");

                consultarArchivosDispape.felConsultaFacturaArchivo request = new consultarArchivosDispape.felConsultaFacturaArchivo();
                consultarArchivosDispape.felRepuestaDescargaDocumentos response = new consultarArchivosDispape.felRepuestaDescargaDocumentos();
                consultarArchivosDispape.consultarArchivos consultar = new consultarArchivosDispape.consultarArchivos(); ;

                request.idEmpresaSpecified = true;
                request.idEmpresa = Convert.ToInt32(Procesos.idEmpresa);
                request.usuario = Procesos.username;
                request.contrasenia = Procesos.password;
                request.token = Procesos.token;
                //request.version = "6";
                switch (tipoDoc)
                {
                    case "01":
                        request.tipoDocumento = "1";
                        break;

                    case "03":
                        request.tipoDocumento = "5";
                        break;

                    case "91":
                        request.tipoDocumento = "2";
                        break;

                    case "92":
                        request.tipoDocumento = "3";
                        break;
                }

                request.prefijo = prefijo;
                request.consecutivoSpecified = true;
                request.consecutivo = Convert.ToInt32(numDoc);
                request.tipoArchivo = 0;

                response = consultar.CallconsultarArchivos(request);

                var serxml = new System.Xml.Serialization.XmlSerializer(request.GetType());
                var ms = new MemoryStream();
                serxml.Serialize(ms, request);
                string xml = Encoding.UTF8.GetString(ms.ToArray());
                //Procesos.requestSend = xml;

                Procesos.EscribirLogFileTXT("ConsultaArchvios: n/" + xml);
                return response;
            }
            catch (Exception ex)
            {
                consultarArchivosDispape.felRepuestaDescargaDocumentos response = null;
                Procesos.EscribirLogFileTXT("PDFDispapeles: " + ex.Message);
                return response;
            }
        }

        //public static consultarArchivosDispape.felRepuestaDescargaDocumentos consultaEstados(string numDoc, string prefijo, string tipoDoc, string wsURL)
        //{
        //    DateTime _createdDate;
        //    _createdDate = DateTime.Now;
        //    Procesos.dateSend = _createdDate;
        //    try
        //    {
        //        Procesos.EscribirLogFileTXT("ConsultaXML: Inicio");
        //        string urlServicio;
        //        urlServicio = wsURL;

        //        ConsultarArchivosDispape.ebFelConsultaFacturaWS consultaPDF = new ConsultarArchivosDispape.ebFelConsultaFacturaWS();
        //        ConsultarArchivosDispape.InterSoapClient clienteServicio;
        //        ConsultarArchivosDispape.documentoElectronicoWsDto response;

        //        clienteServicio = new ConsultarArchivosDispape.InterSoapClient(ObtenerBindingsHttps(), new EndpointAddress(urlServicio));
        //        using (new OperationContextScope(clienteServicio.InnerChannel))
        //        {
        //            //Add SOAP Header (Header property in the envelope) to an outgoing request.

        //            HttpRequestMessageProperty requestMessage = new HttpRequestMessageProperty();
        //            requestMessage.Headers["username"] = Procesos.username;
        //            requestMessage.Headers["password"] = Procesos.password;
        //            requestMessage.Headers["token"] = Procesos.token;


        //            OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = requestMessage;

        //            consultaPDF.consecutivo = numDoc;
        //            consultaPDF.fechafacturacionString = fechaFac.ToString("yyyyMMdd");
        //            consultaPDF.prefijo = prefijo;
        //            consultaPDF.tipodocumento = tipoDoc;
        //            consultaPDF.tokenempresa = Procesos.token;

        //            response = clienteServicio.consultarXmlFactura(consultaPDF);

        //        }
        //        //var serxml = new System.Xml.Serialization.XmlSerializer(Factura.GetType());
        //        //var ms = new MemoryStream();
        //        //serxml.Serialize(ms, Factura);
        //        //string xml = Encoding.UTF8.GetString(ms.ToArray());
        //        //Procesos.requestSend = xml;

        //        clienteServicio.Close();
        //        Procesos.EscribirLogFileTXT("ConsultaXML: Fin");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        ConsultarArchivosDispape.documentoElectronicoWsDto response = null;
        //        Procesos.EscribirLogFileTXT("XMLDispapeles: " + ex.Message);
        //        return response;
        //    }
        //}
    }
}