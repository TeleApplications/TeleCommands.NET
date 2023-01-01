namespace TeleCommands.NET.API.ConsoleWriter.Structures.Character
{
    internal struct CharacterInformation
    {
        public char Character { get; set; }
        public short Attribute { get; set; }

        public CharacterInformation(char character, CharacterColor color)
        {
            Character = character;
            Attribute = (short)(((int)color.ForegroundColor | (int)color.BackgroundColor) << 4);
        }
    }
}
