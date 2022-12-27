using TeleCommands.NET.API.CommandOption.Interfaces;
using TeleCommands.NET.API.CommandOption.OptionStructs;

namespace TeleCommands.NET.Example.Commands.FileCommands.Options
{
    internal sealed class ShowTextOption : CommandOption<ShowTextOption>
    {
        public override ReadOnlyMemory<Argument> Arguments { get; }

        public override async Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            var result = await base.ExecuteOptionAsync(data);
            Console.WriteLine(commandResult.ToString());
            return result;
        }
    }
}
