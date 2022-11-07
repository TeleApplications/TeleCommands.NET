using System.Diagnostics;
using System.Runtime.InteropServices;
using TeleCommands.NET.Console.Interfaces;
using TeleCommands.NET.Console.Structs;

namespace TeleCommands.NET.Console.Handlers.Input
{
    public sealed class KeyInputHandler : InputHandler
    {
        private const uint WM_KEYDOWN = 0x100;
        protected override uint InputMessage =>
            WM_KEYDOWN;

        public ConsoleKey CurrentPressedKey { get; private set; }

        public KeyInputHandler(Process process) : base(process)
        {
        }

        protected override void OnHookProc(uint wParam, uint lParam)
        {
            var message = Marshal.PtrToStructure<Message>((IntPtr)lParam);
            CurrentPressedKey = (ConsoleKey)message.LParam;
        }
    }
}
