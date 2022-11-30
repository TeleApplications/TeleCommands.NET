using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData
{
    internal readonly struct SubnetInformation
    {
        private static readonly int privateAddressCount = 3;

        public IPAddress IpAddress { get; }
        public IPAddress Mask { get; }

        public SubnetInformation(string ipAddress) 
        {
            var newAddress = IPAddress.Parse(ipAddress);
        }

        private IPAddress CalculateMask(byte addressIdentificator)
        {
            byte maskLength = byte.MaxValue / 2;

            for (int i = 0; i < privateAddressCount; i++)
            {
                int firstIndex = (maskLength * (i));
                int lastIndex = (maskLength * (i + 1));
            }
        }
    }
}
