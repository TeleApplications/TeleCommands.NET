using System.Collections.Immutable;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.Interfaces
{
    public interface ICommand<T>
    {
        public ImmutableArray<IOption<T>> Options { get; }

        public Task<CommandResult> ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData);
    }
}
