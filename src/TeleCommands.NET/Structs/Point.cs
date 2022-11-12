namespace TeleCommands.NET.ConsoleInterface.Structs
{
    public readonly struct Point
    {
        public float X { get; }
        public float Y { get; }

        public Point(float x, float y) 
        {
            X = x;
            Y = y;
        }
    }
}
