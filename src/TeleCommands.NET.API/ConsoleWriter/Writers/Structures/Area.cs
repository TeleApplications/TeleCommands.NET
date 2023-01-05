using TeleCommands.NET.API.ConsoleWriter.Structures;

namespace TeleCommands.NET.API.ConsoleWriter.Writers.Structures
{
    public readonly struct Area
    {
        public Coordination Position { get; }
        public Coordination Size { get; }

        public Area(Coordination position, Coordination size) 
        {
            Position = position;
            Size = size;
        }
    }
}
