namespace TeleCommands.NET.API.ConsoleWriter.Structures
{
    public struct Rectangle
    {
        public int Top { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public static Rectangle Zero =>
            new(0, 0, 0, 0);

        public Rectangle(int top, int left, int right, int bottom) 
        {
            Top = top;
            Left = left;
            Right = right;
            Bottom = bottom;
        }
    }
}
