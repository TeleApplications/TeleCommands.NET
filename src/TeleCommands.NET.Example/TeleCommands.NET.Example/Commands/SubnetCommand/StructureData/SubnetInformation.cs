using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData
{
    internal sealed class SubnetInformation
    {
        private static readonly int privateAddressCount = 3;

        public IPAddress IpAddress { get; }
        public IPAddress Mask { get; }

        public SubnetInformation(string ipAddress) 
        {
            var newAddress = IPAddress.Parse(ipAddress);
            IpAddress = newAddress;

            byte maskIdentificator = byte.Parse(ipAddress[0..3]);
            Mask = CalculateMask(maskIdentificator);
        }

        private IPAddress CalculateMask(byte addressIdentificator)
        {
            byte[] addressBytes = new byte[32];
            byte maskLength = byte.MaxValue / 2;

            for (int i = 0; i < privateAddressCount; i++)
            {
                int firstIndex = (maskLength * (i));
                int lastIndex = (maskLength * (i + 1));
            }

            return new IPAddress(addressBytes);
        }
    }
}
