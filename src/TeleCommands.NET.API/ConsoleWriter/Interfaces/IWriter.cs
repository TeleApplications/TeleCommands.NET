using TeleCommands.NET.API.ConsoleWriter.Structures;

namespace TeleCommands.NET.API.ConsoleWriter.Interfaces
{
    internal interface IWriter
    {
        public Rectangle Rectangle { get; }
        public Memory<char> CharacterBuffer { get; }

        public Task WriteAsync(char character, Coordination position);
    }
}
