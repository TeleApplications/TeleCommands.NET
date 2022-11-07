
namespace TeleCommands.NET.Console.Interfaces
{
    public interface IHandler
    {
        public uint Handle { get; }

        public void CreateHandler();
    }
}
