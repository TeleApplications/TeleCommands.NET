using System.Diagnostics;
using TeleCommands.NET.ConsoleInterface.Interfaces;

namespace TeleCommands.NET.Handlers.Input
{
    internal abstract class InputHandler : IHandler, IDisposable
    {
        protected const uint UnknownKey = 0x00F;
        private bool isListening = true;

        public uint CurrentKey { get; private set; }
        public IntPtr Handle { get; }

        public InputHandler(Process process) 
        {
            Handle = process.Handle;
            process.Disposed += (_, _) 
                => Dispose();
        }

        protected abstract Task OnInputAsync(uint key);
        protected abstract Task<uint> GetInputAsync();

        private async Task StartReadingInput() 
        {
            while (isListening) 
            {
                uint currentKey = await GetInputAsync();
                if (currentKey != UnknownKey) 
                {
                    await OnInputAsync(currentKey);
                    CurrentKey = currentKey;
                }
            }
        }

        public void CreateHandler() =>
            Task.Run(() => StartReadingInput());
        public void Dispose() 
        {
            isListening = false;
        }
    }
}
