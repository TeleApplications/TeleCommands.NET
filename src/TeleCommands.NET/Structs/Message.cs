
namespace TeleCommands.NET.Structs
{
    public readonly struct Message
    {
        public uint Handle { get; }
        public uint LParam { get; }
        public uint WParam { get; }
        public uint Result { get; }

        public int Msg { get; }
    }
}
