namespace TeleCommands.NET.ConsoleInterface.Structs
{
    internal readonly struct Message
    {
        public uint Hwnd { get; }
        public uint CurrentMessage { get; }
        public uint WParam { get; }
        public uint LParam { get; }
        public int Time { get; }
        public Point Point { get; }
        public int LPrivate { get; }
    }
}
