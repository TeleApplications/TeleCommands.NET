namespace TeleCommands.NET.Handlers.Option.OptionHandlers.ColorHandler.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ColorOptionAttribute : Attribute, IOptionAttribute<ConsoleColor>
    {
        public ReadOnlyMemory<ConsoleColor> AttributeData { get; }

        public ColorOptionAttribute(params ConsoleColor[] optionColors) 
        {
            AttributeData = optionColors;
        }
    }
}
