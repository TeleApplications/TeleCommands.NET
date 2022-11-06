using System.Collections.Immutable;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Example.Commands.FileCommands.Options;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Example.Commands.FileCommands
{
    [Command("file")]
    internal sealed class FileCommand : ICommand<bool>
    {
        public ImmutableArray<IOption<bool>> Options { get; } =
            ImmutableArray.Create
            (
                (IOption<bool>)ShowFileOption.FactoryValue
            );

        public async Task ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData) 
        {
            var currentOptionData = optionData.Span[0];
            await Options[0].ExecuteOptionAsync(currentOptionData);
        }
    }
}
