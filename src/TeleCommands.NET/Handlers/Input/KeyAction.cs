namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public readonly struct KeyAction<T>
    {
        public ConsoleKey Key { get; }
        public Func<T, Task> Action { get; }

        public KeyAction(ConsoleKey key, Func<T, Task> action) 
        {
            Key = key;
            Action = action;
        }
    }
}
