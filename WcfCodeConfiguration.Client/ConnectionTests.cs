using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using Microsoft.WindowsAzure;
using NUnit.Framework;
using WcfCodeConfiguration.Contract;

namespace WcfCodeConfiguration.Client
{
    public class ConnectionTests
    {
        private string _host;

        [SetUp]
        public void SetUp()
        {
            _host = CloudConfigurationManager.GetSetting("Host");
        }

        [Test]
        public void ShouldConnectToServer()
        {
            var service = GetInstance(_host);
            var data = service.GetData(1111);

            Assert.That(data, Is.EqualTo("You entered: 1111"));
        }

        [Test]
        public void ShouldConnectToServer2()
        {
            var service = GetInstance(_host);
            var data = service.GetDataUsingDataContract(new CompositeType
            {
                BoolValue = true,
                StringValue = "FooBar"
            });

            Assert.That(data,
                        Is.EqualTo(new CompositeType
                        {
                            BoolValue = true,
                            StringValue = "FooBarSuffix"
                        }));
        }

        private static IEchoService GetInstance(string host)
        {
            var uri = new Uri(string.Format("net.tcp://{0}:50001/EchoService.svc", host));
            var address = new EndpointAddress(uri, new DnsEndpointIdentity("localhost"));
            var binding = new NetTcpBinding(SecurityMode.Transport)
            {
                Security =
                {
                    Transport = {ClientCredentialType = TcpClientCredentialType.None}
                }
            };
            var factory = new ChannelFactory<IEchoService>(binding, address);
            var endpointBehavior = factory.Endpoint.EndpointBehaviors[typeof (ClientCredentials)];
            var clientCredentials = (ClientCredentials) endpointBehavior;
            clientCredentials.ServiceCertificate.Authentication.CertificateValidationMode =
                X509CertificateValidationMode.None;

            return factory.CreateChannel();
        }
    }
}