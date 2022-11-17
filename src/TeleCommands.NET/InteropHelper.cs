using System.Runtime.InteropServices;

namespace TeleCommands.NET
{
    internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static class InteropHelper
    {
        [DllImport("user32.dll")]
        public static extern int GetAsyncKeyState(int key);
    }
}
