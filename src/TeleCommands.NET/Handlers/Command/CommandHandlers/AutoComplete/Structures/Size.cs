namespace TeleCommands.NET.Handlers.Command.CommandHandlers.AutoComplete.Structures
{
    public readonly struct Size
    {
        public int Widht { get; }
        public int Height { get; }

        public Size(int widht, int height) 
        {
            Widht = widht;
            Height = height;
        }
    }
}
