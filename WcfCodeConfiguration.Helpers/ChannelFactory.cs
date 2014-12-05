using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

namespace WcfCodeConfiguration.Helpers
{
    public class ChannelFactory<TChannel>
    {
        private const string DnsName = "localhost";

        // ReSharper disable once StaticFieldInGenericType
        private static readonly IDictionary<string, int> Ports = new Dictionary<string, int>
        {
            {"IEchoService", 50001}
        };

        private readonly System.ServiceModel.ChannelFactory<TChannel> _channelFactory;

        public ChannelFactory(string hostName)
        {
            var uri = new Uri(string.Format("net.tcp://{0}:{1}/{2}.svc",
                                            hostName,
                                            GetPort<TChannel>(),
                                            typeof (TChannel).Name));
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

        private static int GetPort<T>()
        {
            return Ports[typeof (T).Name];
        }
    }
}