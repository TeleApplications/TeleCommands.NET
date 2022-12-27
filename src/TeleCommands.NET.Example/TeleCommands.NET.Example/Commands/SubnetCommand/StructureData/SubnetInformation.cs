using System.Net;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.StructureData
{
    internal sealed class SubnetInformation
    {
        private static readonly int byteSize = 8;
        private static ReadOnlySpan<byte> defaultMask =>
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
            Span<byte> maskBytes = stackalloc byte[defaultMask.Length];
            defaultMask.CopyTo(maskBytes);

            int prefixDifference = 32 - prefix;
            int shiftValue = prefixDifference % byteSize;
            int maskShiftLenght = (prefixDifference - shiftValue) / byteSize;
            for (int i = 0; i < maskShiftLenght; i++)
            {
                int currentIndex = (defaultMask.Length - 1) - i;
                maskBytes[currentIndex] = (byte)(maskBytes[currentIndex] >> byteSize);
            }

            int lastIndex = defaultMask.Length - (maskShiftLenght + 1);
            maskBytes[lastIndex] = (byte)(maskBytes[lastIndex] & (maskBytes[lastIndex] << shiftValue));
            return new IPAddress(maskBytes.ToArray());
        }
    }
}
