namespace TeleCommands.NET.API.ConsoleWriter.Interfaces
{
    internal interface IWriter
    {
        public Memory<char> CharacterBuffer { get; }

        public Task Write(string data)
    }
}
