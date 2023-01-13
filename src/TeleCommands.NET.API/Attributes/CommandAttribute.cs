namespace TeleCommands.NET.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : Attribute
    {
        //TODO: This needs to be stored as ReadOnlyMemory of chars
        public string Name { get; }
        public Type Type { get; }

        public CommandAttribute(string name, Type type) 
        {
            Name = name;
            Type = type;
        }
    }
}
