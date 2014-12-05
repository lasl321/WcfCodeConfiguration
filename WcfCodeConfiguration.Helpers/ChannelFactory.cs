using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace WcfCodeConfiguration.Helpers
{
    public class ChannelFactory<T>
    {
        private static IDictionary<string, int> _wellKnownPorts = new Dictionary<string, int>
        {
            {"EchoService", 50001}
        };

        private readonly System.ServiceModel.ChannelFactory<T> _channelFactory;

        public ChannelFactory(string hostName, int port)
            : this(new SimpleServiceNameResolver(), hostName, port, "localhost")
        {
        }

        public ChannelFactory(
            IServiceNameResolver serviceNameResolver,
            string hostName,
            int port,
            string dnsName)
        {
            var uri = new Uri(string.Format("net.tcp://{0}:{1}/{2}.svc",
                                            hostName,
                                            GetWellKnownPort(),
                                            serviceNameResolver.Resolve<T>()));
            var address = new EndpointAddress(uri, new DnsEndpointIdentity(dnsName));
            var binding = CreateNetTcpBinding();
            _channelFactory = CreateChannelFactory(binding, address);
        }

        private static int GetWellKnownPort()
        {
            var serviceName = typeof (T).Name.Substring(1);
            int port;
            if (_wellKnownPorts.TryGetValue(serviceName, out port))
            {
                return port;
            }

            throw new Exception(string.Format(
                "No known port for service {0}",
                serviceName));
        }

        private static System.ServiceModel.ChannelFactory<T> CreateChannelFactory(
            Binding binding,
            EndpointAddress address)
        {
            var channelFactory = new System.ServiceModel.ChannelFactory<T>(
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

        public T CreateChannel()
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