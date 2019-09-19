using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AddOn_FE_DIAN
{
    public class Security : MessageHeader
    {
        private readonly string _password, _username, _nonce;
        private readonly DateTime _createdDate;

        public Security(string id, string username, string password, string nonce)
        {
            _password = password;
            _username = username;
            _nonce = nonce;
            _createdDate = DateTime.Now;
            Procesos.dateSend = _createdDate;
            this.Id = id;
        }

        public string Id { get; set; }

        public override string Name
        {
            get { return "Security"; }
        }

        public override string Namespace
        {
            get { return "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd"; }
        }

        protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", Name, Namespace);
            //writer.WriteXmlnsAttribute("wsse", Namespace);
            writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
        }

        protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        {
            writer.WriteStartElement("wsse", "UsernameToken", Namespace);
            writer.WriteXmlnsAttribute("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

            //writer.WriteStartElement("wsse", "UsernameToken", Namespace);
            //writer.WriteAttributeString("Id", "UsernameToken-10");

            //writer.WriteStartElement("wsse", "Username", Namespace);
            writer.WriteStartElement("wsse", "Username", null);
            writer.WriteValue(_username);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Password", Namespace);
            writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
            writer.WriteValue(_password);
            writer.WriteEndElement();

            writer.WriteStartElement("wsse", "Nonce", Namespace);
            writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            writer.WriteValue(_nonce);
            writer.WriteEndElement();

            writer.WriteStartElement("wsu", "Created", null);
            writer.WriteString(_createdDate.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        //private readonly string _password, _username, _nonce;
        //private readonly DateTime _createdDate;

        //public Security(string id, string username, string password, string nonce)
        //{
        //    _password = password;
        //    _username = username;
        //    _nonce = nonce;
        //    _createdDate = DateTime.Now;
        //    this.Id = id;
        //}

        //public string Id { get; set; }

        //public override string Name => "Security";

        //public override string Namespace => "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";

        //protected override void OnWriteStartHeader(XmlDictionaryWriter writer, MessageVersion messageVersion)
        //{
        //    writer.WriteStartElement("wsse", Name);
        //    writer.WriteXmlnsAttribute("wsse", Namespace);
        //}

        //protected override void OnWriteHeaderContents(XmlDictionaryWriter writer, MessageVersion messageVersion)
        //{
        //    writer.WriteStartElement("wsse", "UsernameToken", Namespace);
        //    writer.WriteAttributeString("wsu", "Id", "UsernameToken-10");
        //    //writer.WriteAttributeString("wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

        //    writer.WriteStartElement("wsse", "Username", Namespace);
        //    writer.WriteValue(_username);
        //    writer.WriteEndElement();

        //    writer.WriteStartElement("wsse", "Password", Namespace);
        //    writer.WriteAttributeString("Type", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText");
        //    writer.WriteValue(_password);
        //    writer.WriteEndElement();

        //    writer.WriteStartElement("wsse", "Nonce", Namespace);
        //    writer.WriteAttributeString("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        //    writer.WriteValue(_nonce);
        //    writer.WriteEndElement();

        //    writer.WriteStartElement("wsu", "Created", Namespace);
        //    writer.WriteValue(_createdDate.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
        //    writer.WriteEndElement();

        //    writer.WriteEndElement();
        //}
    }
}
