using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Common.Lib.Interfaces;

namespace Common.Lib.Common.WCF
{
    public class LoggingEndpointBehavior: IEndpointBehavior
    {
        private readonly ILogger _logger;

        public LoggingEndpointBehavior() { }

        public LoggingEndpointBehavior(ILogger logger)
        {
            _logger = logger;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            // No implementation necessary
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new LoggingMessageInspector(_logger));
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
