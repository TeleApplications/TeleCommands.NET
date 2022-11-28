using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Example.Commands.SubnetCommand
{
    [Command("subnet", typeof(SubnetCommand))]
    internal sealed class SubnetCommand : ICommand<bool>
    {
        public ImmutableArray<IOption<bool>> Options { get; } =
            ImmutableArray.Create
            (
            );

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData) 
        {
        }
    }
}
