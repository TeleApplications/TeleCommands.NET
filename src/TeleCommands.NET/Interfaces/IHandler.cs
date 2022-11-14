namespace TeleCommands.NET.ConsoleInterface.Interfaces
{
    public interface IHandler
    {
        public IntPtr Handle { get; }

        public Task UpdateAsync();
    }
}
