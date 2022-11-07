using TeleCommands.NET.CommandOption.Interfaces;

namespace TeleCommands.NET.CommandOption.Results
{
    public sealed class SuccesfulResult<T> : IResult<T>
    {
        public T Value { get; }
        public string Message { get; } =
            String.Empty;

        public SuccesfulResult(T value) 
        {
            Value = value;
        }
    }
}
