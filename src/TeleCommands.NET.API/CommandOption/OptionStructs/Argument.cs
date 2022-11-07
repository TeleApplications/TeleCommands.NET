namespace TeleCommands.NET.CommandOption
{
    public readonly struct Argument
    {
        public string ArgumentName { get; }
        public Func<Task> ArgumentAction { get; }

        public Argument(string name, Func<Task> argumentOption) 
        {
            ArgumentName = name;
            ArgumentAction = argumentOption;
        }
    }
}
