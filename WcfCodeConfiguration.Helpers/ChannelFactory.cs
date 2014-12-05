using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace WcfCodeConfiguration.Helpers
{
    public class ChannelFactory<TChannel>
    {
        private const string DnsName = "localhost";

        private readonly System.ServiceModel.ChannelFactory<TChannel> _channelFactory;
        private readonly ChannelPorts _channelPorts = new ChannelPorts();

        public ChannelFactory(string hostName)
        {
            if (hostName == null)
            {
                throw new ArgumentNullException("hostName");
            }

            var channelType = typeof (TChannel);
            var port = _channelPorts[channelType];
            var path = string.Format("{0}.svc", channelType.Name);
            var uri = new Uri(string.Format("net.tcp://{0}:{1}/{2}",
                                            hostName,
                                            port,
                                            path));

            // TODO Validate URI.

            var address = new EndpointAddress(uri, new DnsEndpointIdentity(DnsName));
            var binding = CreateNetTcpBinding();
            _channelFactory = CreateChannelFactory(binding, address);
        }

        private static System.ServiceModel.ChannelFactory<TChannel> CreateChannelFactory(
            Binding binding,
            EndpointAddress address)
        {
            var channelFactory = new System.ServiceModel.ChannelFactory<TChannel>(
                binding,
                address);

            var endpointBehavior = channelFactory
                .Endpoint
                .EndpointBehaviors[typeof (ClientCredentials)];

            ((ClientCredentials) endpointBehavior)
                .ServiceCertificate
                .Authentication
                .CertificateValidationMode = X509CertificateValidationMode.None;

            return channelFactory;
        }

        public TChannel CreateChannel()
        {
            return _channelFactory.CreateChannel();
        }

        private static NetTcpBinding CreateNetTcpBinding()
        {
            return new NetTcpBinding(SecurityMode.Transport)
            {
                Security =
                {
                    Transport = {ClientCredentialType = TcpClientCredentialType.None}
                }
            };
        }
    }
}