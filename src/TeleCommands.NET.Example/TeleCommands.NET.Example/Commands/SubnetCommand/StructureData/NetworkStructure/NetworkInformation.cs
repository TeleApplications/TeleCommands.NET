using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData.NetworkStructure
{
    internal struct IpRange 
    {
        public IPAddress Start { get; set; }
        public IPAddress End { get; set; }
        public int Prefix { get; set; }

        public IpRange(IPAddress start, IPAddress end, int prefix)
        {
            Start = start;
            End = end;
            Prefix = prefix;
        }
    }

    internal struct NetworkInformation
    {
        public NetworkData Network { get; }
        public IpRange AddressRange { get; set; }

        public NetworkInformation(NetworkData network, IpRange range) 
        {
            Network = network;
            AddressRange = range;
        }
    }
}
