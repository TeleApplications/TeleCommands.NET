using System.Diagnostics;
using TeleCommands.NET.ConsoleInterface.Interfaces;

namespace TeleCommands.NET.Handlers.Input
{
    public abstract class InputHandler : IHandler, IDisposable
    {
        protected const uint UnknownKey = 0x00F;
        private bool isListening = true;

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

            uint currentKey = await GetInputAsync();
            if (currentKey != UnknownKey) 
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
