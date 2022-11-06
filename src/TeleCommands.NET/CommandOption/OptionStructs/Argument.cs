

namespace TeleCommands.NET.CommandOption
{
    public readonly struct Argument
    {
        public char ArgumentCharacter { get; }
        public Func<Task> ArgumentAction { get; }

        public Argument(char character, Func<Task> argumentOption) 
        {
            ArgumentCharacter = character;
            ArgumentAction = argumentOption;
        }
    }
}
