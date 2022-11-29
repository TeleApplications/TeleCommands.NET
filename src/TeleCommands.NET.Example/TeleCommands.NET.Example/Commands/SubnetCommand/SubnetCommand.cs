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
                (IOption<bool>)DataOption.FactoryValue
            );

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData) 
        {
            var dataOption = (Options[0] as DataOption);
            var data = dataOption!.TryGetData(out ReadOnlyMemory<string> result, optionData.Span[0]);

            return null!;
        }
    }
}
