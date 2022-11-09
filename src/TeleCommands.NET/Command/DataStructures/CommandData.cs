using TeleCommands.NET.ConsoleInterface.Structs;

namespace TeleCommands.NET.Command.DataStructures
{
    internal struct CommandData
    {
        public string CommandName { get; set; }
        public IndexMemory<char> OptionsData { get; set; }
    }
}
