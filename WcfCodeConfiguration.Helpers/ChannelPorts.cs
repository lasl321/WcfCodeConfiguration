using System;
using System.Collections.Generic;

namespace WcfCodeConfiguration.Helpers
{
    internal class ChannelPorts
    {
        private static readonly IDictionary<string, int> PortsMap = new Dictionary<string, int>
        {
            {"IEchoService", 50001}
        };

        public int this[Type channelType]
        {
            get
            {
                int port;
                if (PortsMap.TryGetValue(channelType.Name, out port))
                {
                    return port;
                }

                throw new InvalidOperationException(string.Format(
                    "No port known for channel type {0}",
                    channelType.Name));
            }
        }
    }
}