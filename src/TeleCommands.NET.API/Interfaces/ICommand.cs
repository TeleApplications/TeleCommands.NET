using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.API.CommandOption.OptionStructs;

namespace TeleCommands.NET.API.Interfaces
{
    public interface ICommand<T>
    {
        public ImmutableArray<IOption<T>> Options { get; }

        public Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData);
    }
}
