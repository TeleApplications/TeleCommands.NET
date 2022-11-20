namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public readonly struct KeyAction<T>
    {
        public ConsoleKey Key { get; }
        public Func<T, Task<T>> Action { get; }

        public KeyAction(ConsoleKey key, Func<T, Task<T>> action) 
        {
            Key = key;
            Action = action;
        }
    }
}
