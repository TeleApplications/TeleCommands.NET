using System.Diagnostics;
using TeleCommands.NET.ConsoleInterface.Interfaces;

namespace TeleCommands.NET.Handlers.Input
{
    public abstract class InputHandler : IHandler, IDisposable
    {
        public static readonly uint UnknownKey = 0x00F;
        private bool isListening = true;

        public uint CurrentPressedKey { get; protected set; }

        public IntPtr Handle { get; }

        public InputHandler(Process process) 
        {
            Handle = process.Handle;
            process.Disposed += (_, _)
                => Dispose();
        }

        protected abstract Task OnInputAsync(uint key);
        protected abstract Task<uint> GetInputAsync();

        public async Task UpdateAsync() 
        {
            if (!isListening)
                await Task.CompletedTask;

            uint currentKey = await GetInputAsync();
            CurrentPressedKey = currentKey;

            if (CurrentPressedKey != UnknownKey) 
            {
                await OnInputAsync(currentKey);
            }
        }

        public void Dispose() 
        {
            isListening = false;
        }
    }
}
