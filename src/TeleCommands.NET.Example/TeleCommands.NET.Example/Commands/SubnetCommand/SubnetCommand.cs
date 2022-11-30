using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Example.Commands.SubnetCommand.Options;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Example.Commands.SubnetCommand
{
    [Command("subnet", typeof(SubnetCommand))]
    internal sealed class SubnetCommand : ICommand<bool>
    {
        public ImmutableArray<IOption<bool>> Options { get; } =
            ImmutableArray.Create
            (
                (IOption<bool>)DataOption.FactoryValue,
                (IOption<bool>)DataOption.FactoryValue
            );

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData)
        {
            var subnetDataOption = (Options[0] as DataOption);
            var addressDataOption = (Options[1] as DataOption);

            var subnetResult = subnetDataOption!.TryGetData(out ReadOnlyMemory<string> subnetData, optionData.Span[0]);
            var addressResult = addressDataOption!.TryGetData(out ReadOnlyMemory<string> addressData, optionData.Span[1]);

            return null!;
        }
    }
}
