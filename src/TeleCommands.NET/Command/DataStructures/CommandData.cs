using TeleCommands.NET.ConsoleInterface.Structs;

namespace TeleCommands.NET.Command.DataStructures
{
    public struct CommandData
    {
        public string CommandName { get; set; }
        public IndexMemory<char> OptionsData { get; set; }

        public CommandData(string commandName, IndexMemory<char> optionsData) 
        {
            CommandName = commandName;
            OptionsData = optionsData;
        }
    }
}
