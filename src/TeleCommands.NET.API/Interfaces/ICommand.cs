using System.Collections.Immutable;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.Interfaces
{
    public interface ICommand<T>
    {
        public ImmutableArray<IOption<T>> Options { get; }

        public Task ExecuteCommandAsync(ReadOnlyMemory<OptionData> optionData);
    }
}
