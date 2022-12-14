using System.Collections.Immutable;
using System.Net;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Example.Commands.SubnetCommand.Options;
using TeleCommands.NET.Example.Commands.SubnetCommand.StructureData;
using TeleCommands.NET.Example.Commands.SubnetCommand.StructureData.NetworkStructure;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Example.Commands.SubnetCommand
{
    [Command("subnet", typeof(SubnetCommand))]
    internal sealed class SubnetCommand : ICommand<bool>
    {
        private static readonly char networkSeparator = '=';

        public ImmutableArray<IOption<bool>> Options { get; } =
            ImmutableArray.Create<IOption<bool>>
            (
                DataOption.FactoryValue,
                DataOption.FactoryValue,
                DataOption.FactoryValue
            );

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData)
        {
            var subnetDataOption = (Options[0] as DataOption);
            var addressDataOption = (Options[1] as DataOption);
            var prefixDataOption = (Options[2] as DataOption);

            addressDataOption!.TryGetData(out ReadOnlyMemory<string> addressData, optionData.Span[0]);
            prefixDataOption!.TryGetData(out ReadOnlyMemory<string> prefixData, optionData.Span[0]);
            if (subnetDataOption!.TryGetData(out ReadOnlyMemory<string> subnetData, optionData.Span[0]).Value) 
            {
                var networkData = await GetNetworkDataAsync(subnetData);
                var networkInformation = CalculateNetworkSubnet(new SubnetInformation(addressData.Span[0], int.Parse(prefixData.Span[0])), networkData);
            }


            return null!;
        }

        private async Task<Memory<NetworkData>> GetNetworkDataAsync(ReadOnlyMemory<string> data) 
        {
            int dataLength = data.Length;
            Memory<NetworkData> networkData = new NetworkData[dataLength];

            for (int i = 0; i < dataLength; i++)
            {
                string currentData = data.Span[i];
                int separatorIndex = await CommandHelper.GetFirstSeparatorIndexAsync(currentData.ToCharArray(), networkSeparator);

                networkData.Span[i] = new NetworkData(currentData[0..(separatorIndex)], currentData[(separatorIndex)]);
            }
            return networkData;
        }

        private IEnumerable<NetworkInformation> CalculateNetworkSubnet(SubnetInformation information, Memory<NetworkData> networkData)
        {
            //TODO: Create first check, if even the all networks fit
            //into the mask host length
            IPAddress baseBroadCast = CalculateBroadCast(information.IpAddress, information.Prefix);
            var baseRange = new IpRange(information.IpAddress, baseBroadCast, information.Prefix);
            IpRange[] addressRanges = { baseRange, baseRange};

            int sliceCount = 0;
            int index = 0;
            int networkLength = networkData.Length;
            while (networkLength != index) 
            {
                int networkCount = (networkLength) - index;
                for (int i = 0; i < networkCount; i++)
                {
                    var currentNetwork = networkData.Span[i];
                    if (InIpRange(currentNetwork, addressRanges[1]))
                    {
                        networkData.Span[i] = networkData.Span[(networkCount - 1)];
                        index++;

                        yield return new NetworkInformation(currentNetwork, addressRanges[1]);
                    }
                }
                sliceCount++;

                addressRanges[0].Start = CalculateBroadCast(addressRanges[0].Start, information.Prefix + sliceCount);
                addressRanges[1].Start = CalculateBroadCast(addressRanges[0].End, information.Prefix + sliceCount);
                addressRanges[1].End = addressRanges[0].End;
            }
        }

        private bool InIpRange(NetworkData data, IpRange addressRange) 
        {
            int minHostCount = (int)Math.Pow(2, (32 - addressRange.Prefix)) / 2;
            return data.HostCount > (minHostCount - 2);
        }

        private IPAddress CalculateBroadCast(IPAddress address, int prefix)
        {
            var addressBytes = address.GetAddressBytes();
            var maskBytes = SubnetInformation.CalculateMask(prefix).GetAddressBytes();

            int maskLength = maskBytes.Length;
            for (int i = 0; i < maskLength; i++)
            {
                byte currentData = (byte)(255 ^ maskBytes[i]);
                addressBytes[i] = (byte)Math.Abs(addressBytes[i] - currentData);
            }
            return new IPAddress(addressBytes);
        }
    }
}
