using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Common.Lib.Common.Enums;
using Common.Lib.Interfaces;

namespace Common.Lib.Common.WCF
{
    public class LoggingMessageInspector : IClientMessageInspector
    {
        private readonly ILogger _logger;

        public LoggingMessageInspector() { }

        public LoggingMessageInspector(ILogger logger)
        {
            _logger = logger;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            MessageBuffer buffer = reply.CreateBufferedCopy(Int32.MaxValue);
            Message msgCopy = buffer.CreateMessage();

            reply = buffer.CreateMessage();

            // Get the SOAP XML content.
            string strMessage = buffer.CreateMessage().ToString();

            // Get the SOAP XML body content.
            System.Xml.XmlDictionaryReader xrdr = msgCopy.GetReaderAtBodyContents();
            string bodyData = xrdr.ReadOuterXml();

            // Replace the body placeholder with the actual SOAP body.
            strMessage = strMessage.Replace("... stream ...", bodyData);

            List<object> results = new List<object> { string.Format("Received:\n{0}", strMessage) };
            _logger.WriteLogEntry(results, "AfterReceiveReply", LogLevelType.Trace);
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            MessageBuffer buffer = request.CreateBufferedCopy(Int32.MaxValue);
            request = buffer.CreateMessage();
            List<object> results = new List<object> { string.Format("Sending:\n{0}", buffer.CreateMessage().ToString()) };
            _logger.WriteLogEntry(results, "BeforeSendRequest", LogLevelType.Trace);
            return null;
        }
    }
}
