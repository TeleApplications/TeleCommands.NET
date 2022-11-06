
namespace TeleCommands.NET
{
    internal abstract class Factory<T> where T : Factory<T>, new()
    {
        private readonly static T currentInstance = new();

        public static T FactoryValue { get; } = currentInstance;
    }
}
