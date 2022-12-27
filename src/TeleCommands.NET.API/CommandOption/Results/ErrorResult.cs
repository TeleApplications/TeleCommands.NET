using TeleCommands.NET.API.CommandOption.Interfaces;

namespace TeleCommands.NET.API.CommandOption.Results
{
    public sealed class ErrorResult<T> : IResult<T>
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
