﻿using System.Collections.Immutable;
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

            var subnetResult = subnetDataOption!.TryGetData(out ReadOnlyMemory<string> subnetData, optionData.Span[0]);
            var addressResult = addressDataOption!.TryGetData(out ReadOnlyMemory<string> addressData, optionData.Span[0]);

            return null!;
        }

        private async Task<ReadOnlyMemory<NetworkData>> GetNetworkData(ReadOnlyMemory<string> data) 
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

        private IEnumerable<NetworkInformation> CalculateNetworkSubnet(SubnetInformation information, ReadOnlyMemory<NetworkData> networkData)
        {
            //TODO: Create first check, if even the all networks fit
            //into the mask host length
            IPAddress baseBroadCast = CalculateBroadCast(information.IpAddress, information.Prefix);
            var addressRanges = new List<IpRange>() { new IpRange(information.IpAddress, baseBroadCast, information.Prefix) };
            int index = 0;

            int networkLength = networkData.Length;
            while (networkLength != index) 
            {

            }
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