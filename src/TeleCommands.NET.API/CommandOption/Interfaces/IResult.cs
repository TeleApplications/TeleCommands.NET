namespace TeleCommands.NET.API.CommandOption.Interfaces
{
    public interface IResult<T>
    {
        public T Value { get; }

        //In a future there will be a proper error message structure
        //for multiple commamnds at once
        public string Message { get; }
    }
}
