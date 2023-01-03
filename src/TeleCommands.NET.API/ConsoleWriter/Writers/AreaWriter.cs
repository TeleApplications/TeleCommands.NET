using Microsoft.Win32.SafeHandles;
using TeleCommands.NET.API.ConsoleWriter.Interfaces;
using TeleCommands.NET.API.ConsoleWriter.Structures;

namespace TeleCommands.NET.API.ConsoleWriter.Writers
{
    public class AreaWriter : IWriter, IDisposable
    {
        private SafeFileHandle fileHandle;
        private Coordination areaPosition;

        public Rectangle Rectangle { get; }
        public Memory<char> CharacterBuffer { get; }

        //TODO: Create size structure, instead of
        //Coordination structures
        public AreaWriter(Coordination position, Coordination size)
        {
            areaPosition = position;
            Rectangle = new(0, 0, size.X, size.Y);

            int charactersLength = size.X * size.Y;
            CharacterBuffer = new char[charactersLength];
        }

        public async Task WriteAsync(char character, Coordination position) 
        {
        }

        public void Dispose()
        {
            fileHandle.Dispose();
        }
    }
}
