
namespace TeleCommands.NET.CommandOption
{
    internal readonly struct Argument<T>
    {
        public char ArgumentCharacter { get; }
        public Func<T, Task<T>> ArgumentFunction { get; }

        public Argument(char character, Func<T, Task<T>> argumentOption) 
        {
            ArgumentCharacter = character;
            ArgumentFunction = argumentOption;
        }
    }
}
