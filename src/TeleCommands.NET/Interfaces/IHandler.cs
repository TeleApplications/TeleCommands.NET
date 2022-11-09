namespace TeleCommands.NET.ConsoleInterface.Interfaces
{
    public interface IHandler
    {
        public uint Handle { get; }

        public void CreateHandler();
    }
}
