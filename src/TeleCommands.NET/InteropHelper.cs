using System.Runtime.InteropServices;

namespace TeleCommands.NET
{
    internal delegate uint HookProc(int code, uint wParam, uint lParam);
    internal static class InteropHelper
    {
        [DllImport("User32.dll")]
        public static extern uint SetWindowsHookEx(uint hookType, HookProc lpfn, uint hMod, uint dwThreadId);

        [DllImport("User32.dll")]
        public static extern uint CallNextHookEx(uint hhk, int nCode, uint wParam, uint lParam);

        [DllImport("Kernel32.dll")]
        public static extern uint GetModuleHandle(string lpModuleName);
    }
}
