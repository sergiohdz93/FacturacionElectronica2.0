﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace AddOn_FE_DIAN.consultarArchivosDispape {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="consultarArchivosSoapBinding", Namespace="http://wsconsultarpdfxml.webservice.dispapeles.com/")]
    public partial class consultarArchivos : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback CallconsultarArchivosOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public consultarArchivos() {
            this.Url = global::AddOn_FE_DIAN.Properties.Settings.Default.AddOn_FE_DIAN_consultarArchivosDispape_consultarArchivos;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event CallconsultarArchivosCompletedEventHandler CallconsultarArchivosCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestElementName="consultarArchivos", RequestNamespace="http://wsconsultarpdfxml.webservice.dispapeles.com/", ResponseElementName="consultarArchivosResponse", ResponseNamespace="http://wsconsultarpdfxml.webservice.dispapeles.com/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("return", Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public felRepuestaDescargaDocumentos CallconsultarArchivos([System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)] felConsultaFacturaArchivo Fel_ConsultaFacturaArchivo) {
            object[] results = this.Invoke("CallconsultarArchivos", new object[] {
                        Fel_ConsultaFacturaArchivo});
            return ((felRepuestaDescargaDocumentos)(results[0]));
        }
        
        /// <remarks/>
        public void CallconsultarArchivosAsync(felConsultaFacturaArchivo Fel_ConsultaFacturaArchivo) {
            this.CallconsultarArchivosAsync(Fel_ConsultaFacturaArchivo, null);
        }
        
        /// <remarks/>
        public void CallconsultarArchivosAsync(felConsultaFacturaArchivo Fel_ConsultaFacturaArchivo, object userState) {
            if ((this.CallconsultarArchivosOperationCompleted == null)) {
                this.CallconsultarArchivosOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCallconsultarArchivosOperationCompleted);
            }
            this.InvokeAsync("CallconsultarArchivos", new object[] {
                        Fel_ConsultaFacturaArchivo}, this.CallconsultarArchivosOperationCompleted, userState);
        }
        
        private void OnCallconsultarArchivosOperationCompleted(object arg) {
            if ((this.CallconsultarArchivosCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CallconsultarArchivosCompleted(this, new CallconsultarArchivosCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://wsconsultarpdfxml.webservice.dispapeles.com/")]
    public partial class felConsultaFacturaArchivo {
        
        private long consecutivoField;
        
        private bool consecutivoFieldSpecified;
        
        private string contraseniaField;
        
        private long idEmpresaField;
        
        private bool idEmpresaFieldSpecified;
        
        private string prefijoField;
        
        private int tipoArchivoField;
        
        private string tipoDocumentoField;
        
        private string tokenField;
        
        private string usuarioField;
        
        private string versionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long consecutivo {
            get {
                return this.consecutivoField;
            }
            set {
                this.consecutivoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool consecutivoSpecified {
            get {
                return this.consecutivoFieldSpecified;
            }
            set {
                this.consecutivoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string contrasenia {
            get {
                return this.contraseniaField;
            }
            set {
                this.contraseniaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long idEmpresa {
            get {
                return this.idEmpresaField;
            }
            set {
                this.idEmpresaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idEmpresaSpecified {
            get {
                return this.idEmpresaFieldSpecified;
            }
            set {
                this.idEmpresaFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prefijo {
            get {
                return this.prefijoField;
            }
            set {
                this.prefijoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int tipoArchivo {
            get {
                return this.tipoArchivoField;
            }
            set {
                this.tipoArchivoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tipoDocumento {
            get {
                return this.tipoDocumentoField;
            }
            set {
                this.tipoDocumentoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string version {
            get {
                return this.versionField;
            }
            set {
                this.versionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://wsconsultarpdfxml.webservice.dispapeles.com/")]
    public partial class felMensajesProceso {
        
        private string codigoMensajeField;
        
        private string descripcionMensajeField;
        
        private string rechazoNotificacionField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codigoMensaje {
            get {
                return this.codigoMensajeField;
            }
            set {
                this.codigoMensajeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string descripcionMensaje {
            get {
                return this.descripcionMensajeField;
            }
            set {
                this.descripcionMensajeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string rechazoNotificacion {
            get {
                return this.rechazoNotificacionField;
            }
            set {
                this.rechazoNotificacionField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://wsconsultarpdfxml.webservice.dispapeles.com/")]
    public partial class felArchivos {
        
        private string formatoField;
        
        private string mimeTypeField;
        
        private string nameFileField;
        
        private byte[] streamFileField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string formato {
            get {
                return this.formatoField;
            }
            set {
                this.formatoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string mimeType {
            get {
                return this.mimeTypeField;
            }
            set {
                this.mimeTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nameFile {
            get {
                return this.nameFileField;
            }
            set {
                this.nameFileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="base64Binary")]
        public byte[] streamFile {
            get {
                return this.streamFileField;
            }
            set {
                this.streamFileField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://wsconsultarpdfxml.webservice.dispapeles.com/")]
    public partial class felRepuestaDescargaDocumentos {
        
        private int codigoRespuestaField;
        
        private long consecutivoField;
        
        private bool consecutivoFieldSpecified;
        
        private string descripcionRespuestaField;
        
        private int estadoProcesoField;
        
        private string idErpField;
        
        private felArchivos[] listaArchivosField;
        
        private felMensajesProceso[] listaMensajesProcesoField;
        
        private string prefijoField;
        
        private string tipoDocumentoField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int codigoRespuesta {
            get {
                return this.codigoRespuestaField;
            }
            set {
                this.codigoRespuestaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public long consecutivo {
            get {
                return this.consecutivoField;
            }
            set {
                this.consecutivoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool consecutivoSpecified {
            get {
                return this.consecutivoFieldSpecified;
            }
            set {
                this.consecutivoFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string descripcionRespuesta {
            get {
                return this.descripcionRespuestaField;
            }
            set {
                this.descripcionRespuestaField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int estadoProceso {
            get {
                return this.estadoProcesoField;
            }
            set {
                this.estadoProcesoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string idErp {
            get {
                return this.idErpField;
            }
            set {
                this.idErpField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("listaArchivos", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public felArchivos[] listaArchivos {
            get {
                return this.listaArchivosField;
            }
            set {
                this.listaArchivosField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("listaMensajesProceso", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=true)]
        public felMensajesProceso[] listaMensajesProceso {
            get {
                return this.listaMensajesProcesoField;
            }
            set {
                this.listaMensajesProcesoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string prefijo {
            get {
                return this.prefijoField;
            }
            set {
                this.prefijoField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tipoDocumento {
            get {
                return this.tipoDocumentoField;
            }
            set {
                this.tipoDocumentoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void CallconsultarArchivosCompletedEventHandler(object sender, CallconsultarArchivosCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CallconsultarArchivosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal CallconsultarArchivosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public felRepuestaDescargaDocumentos Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((felRepuestaDescargaDocumentos)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591