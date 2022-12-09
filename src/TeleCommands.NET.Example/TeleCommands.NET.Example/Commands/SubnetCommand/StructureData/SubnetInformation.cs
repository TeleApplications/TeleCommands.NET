using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData
{
    internal sealed class SubnetInformation
    {
        private static ReadOnlyMemory<byte> defaultMask =
            new byte[] { 255, 255, 255, 255 };

        public IPAddress IpAddress { get; }
        public IPAddress Mask { get; }
        public int Prefix { get; }

        public SubnetInformation(string ipAddress, int prefix)
        {
        }

        private IPAddress CalculateMask(int prefix) 
        {
            int prefixDifference = 32 - prefix;

            int shiftValue = prefixDifference % 8;
            int maskIndex = (prefixDifference - shiftValue) / 8;
        }
    }
}
