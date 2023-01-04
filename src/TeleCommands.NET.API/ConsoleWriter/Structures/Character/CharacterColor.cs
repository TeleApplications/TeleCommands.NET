namespace TeleCommands.NET.API.ConsoleWriter.Structures.Character
{
    public struct CharacterColor
    {
        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public static readonly CharacterColor DefaultColor =
            new(ConsoleColor.White, ConsoleColor.Black);

        public CharacterColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor) 
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }
}
