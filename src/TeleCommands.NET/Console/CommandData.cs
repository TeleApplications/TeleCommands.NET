using TeleCommands.NET.Console.Structs;

namespace TeleCommands.NET.Console
{
    internal struct CommandData
    {
        public string CommandName { get; set; }
        public IndexMemory<char> OptionsData { get; set; }
    }
}
