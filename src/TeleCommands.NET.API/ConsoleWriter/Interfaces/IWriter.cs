using TeleCommands.NET.API.ConsoleWriter.Structures;
using TeleCommands.NET.API.ConsoleWriter.Structures.Character;

namespace TeleCommands.NET.API.ConsoleWriter.Interfaces
{
    internal interface IWriter
    {
        public Rectangle Rectangle { get; }
        public Memory<CharacterInformation> CharacterBuffer { get; }

        public void Write(char character, Coordination position, CharacterColor color);

        public void Display();

        public void Clear();
    }
}
