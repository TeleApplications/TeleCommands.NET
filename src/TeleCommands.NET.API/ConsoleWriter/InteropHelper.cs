using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using TeleCommands.NET.API.ConsoleWriter.Structures;
using TeleCommands.NET.API.ConsoleWriter.Structures.Character;

namespace TeleCommands.NET.API.ConsoleWriter
{
    internal static class InteropHelper
    {
        [DllImport("Kernel32.dll")]
        public static extern SafeFileHandle CreateFile(string name, uint access, uint share, uint securityAttributes,
            FileMode disposition, int flags, uint template);

        [DllImport("Kernel32.dll")]
        public static extern bool WriteConsoleOutputW(SafeFileHandle cosnoleOutput, CharacterInformation[] charBuffer,
            Coordination size, Coordination position, ref Rectangle writeRegion);
    }
}
