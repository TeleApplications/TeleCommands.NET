namespace TeleCommands.NET
{
    public abstract class Factory<T> where T : Factory<T>, new()
    {
        private readonly static T currentInstance = new();

        public static T FactoryValue { get; } = currentInstance;
    }
}
