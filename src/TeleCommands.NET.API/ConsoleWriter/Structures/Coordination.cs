namespace TeleCommands.NET.API.ConsoleWriter.Structures
{
    public readonly struct Coordination
    {
        public int X { get; }
        public int Y { get; }

        public static Coordination Zero =>
            new(0, 0);

        public Coordination(int x, int y) 
        {
            X = x;
            Y = y;
        }
    }
}
