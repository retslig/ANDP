using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Common.Lib.Common.WCF
{

    // Endpoint behavior
    public class RemoveHeaderEndpointBehavior : IEndpointBehavior
    {
        private readonly string _headerName;
        private readonly string _headerNameSpace;

        public RemoveHeaderEndpointBehavior() { }

        public RemoveHeaderEndpointBehavior(string name, string ns)
        {
            _headerName = name;
            _headerNameSpace = ns;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new HeaderRemoverMessageInspector(_headerName, _headerNameSpace));
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            // No implementation necessary
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            // No implementation necessary
        }
    }
}
