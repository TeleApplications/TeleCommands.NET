
namespace TeleCommands.NET.CommandOption.OptionStructs
{
    public readonly struct OptionData
    {
        public ReadOnlyMemory<char> Arguments { get; }
        public ReadOnlyMemory<char> Data { get; }
    }
}
