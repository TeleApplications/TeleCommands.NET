using TeleCommands.NET.CommandOption.Interfaces;

namespace TeleCommands.NET.CommandOption.Results
{
    internal sealed class ErrorResult<T> : IResult<T>
    {
        public T Value { get; }
        public string Message { get; }

        public ErrorResult(T ?value, string message) 
        {
            Value = value!;
            Message = message;
        }
    }
}
