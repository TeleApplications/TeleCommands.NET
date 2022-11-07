namespace TeleCommands.NET.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CommandAttribute : Attribute
    {
        public string Name { get; }

        public CommandAttribute(string name) 
        {
            Name = name;
        }
    }
}
