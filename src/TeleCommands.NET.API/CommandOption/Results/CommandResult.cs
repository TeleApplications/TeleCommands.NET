using TeleCommands.NET.API.CommandOption.Interfaces;

namespace TeleCommands.NET.API.CommandOption.Results
{
    public sealed class CommandResult : IResult<ReadOnlyMemory<char>>
    {
        public ReadOnlyMemory<char> Value { get; set; }
        public string Message { get; set; } = String.Empty;

        public CommandResult(ReadOnlyMemory<char> value)
        {
            Value = value;
        }
    }
}
