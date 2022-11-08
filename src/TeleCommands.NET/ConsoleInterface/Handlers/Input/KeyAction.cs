namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public readonly struct KeyAction
    {
        public ConsoleKey Key { get; }
        public Func<Task> Action { get; }

        public KeyAction(ConsoleKey key, Func<Task> action) 
        {
            Key = key;
            Action = action;
        }
    }
}
