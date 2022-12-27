using TeleCommands.NET.Handlers.Option.Interfaces;

namespace TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InformationOptionAttribute : Attribute, IOptionAttribute<string>
    {
        public ReadOnlyMemory<string> AttributeData { get; }

        public InformationOptionAttribute(params string[] optionColors)
        {
            AttributeData = optionColors;
        }
    }
}
