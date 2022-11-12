using System.Runtime.InteropServices;
using TeleCommands.NET.ConsoleInterface.Structs;
using TeleCommands.NET.Structs;

namespace TeleCommands.NET
{
    internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static class InteropHelper
    {
        [DllImport("kernel32.dll")]
        public static extern bool ReadConsoleInputW(IntPtr hConsoleInput, out InputRecord inputRecord, uint length, out uint numberOfRead); 

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr handle, uint dwMode);
    }
}
