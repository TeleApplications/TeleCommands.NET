using System.Runtime.InteropServices;
using System.Text;

namespace TeleCommands.NET
{
    internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static class InteropHelper
    {
        [DllImport("user32.dll")]
        public static extern uint GetAsyncKeyState(uint key);

        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKey, uint scanCode, byte[] keyboardBuffer, ref StringBuilder result, int bufferSize, uint flags);
    }
}
