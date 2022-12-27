namespace TeleCommands.NET.API.CommandOption.OptionStructs
{
    public readonly struct Argument
    {
        public static readonly char ArgumentSeparator = '-';
        public static readonly char UnknownArgument = ' ';

        public string ArgumentName { get; }
        public Func<Task> ArgumentAction { get; }

        public Argument(string name, Func<Task> argumentOption) 
        {
            ArgumentName = name;
            ArgumentAction = argumentOption;
        }
    }
}
