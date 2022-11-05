
namespace TeleCommands.NET.CommandOption
{
    internal readonly struct Argument<T, TSource> where T : Option<T, TSource>
    {
        public char ArgumentCharacter { get; }
        public Func<T, TSource> ArgumentOption { get; }

        public Argument(char character, Func<T, TSource> argumentOption) 
        {
            ArgumentCharacter = character;
            ArgumentOption = argumentOption;
        }
    }
}
