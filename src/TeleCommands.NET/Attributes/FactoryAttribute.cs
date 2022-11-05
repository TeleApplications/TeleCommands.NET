
namespace TeleCommands.NET.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal sealed class FactoryAttribute : Attribute
    {
        public Type ObjectType { get; }

        public FactoryAttribute(Type objectType) 
        {
            ObjectType = objectType;
        }
    }
}
