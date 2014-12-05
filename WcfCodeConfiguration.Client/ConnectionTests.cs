using Microsoft.WindowsAzure;
using NUnit.Framework;
using WcfCodeConfiguration.Contract;
using WcfCodeConfiguration.Helpers;

namespace WcfCodeConfiguration.Client
{
    public class ConnectionTests
    {
        private ChannelFactory<IEchoService> _channelFactory;
        private string _hostName;
        private IEchoService _service;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _hostName = CloudConfigurationManager.GetSetting("HostName");
            _channelFactory = new ChannelFactory<IEchoService>(_hostName);
        }

        [SetUp]
        public void SetUp()
        {
            _service = _channelFactory.CreateChannel();
        }

        [Test]
        public void ShouldConnectToServer()
        {
            var data = _service.GetData(1111);

            Assert.That(data, Is.EqualTo("You entered: 1111"));
        }

        [Test]
        public void ShouldConnectToServer2()
        {
            var data = _service.GetDataUsingDataContract(new CompositeType
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
    }
}