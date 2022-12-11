using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData
{
    internal sealed class SubnetInformation
    {
        private static readonly int byteSize = 8;
        private static readonly Memory<byte> defaultMask =
            new byte[] { 255, 255, 255, 255 };

        public IPAddress IpAddress { get; }
        public IPAddress Mask { get; }
        public int Prefix { get; }

        public SubnetInformation(string ipAddress, int prefix)
        {
            IpAddress = IPAddress.Parse(ipAddress);
            Prefix = prefix;
            Mask = CalculateMask(prefix);
        }

        public static IPAddress CalculateMask(int prefix)
        {
            var maskBytes = defaultMask;
            int prefixDifference = 32 - prefix;

            int shiftValue = prefixDifference % byteSize;
            int maskShiftLenght = (prefixDifference - shiftValue) / byteSize;
            for (int i = 0; i < maskShiftLenght; i++)
            {
                int currentIndex = (defaultMask.Length - 1) - i;
                maskBytes.Span[currentIndex] = (byte)(maskBytes.Span[currentIndex] >> byteSize);
            }

            int lastIndex = defaultMask.Length - (maskShiftLenght + 1);
            maskBytes.Span[lastIndex] = (byte)(maskBytes.Span[lastIndex] & (maskBytes.Span[lastIndex] << shiftValue));
            return new IPAddress(maskBytes.ToArray());
        }
    }
}
