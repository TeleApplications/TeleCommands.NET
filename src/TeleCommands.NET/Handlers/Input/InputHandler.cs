using System.Diagnostics;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Handlers.Enums;

namespace TeleCommands.NET.Handlers.Input
{
    public abstract class InputHandler : IHandler, IDisposable
    {
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
        protected abstract ValueTask<uint> GetInputAsync();

        public async Task UpdateAsync() 
        {
            if (!isListening)
                await Task.CompletedTask;

            var currentKey = await GetInputAsync();
            CurrentPressedKey = currentKey;

            if (CurrentPressedKey != (uint)InputKey.UnknownKey) 
            {
                await OnInputAsync((uint)currentKey);
            }
        }

        public void Dispose() 
        {
            isListening = false;
        }
    }
}
