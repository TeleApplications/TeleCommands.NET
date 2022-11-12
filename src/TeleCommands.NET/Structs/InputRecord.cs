
namespace TeleCommands.NET.Structs
{
    [Flags]
    public enum ControlKey : uint 
    {
        LeftShift = 0x00010 
    }

    public struct KeyEvent
    {
        public uint VirtualKeyCode { get; set; }
        public uint VirtualScanCode { get; set; }
        public char Character { get; set; }
        public bool KeyDown { get; set; }
        public uint ControlKeyState { get; set; }
    }

    public struct InputRecord
    {
        public uint EventType { get; set; }
        public KeyEvent KeyEvent { get; set; }
    }
}
