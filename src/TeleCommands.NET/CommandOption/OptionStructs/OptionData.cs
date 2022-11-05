
namespace TeleCommands.NET.CommandOption.OptionStructs
{
    internal readonly struct OptionData
    {
        public ReadOnlyMemory<char> Arguments { get; }
        public ReadOnlyMemory<char> Data { get; }
    }
}
