using System.Runtime.InteropServices;
using System.Text;

namespace TeleCommands.NET
{
    public static class InteropHelper
    {
        [DllImport("user32.dll")]
        public static extern uint GetAsyncKeyState(uint key);

        [DllImport("user32.dll")]
        public static extern int ToAscii(uint virtualKey, uint scanKey, byte[] lpKeyState, StringBuilder result, uint uFlags);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKeyA(uint uCode, uint uMapType);
    }
}
