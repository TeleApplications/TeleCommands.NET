using System.Runtime.InteropServices;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.ConsoleInterface.Structs;

namespace TeleCommands.NET
{
    public class CommandOption<T> : Option<T, bool> 
        where T : Option<T, bool>, new()
    {
        private static readonly char emptyCharacter = ' ';
        protected ReadOnlyMemory<char> commandResult { get; private set; }

        public override OptionBarrier CharacterBarrier { get; } =
            new OptionBarrier('{', '}');

        public override async Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            var commandData = await GetCommandDataAsync(data.Data);
            var result = await CommandHelper.RunCommandAsync(commandData);
            commandResult = result.Value;

            return await base.ExecuteOptionAsync(new OptionData(data.Arguments, result.Value));
        }

        private async Task<CommandData> GetCommandDataAsync(ReadOnlyMemory<char> data) 
        {
            int nameIndex = await CommandHelper.GetFirstSeparatorIndexAsync(data, emptyCharacter);
            var name = data[0..nameIndex];

            int bufferLength = data.Length - nameIndex;
            var optionsData = new IndexMemory<char>(bufferLength);
            optionsData.Memory = data[(nameIndex + 1)..].ToArray();

            //TODO: Remove this expensive casting by just creating
            //a better implementation of copying that memory in CommandReader
            return new CommandData(MemoryMarshal.AsMemory(name), optionsData);
        }
    }
}
