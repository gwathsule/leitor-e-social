﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace WebServices.WsEnviarRestrira {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    // CODEGEN: The optional WSDL extension element 'PolicyReference' from namespace 'http://schemas.xmlsoap.org/ws/2004/09/policy' was not handled.
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WsEnviarLoteEventos", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0")]
    public partial class ServicoEnviarLoteEventos : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback EnviarLoteEventosOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ServicoEnviarLoteEventos() {
            this.Url = global::WebServices.Properties.Settings.Default.WebServices_WsEnviarRestrira_ServicoEnviarLoteEventos;
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
        public event EnviarLoteEventosCompletedEventHandler EnviarLoteEventosCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0/ServicoEn" +
            "viarLoteEventos/EnviarLoteEventos", RequestNamespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0", ResponseNamespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/v1_1_0", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public System.Xml.XmlElement EnviarLoteEventos(System.Xml.XmlElement loteEventos) {
            object[] results = this.Invoke("EnviarLoteEventos", new object[] {
                        loteEventos});
            return ((System.Xml.XmlElement)(results[0]));
        }
        
        /// <remarks/>
        public void EnviarLoteEventosAsync(System.Xml.XmlElement loteEventos) {
            this.EnviarLoteEventosAsync(loteEventos, null);
        }
        
        /// <remarks/>
        public void EnviarLoteEventosAsync(System.Xml.XmlElement loteEventos, object userState) {
            if ((this.EnviarLoteEventosOperationCompleted == null)) {
                this.EnviarLoteEventosOperationCompleted = new System.Threading.SendOrPostCallback(this.OnEnviarLoteEventosOperationCompleted);
            }
            this.InvokeAsync("EnviarLoteEventos", new object[] {
                        loteEventos}, this.EnviarLoteEventosOperationCompleted, userState);
        }
        
        private void OnEnviarLoteEventosOperationCompleted(object arg) {
            if ((this.EnviarLoteEventosCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.EnviarLoteEventosCompleted(this, new EnviarLoteEventosCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void EnviarLoteEventosCompletedEventHandler(object sender, EnviarLoteEventosCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class EnviarLoteEventosCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal EnviarLoteEventosCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public System.Xml.XmlElement Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((System.Xml.XmlElement)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591