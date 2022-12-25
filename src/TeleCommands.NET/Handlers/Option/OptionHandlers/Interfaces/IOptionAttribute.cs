namespace TeleCommands.NET.Handlers.Option
{
    public interface IOptionAttribute<T>
    {
        public ReadOnlyMemory<T> AttributeData { get; }
    }
}
