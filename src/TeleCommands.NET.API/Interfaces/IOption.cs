using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.API.CommandOption.Interfaces;

namespace TeleCommands.NET.API.Interfaces
{
    public interface IOption<T>
    {
        public OptionBarrier CharacterBarrier { get; }
        public ReadOnlyMemory<Argument> Arguments { get; }

        public Task<IResult<T>> ExecuteOptionAsync(OptionData data);
    }
}
