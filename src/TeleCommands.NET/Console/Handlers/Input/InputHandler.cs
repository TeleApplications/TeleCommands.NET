using System.Diagnostics;
using TeleCommands.NET.Console.Interfaces;

namespace TeleCommands.NET.Console.Handlers.Input
{
    public abstract class InputHandler : IHandler
    {
        private uint inputProc;
        private uint module;

        protected abstract uint InputMessage { get; }
        public uint Handle { get; }

        public InputHandler(Process consoleProcess) 
        {
            if (!consoleProcess.Responding || consoleProcess.MainModule is null)
                throw new Exception($"Current process: {consoleProcess.ProcessName} is not active");

            Handle = (uint)consoleProcess.Handle;
            string moduleProcessName = consoleProcess.MainModule!.ModuleName!;

            module = InteropHelper.GetModuleHandle(moduleProcessName);
        }

        public void CreateHandler() =>
            inputProc = InteropHelper.SetWindowsHookEx(13, InputHookProc, module, 0);

        private uint InputHookProc(int code, uint wParam, uint lParam) 
        {
            if (wParam == InputMessage)
                OnHookProc(wParam, lParam);
            return InteropHelper.CallNextHookEx(inputProc, code, wParam, lParam);
        }

        protected abstract void OnHookProc(uint wParam, uint lParam);
    }
}
