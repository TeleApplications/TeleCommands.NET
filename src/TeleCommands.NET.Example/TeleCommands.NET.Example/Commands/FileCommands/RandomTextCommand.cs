using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.API.Attributes;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.API.Interfaces;

namespace TeleCommands.NET.Example.Commands.FileCommands
{
    [Command("rndtxt", typeof(RandomTextCommand))]
    internal sealed class RandomTextCommand : ICommand<bool>
    {
        private static readonly string testText =
            "Hi, I'am Ma-tes and I'm main and only developer of this library";
        public ImmutableArray<IOption<bool>> Options { get; }

        public async Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData)
        {
            return new CommandResult(testText.ToCharArray());
        }
    }
}
