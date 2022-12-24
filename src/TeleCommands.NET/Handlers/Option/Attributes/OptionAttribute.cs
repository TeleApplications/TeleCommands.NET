
namespace TeleCommands.NET.Handlers.Option.Attributes
{
    internal abstract class OptionAttribute<T>
    {
        public ReadOnlyMemory<T> AttributeData { get; }

        public OptionAttribute(params T[] data)
        {
            AttributeData = data;
        }
    }
}
