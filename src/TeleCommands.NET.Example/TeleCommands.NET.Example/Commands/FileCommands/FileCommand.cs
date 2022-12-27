using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.API.Attributes;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.API.Interfaces;
using TeleCommands.NET.Example.Commands.FileCommands.Options;

namespace TeleCommands.NET.Example.Commands.FileCommands
{
    [Command("file", typeof(FileCommand))]
    internal sealed class FileCommand : ICommand<bool>
    {
        public ImmutableArray<IOption<bool>> Options { get; } =
            ImmutableArray.Create
            (
                (IOption<bool>)ShowFileOption.FactoryValue
            );

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData)
        {
            var currentOptionData = optionData.Span[0];
            await Options[0].ExecuteOptionAsync(currentOptionData);

            return null!;
        }
    }
}
