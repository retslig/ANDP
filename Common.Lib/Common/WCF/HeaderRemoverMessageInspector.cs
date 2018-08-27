using System.ServiceModel;
using System.ServiceModel.Dispatcher;

namespace Common.Lib.Common.WCF
{
    public class HeaderRemoverMessageInspector : IClientMessageInspector
    {
        private readonly string _headerName;
        private readonly string _headerNameSpace;

        public HeaderRemoverMessageInspector() { }

        public HeaderRemoverMessageInspector(string name, string ns)
        {
            _headerName = name;
            _headerNameSpace = ns;
        }

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            // No implementation necessary
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel)
        {
            if (!string.IsNullOrEmpty(_headerName) && !string.IsNullOrEmpty(_headerNameSpace))
            {
                int index = request.Headers.FindHeader(_headerName, _headerNameSpace);
                if (index >= 0)
                    request.Headers.RemoveAt(index);
            }
            return null;
        }
    }
}
