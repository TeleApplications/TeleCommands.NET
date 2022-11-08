
namespace TeleCommands.NET.Console.Structs
{
    internal sealed class IndexMemory<T>
    {
        public Memory<T> Memory { get; set; }
        public int Index { get; set; }

        public IndexMemory(int size) 
        {
            Memory = new T[size];
        }
    }
}
