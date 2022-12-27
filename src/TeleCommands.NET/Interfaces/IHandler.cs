namespace TeleCommands.NET.Interfaces
{
    public interface IHandler
    {
        public IntPtr Handle { get; }

        public Task UpdateAsync();

        public Task OnReadAsync();
    }
}
