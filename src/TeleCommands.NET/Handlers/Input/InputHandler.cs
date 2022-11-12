using System.Diagnostics;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Structs;

namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public abstract class InputHandler : IHandler
    {
        protected abstract uint InputMessage { get; }

        public IntPtr Handle { get; set; }

        public InputHandler(Process consoleProcess) 
        {
            if (!consoleProcess.Responding || consoleProcess.Handle == IntPtr.Zero)
                throw new Exception($"Current process: {consoleProcess.ProcessName} is not active");
        }

        protected abstract Task OnInputMessage(InputRecord inputRecord);

        private async Task ListenMessageAsync() 
        {
            var currentConsoleInput = InteropHelper.GetStdHandle(-10);
            InteropHelper.SetConsoleMode(currentConsoleInput, 0x0008 | 0x001);

            while (true) 
            {
                if (!InteropHelper.ReadConsoleInputW(currentConsoleInput, out InputRecord inputRecord, 2, out uint reads))
                    throw new Exception("Reading console input is not possible");
                if(inputRecord.KeyEvent.VirtualKeyCode != 1)
                    await OnInputMessage(inputRecord);
            }
        }

        public void CreateHandler() 
        {
            Task.Run(async() => await ListenMessageAsync());
        }
    }
}
