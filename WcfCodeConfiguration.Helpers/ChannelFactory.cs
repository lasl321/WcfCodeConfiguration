using System.ServiceModel;

namespace WcfCodeConfiguration.Helpers
{
    public class ChannelFactory<T>
    {
        private System.ServiceModel.ChannelFactory<T> _channelFactory;

        public ChannelFactory()
        {
            _channelFactory = new System.ServiceModel.ChannelFactory<T>(new NetTcpBinding());
        }
    }
}