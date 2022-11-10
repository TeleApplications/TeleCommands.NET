using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.Interfaces
{
    public interface IOption<T>
    {
        public OptionBarrier CharacterBarrier { get; }
        public ReadOnlyMemory<Argument> Arguments { get; }

        public Task<IResult<T>> ExecuteOptionAsync(OptionData data);
    }
}
