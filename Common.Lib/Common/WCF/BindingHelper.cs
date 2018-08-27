using System.ServiceModel.Channels;
using System.Text;
using Common.Lib.Common.UsernameToken;

namespace Common.Lib.Common.WCF
{
    public static class BindingHelper
    {
        public static Binding UserNameBinding(MessageVersion messageVersion)
        {
            // NOTE: we use a non-secure transport here, which means the message will be visible to others.
            HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();

            // the transport security binding element will be configured to require a username token
            TransportSecurityBindingElement transportSecurity = new TransportSecurityBindingElement();
            transportSecurity.EndpointSupportingTokenParameters.SignedEncrypted.Add(new UsernameTokenParameters());

            // here you can require secure transport, in which case you'd probably replace HTTP with HTTPS as well
            transportSecurity.AllowInsecureTransport = true;
            transportSecurity.IncludeTimestamp = false;

            //MtomMessageEncodingBindingElement mtom = new MtomMessageEncodingBindingElement
            //                                             {
            //                                                 WriteEncoding = System.Text.Encoding.UTF8,
            //                                                 MessageVersion = MessageVersion.Soap11WSAddressing10,
            //                                                 MaxBufferSize = 2147483647
            //                                             };
            //mtom.ReaderQuotas.MaxStringContentLength = 2147483647;

            TextMessageEncodingBindingElement me = new TextMessageEncodingBindingElement(messageVersion, Encoding.UTF8);

            return new CustomBinding(transportSecurity, me, httpTransport);
        }
    }
}
