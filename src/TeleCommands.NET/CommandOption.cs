using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET
{
    public sealed class CommandOption<T> : Option<T, bool> 
        where T : Option<T, bool>, new()
    {
        public override OptionBarrier CharacterBarrier { get; } =
            new OptionBarrier('{', '}');

        public override Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            return base.ExecuteOptionAsync(data);
        }
    }
}
