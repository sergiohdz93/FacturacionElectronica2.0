using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace AddOn_FE_DIAN.Conexus
{
    class MetodosConexus
    {
        public static ResponseType setDocumento(DataTable Fac, DataTable impFactura)
        {
            Procesos.responseStatus = "";
            DateTime _createdDate;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            try
            {
                int i = 0;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ServiceClient client = new ServiceClient();
                DocumentType oDocument = new DocumentType();
                ResponseType oResponse = new ResponseType();

                SoftwareSeguridadType oSeguridad = new SoftwareSeguridadType();
                oSeguridad.TipoDocumento = Procesos.Buscar_ValorCab("TipoDocumento", i, Fac);
                oSeguridad.GuidEmpresa = Procesos.Buscar_ValorCab("GuidEmpresa", i, Fac);
                oSeguridad.GuidOrigen = Procesos.Buscar_ValorCab("GuidOrigen", i, Fac);
                oSeguridad.HashSeguridad = Procesos.Buscar_ValorCab("HashSeguridad", i, Fac);
                oSeguridad.ClaveTecnica = Procesos.Buscar_ValorCab("ClaveTecnica", i, Fac);
                oSeguridad.CodigoErp = Procesos.Buscar_ValorCab("CodigoErp", i, Fac);
                oSeguridad.NumeroDocumento = Procesos.Buscar_ValorCab("NumeroDocumento", i, Fac);
                oDocument.SoftwareSeguridad = oSeguridad;

                EmisorType oEmisor = new EmisorType();
                oEmisor.EmiTipoPersona = Convert.ToInt32(Procesos.Buscar_ValorCab("EmiTipoPersona", i, Fac));
                oEmisor.EmiTipoIdentificacion = Convert.ToInt32(Procesos.Buscar_ValorCab("EmiTipoIdentificacion", i, Fac));
                oEmisor.EmiIdentificacion = Procesos.Buscar_ValorCab("EmiIdentificacion", i, Fac);
                oEmisor.EmiDVIdentificacion = Procesos.Buscar_ValorCab("EmiDVIdentificacion", i, Fac);
                oDocument.EmisorData = oEmisor;

                CompradorType oComprador = new CompradorType();
                oComprador.CompradorTipoPersona = Procesos.Buscar_ValorCab("CompradorTipoPersona", i, Fac);
                oComprador.CompradorTipoIdentificacion = Procesos.Buscar_ValorCab("CompradorTipoIdentificacion", i, Fac);
                oComprador.CompradorIdentificacion = Procesos.Buscar_ValorCab("CompradorIdentificacion", i, Fac);
                oComprador.CompradorDVIdentificacion = Procesos.Buscar_ValorCab("CompradorDVIdentificacion", i, Fac);
                oComprador.CompradorTipoRegimen = Procesos.Buscar_ValorCab("CompradorTipoRegimen", i, Fac);
                oComprador.CompradorRespFiscal = Procesos.Buscar_ValorCab("CompradorRespFiscal", i, Fac);
                oComprador.CompradorImpuesto = Procesos.Buscar_ValorCab("CompradorImpuesto", i, Fac);
                oComprador.CompradorRazonSocial = Procesos.Buscar_ValorCab("CompradorRazonSocial", i, Fac);
                oComprador.CompradorPrimerNombre = Procesos.Buscar_ValorCab("CompradorPrimerNombre", i, Fac);
                oComprador.CompradorSegundoNombre = Procesos.Buscar_ValorCab("CompradorSegundoNombre", i, Fac);
                oComprador.CompradorApellidos = Procesos.Buscar_ValorCab("CompradorApellidos", i, Fac);
                oComprador.CompradorNombreCompleto = Procesos.Buscar_ValorCab("CompradorNombreCompleto", i, Fac);
                oComprador.CompradorPais = Procesos.Buscar_ValorCab("CompradorPais", i, Fac);
                oComprador.CompradorNombrePais = Procesos.Buscar_ValorCab("CompradorNombrePais", i, Fac);
                oComprador.CompradorDepartamento = Procesos.Buscar_ValorCab("CompradorDepartamento", i, Fac);
                oComprador.CompradorCodDepartamento = Procesos.Buscar_ValorCab("CompradorCodDepartamento", i, Fac);
                oComprador.CompradorCiudad = Procesos.Buscar_ValorCab("CompradorCiudad", i, Fac);
                oComprador.CompradorCodCiudad = Procesos.Buscar_ValorCab("CompradorCodCiudad", i, Fac);
                oComprador.CompradorCodPostal = Procesos.Buscar_ValorCab("CompradorCodPostal", i, Fac);
                oComprador.CompradorDireccion = Procesos.Buscar_ValorCab("CompradorDireccion", i, Fac);
                oComprador.CompradorEnviarCorreo = Convert.ToBoolean(Procesos.Buscar_ValorCab("CompradorEnviarCorreo", i, Fac));
                oComprador.CompradorTelefonoCont = Procesos.Buscar_ValorCab("CompradorTelefonoCont", i, Fac);
                oComprador.CompradorCorreoElectronico = Procesos.Buscar_ValorCab("CompradorCorreoElectronico", i, Fac);
                oDocument.CompradorFactura = oComprador;

                EncabezadoType oEncabezado = new EncabezadoType();
                oEncabezado.FacTipoFactura = Procesos.Buscar_ValorCab("FacTipoFactura", i, Fac);
                oEncabezado.FacCodOperacion = Procesos.Buscar_ValorCab("FacCodOperacion", i, Fac);
                oEncabezado.FacFechaHoraFactura = Procesos.Buscar_ValorCab("FacFechaHoraFactura", i, Fac);
                oEncabezado.FacFechaIni = Procesos.Buscar_ValorCab("FacFechaIni", i, Fac);
                oEncabezado.FacFechaFin = Procesos.Buscar_ValorCab("FacFechaFin", i, Fac);
                oEncabezado.FacRefContigencia = Procesos.Buscar_ValorCab("FacRefContigencia", i, Fac);
                oEncabezado.FacTipoRefContigencia = Procesos.Buscar_ValorCab("FacTipoRefContigencia", i, Fac);
                oEncabezado.FacFechaContingencia = Procesos.Buscar_ValorCab("FacFechaContingencia", i, Fac);
                oDocument.EncabezadoData = oEncabezado;

                InfoMonetarioType oInfoMoneda = new InfoMonetarioType();
                oInfoMoneda.FacCodMoneda = Procesos.Buscar_ValorCab("FacCodMoneda", i, Fac);
                oInfoMoneda.FacTotalImporteBruto = Procesos.Buscar_ValorCab("FacTotalImporteBruto", i, Fac);
                oInfoMoneda.FacTotalCargos = Procesos.Buscar_ValorCab("FacTotalCargos", i, Fac);
                oInfoMoneda.FacTotalDescuentos = Procesos.Buscar_ValorCab("FacTotalDescuentos", i, Fac);
                oInfoMoneda.FacTotalBaseImponible = Procesos.Buscar_ValorCab("FacTotalBaseImponible", i, Fac);
                oInfoMoneda.FacTotalBrutoMasImp = Procesos.Buscar_ValorCab("FacTotalBrutoMasImp", i, Fac);
                oInfoMoneda.FacTotalAnticipos = Procesos.Buscar_ValorCab("FacTotalAnticipos", i, Fac);
                oInfoMoneda.FacTotalFactura = Procesos.Buscar_ValorCab("FacTotalFactura", i, Fac);
                oDocument.InfoMonetarioData = oInfoMoneda;

                FormaPagoType oFormaPago = new FormaPagoType();
                oDocument.lsFormaPago = new FormaPagoType[1];
                oFormaPago.FacMetodoPago = Procesos.Buscar_ValorCab("FacMetodoPago", i, Fac);
                oFormaPago.FacFormaPago = Procesos.Buscar_ValorCab("FacFormaPago", i, Fac);
                oFormaPago.FacVencimientoFac = Procesos.Buscar_ValorCab("FacVencimientoFac", i, Fac);
                oDocument.lsFormaPago[0] = oFormaPago;


                i = 0;
                if (impFactura.Rows.Count > 0)
                {
                    foreach (DataRow _row in impFactura.Rows)
                    {
                        ImpuestoType oImpuestos = new ImpuestoType();
                        oDocument.LsImpuestos = new ImpuestoType[impFactura.Rows.Count];
                        oImpuestos.CodigoImpuesto = Procesos.Buscar_ValorCab("CodigoImpuesto", i, impFactura);
                        oImpuestos.NombreImpuesto = Procesos.Buscar_ValorCab("NombreImpuesto", i, impFactura);
                        oImpuestos.EsRetencionImpuesto = Convert.ToBoolean(Procesos.Buscar_ValorCab("EsRetencionImpuesto", i, impFactura));
                        oImpuestos.BaseImponible = Procesos.Buscar_ValorCab("BaseImponible", i, impFactura);
                        oImpuestos.Porcentaje = Procesos.Buscar_ValorCab("Porcentaje", i, impFactura);
                        oImpuestos.ValorImpuesto = Procesos.Buscar_ValorCab("ValorImpuesto", i, impFactura);
                        oDocument.LsImpuestos[i] = oImpuestos;
                        i++;
                    }
                }

                i = 0;
                if (Fac.Rows.Count > 0)
                {
                    foreach (DataRow _row in Fac.Rows)
                    {
                        DetalleType oDetalle = new DetalleType();
                        oDocument.LsDetalle = new DetalleType[Fac.Rows.Count];
                        oDetalle.DetFacConsecutivo = Convert.ToInt32(Procesos.Buscar_ValorCab("DetFacConsecutivo", i, Fac));
                        oDetalle.Codigo = Procesos.Buscar_ValorCab("Codigo", i, Fac);
                        oDetalle.CodigoEstandar = Procesos.Buscar_ValorCab("CodigoEstandar", i, Fac);
                        oDetalle.Codificacion = Procesos.Buscar_ValorCab("Codificacion", i, Fac);
                        oDetalle.Descripcion = Procesos.Buscar_ValorCab("Descripcion", i, Fac);
                        oDetalle.Cantidad = Procesos.Buscar_ValorCab("Cantidad", i, Fac);
                        oDetalle.UnidadMedida = Procesos.Buscar_ValorCab("UnidadMedida", i, Fac);
                        oDetalle.PrecioUnitario = Procesos.Buscar_ValorCab("PrecioUnitario", i, Fac);
                        oDetalle.PrecioSinImpuestos = Procesos.Buscar_ValorCab("PrecioSinImpuestos", i, Fac);
                        oDetalle.PrecioTotal = Procesos.Buscar_ValorCab("PrecioTotal", i, Fac);
                        oDocument.LsDetalle[i] = oDetalle;
                        i++;
                    }
                }

                i = 0;
                if (Fac.Rows.Count > 0)
                {
                    foreach (DataRow _row in Fac.Rows)
                    {
                        ImpuestoDetalleType oImpuDetalle = new ImpuestoDetalleType();
                        oDocument.LsDetalleImpuesto = new ImpuestoDetalleType[Fac.Rows.Count];
                        oImpuDetalle.Secuencia = Convert.ToInt32(Procesos.Buscar_ValorCab("Secuencia", i, Fac));
                        oImpuDetalle.CodigoImpuesto = Procesos.Buscar_ValorCab("CodigoImpuesto", i, Fac);
                        oImpuDetalle.NombreImpuesto = Procesos.Buscar_ValorCab("NombreImpuesto", i, Fac);
                        oImpuDetalle.EsRetencionImpuesto = Convert.ToBoolean(Procesos.Buscar_ValorCab("EsRetencionImpuesto", i, Fac));
                        oImpuDetalle.BaseImponible = Procesos.Buscar_ValorCab("BaseImponible", i, Fac);
                        oImpuDetalle.Porcentaje = Procesos.Buscar_ValorCab("Porcentaje", i, Fac);
                        oImpuDetalle.ValorImpuesto = Procesos.Buscar_ValorCab("ValorImpuesto", i, Fac);
                        oDocument.LsDetalleImpuesto[i] = oImpuDetalle;
                        i++;
                    }
                }

                i = 0;

                AutorizacionType oAutorizacion = new AutorizacionType();
                oAutorizacion.AutFechaInicio = Procesos.Buscar_ValorCab("AutFechaInicio", i, Fac);
                oAutorizacion.AutFechaFinal = Procesos.Buscar_ValorCab("AutFechaFinal", i, Fac);
                oAutorizacion.AutNumAutorizacion = Procesos.Buscar_ValorCab("AutNumAutorizacion", i, Fac);
                oAutorizacion.AutPrefijo = Procesos.Buscar_ValorCab("AutPrefijo", i, Fac);
                oAutorizacion.AutSecuenciaInicio = Procesos.Buscar_ValorCab("AutSecuenciaInicio", i, Fac);
                oAutorizacion.AutSecuenciaFinal = Procesos.Buscar_ValorCab("AutSecuenciaFinal", i, Fac);
                oDocument.AutorizacionFactura = oAutorizacion;

                var serxml = new System.Xml.Serialization.XmlSerializer(oDocument.GetType());
                var ms = new MemoryStream();
                serxml.Serialize(ms, oDocument);
                string xml = Encoding.UTF8.GetString(ms.ToArray());
                Procesos.requestSend = xml;
                Procesos.EscribirLogFileTXT(xml);

                oResponse = client.SetDocument(oDocument);

                client.Close();

                return oResponse;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("setDocumento: " + ex.Message);
                return null;
            }
        }

        public static void getTransaccionbyId(string idLog)
        {
            Procesos.responseStatus = "";
            try
            {
                string documento, fecha, request;
                SAPbobsCOM.UserTables tbls = null;
                SAPbobsCOM.UserTable tbl = null;

                tbls = Procesos.oCompany.UserTables;
                tbl = tbls.Item("FEDIAN_MONITORLOG");

                tbl.GetByKey(idLog.ToString());

                request = tbl.UserFields.Fields.Item("U_Det_Peticion").Value;
                documento = tbl.UserFields.Fields.Item("U_Prefijo").Value + tbl.UserFields.Fields.Item("U_Folio").Value;
                fecha = (tbl.UserFields.Fields.Item("U_Fecha_Envio").Value).ToString("yyyymm");

                ServiceClient Cl = new ServiceClient();
                var R = Cl.GetTransaccionbyIdentificacion(documento, fecha);

                Procesos.UpdateLogConexus(idLog, R, request);

                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbls);
                tbls = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(tbl);
                tbl = null;
                GC.Collect();
            }
            catch(Exception ex)
            {
                Procesos.EscribirLogFileTXT("getTransaccionbyId: " + ex.Message);
            }
        }

        public static ResponseType getTransaccion(string CUFE, string fecha)
        {
            Procesos.responseStatus = "";
            try
            {
                ServiceClient Cl = new ServiceClient();
                var R = Cl.GetTransaccion(CUFE, fecha);
                return R;
            }
            catch (Exception ex)
            {
                Procesos.EscribirLogFileTXT("getTransaccion: " + ex.Message);
                return null;
            }
        }
    }
}
