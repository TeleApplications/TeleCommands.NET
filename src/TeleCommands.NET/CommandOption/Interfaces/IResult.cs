
namespace TeleCommands.NET.CommandOption.Interfaces
{
    internal interface IResult<T>
    {
        public T Value { get; }

        //In a future there will be a proper error message structure
        //for multiple commamnds at once
        public string Message { get; }
    }
}
