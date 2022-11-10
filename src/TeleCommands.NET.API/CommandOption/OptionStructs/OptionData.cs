namespace TeleCommands.NET.CommandOption.OptionStructs
{
    public readonly struct OptionData
    {
        public ReadOnlyMemory<char> Arguments { get; }
        public ReadOnlyMemory<char> Data { get; }

        public OptionData(ReadOnlyMemory<char> arguments, ReadOnlyMemory<char> data) 
        {
            Arguments = arguments;
            Data = data;
        }
    }
}
