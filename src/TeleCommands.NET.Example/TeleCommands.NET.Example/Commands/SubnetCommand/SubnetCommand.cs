using System.Collections.Immutable;
using System.Net;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Example.Commands.SubnetCommand.Options;
using TeleCommands.NET.Example.Commands.SubnetCommand.StructureData;
using TeleCommands.NET.Example.Commands.SubnetCommand.StructureData.NetworkStructure;
using TeleCommands.NET.Handlers.Option.OptionHandlers.ColorHandler.Attributes;
using TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler.Attributes;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Example.Commands.SubnetCommand
{
    [Command("subnet", typeof(SubnetCommand))]
    internal sealed class SubnetCommand : ICommand<bool>
    {
        private static readonly char networkSeparator = '=';

        [ColorOption(ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue)]
        [InformationOption("--[A=257,B=127000,C=800]--", "--[10.0.0.0-192.168.255.255]--", "--[1-31]--")]
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

            addressDataOption!.TryGetData(out ReadOnlyMemory<string> addressData, optionData.Span[1]);
            prefixDataOption!.TryGetData(out ReadOnlyMemory<string> prefixData, optionData.Span[2]);
            if (subnetDataOption!.TryGetData(out ReadOnlyMemory<string> subnetData, optionData.Span[0]).Value) 
            {
                var networkData = await GetNetworkDataAsync(subnetData);

                int prefix = int.Parse(prefixData.Span[0]);
                var networkInformation = CalculateNetworkSubnet(new SubnetInformation(addressData.Span[0], prefix), networkData);
                WriteInformations(networkInformation);
            }

            return null!;
        }
        
        //This is here just for temporary testing, because
        //in a future, there is going to be better way of
        //writing command content into the command line
        private void WriteInformations(ReadOnlyMemory<NetworkInformation> informations) 
        {
            int informationLength = informations.Length;
            for (int i = 0; i < informationLength; i++)
            {
                var currentSubnet = informations.Span[i];
                Console.WriteLine(currentSubnet);
            }
        }

        private async Task<Memory<NetworkData>> GetNetworkDataAsync(ReadOnlyMemory<string> data) 
        {
            int dataLength = data.Length;
            Memory<NetworkData> networkData = new NetworkData[dataLength];

            for (int i = 0; i < dataLength; i++)
            {
                string currentData = data.Span[i];
                int separatorIndex = await CommandHelper.GetFirstSeparatorIndexAsync(currentData.ToCharArray(), networkSeparator);

                int hostCount = int.Parse(currentData[(separatorIndex + 1)..]);
                networkData.Span[i] = new NetworkData(currentData[0..(separatorIndex)], hostCount);
            }
            return networkData;
        }

        private ReadOnlyMemory<NetworkInformation> CalculateNetworkSubnet(SubnetInformation information, Memory<NetworkData> networkData)
        {
            int hostCount = CalculateHostCount(information.Prefix);
            if (hostCount <= GetNetworkSize(networkData))
                throw new OverflowException("Host count is greater than overall size of networks");

            int sliceCount = 1;
            Memory<NetworkInformation> networkInformations = new NetworkInformation[networkData.Length];

            IPAddress baseBroadCast = CalculateBroadCast(information.IpAddress, information.Prefix + sliceCount);
            var baseRange = new IpRange(information.IpAddress, baseBroadCast, information.Prefix + sliceCount);
            IpRange[] addressRanges = { baseRange, baseRange};

            int index = 0;
            int networkLength = networkData.Length;
            while (networkLength != index) 
            {
                int networkCount = (networkLength) - index;
                for (int i = 0; i < networkCount; i++)
                {
                    var currentNetwork = networkData.Span[i];
                    var currentRange = new IpRange(addressRanges[1].Start, addressRanges[1].End, information.Prefix + sliceCount);

                    if (InIpRange(currentNetwork, currentRange))
                    {
                        networkData.Span[i] = networkData.Span[(networkCount - 1)];
                        networkInformations.Span[index] = new NetworkInformation(currentNetwork, currentRange);
                        index++;
                    }
                }

                sliceCount++;
                addressRanges[1].End = addressRanges[0].End;
                addressRanges[1].Start = CalculateBroadCast(addressRanges[0].End, information.Prefix + sliceCount);
                addressRanges[0].End = CalculateBroadCast(addressRanges[0].Start, information.Prefix + sliceCount);
            }
            return networkInformations;
        }

        private int GetNetworkSize(Memory<NetworkData> networkData) 
        {
            int returnSize = 0;
            int networkLength = networkData.Length;
            for (int i = 0; i < networkLength; i++)
            {
                var currentNetwork = networkData.Span[i];
                returnSize += currentNetwork.HostCount;
            }

            return returnSize;
        }

        private int CalculateHostCount(int prefix) =>
            (int)Math.Pow(2, (32 - prefix));

        private bool InIpRange(NetworkData data, IpRange addressRange) 
        {
            int minHostCount = CalculateHostCount(addressRange.Prefix) / 2;
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
