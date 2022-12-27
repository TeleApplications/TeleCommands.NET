namespace TeleCommands.NET.Handlers.Option.Interfaces
{
    public interface IOptionAttribute<T>
    {
        public ReadOnlyMemory<T> AttributeData { get; }
    }
}
