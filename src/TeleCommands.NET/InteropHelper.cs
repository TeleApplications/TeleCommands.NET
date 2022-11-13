using System.Runtime.InteropServices;
using TeleCommands.NET.ConsoleInterface.Structs;
using TeleCommands.NET.Structs;

namespace TeleCommands.NET
{
    internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    public static class InteropHelper
    {
        [DllImport("kernel32.dll")]
        public static extern bool ReadConsoleInput(IntPtr hConsoleInput, out InputRecord inputRecord, uint length, out uint numberOfRead);

        [DllImport("kernel32.dll")]
        public static extern bool FlushConsoleInputBuffer(IntPtr hConsoleInput);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetStdHandle(int handle);

        [DllImport("kernel32.dll")]
        public static extern IntPtr SetStdHandle(int device, IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool ReadConsole(IntPtr consoleHandle, out byte[] buffer, uint nNumberBytesToRead, out uint bytesToRead, uint lpOverLappped);

        [DllImport("kernel32.dll")]
        public static extern bool GetNumberOfConsoleInputEvents(IntPtr consoleInput, out int events);

        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleMode(IntPtr handle, out uint dwMode);

        [DllImport("kernel32.dll")]
        public static extern bool SetConsoleMode(IntPtr handle, uint dwMode);
    }
}
