
namespace TeleCommands.NET.API.CommandOption.OptionStructs
{
    public readonly struct OptionBarrier
    {
        public char Start { get; }
        public char End { get; }

        public static OptionBarrier Empty { get; } =
            new(' ', ' ');

        public OptionBarrier(char start, char end) 
        {
            Start = start;
            End = end;
        }
    }
}
