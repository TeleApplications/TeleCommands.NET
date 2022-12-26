using TeleCommands.NET.ConsoleInterface.Structs;

namespace TeleCommands.NET.Command.DataStructures
{
    public struct CommandData
    {
        public Memory<char> CommandName { get; set; }
        public IndexMemory<char> OptionsData { get; set; }
        public int OptionIndex { get; set; } = 0;


        public CommandData(Memory<char> commandName, IndexMemory<char> optionsData) 
        {
            CommandName = commandName;
            OptionsData = optionsData;
        }
    }
}
