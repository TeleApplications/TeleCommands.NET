using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using TeleCommands.NET.API.ConsoleWriter.Interfaces;
using TeleCommands.NET.API.ConsoleWriter.Structures;
using TeleCommands.NET.API.ConsoleWriter.Structures.Character;

namespace TeleCommands.NET.API.ConsoleWriter.Writers
{
    public class AreaWriter : IWriter, IDisposable
    {
        private const uint ReadHandle = 0x40000000;
        private const string ReadName = "CONOUT$";

        private SafeFileHandle fileHandle;
        private Coordination areaPosition;
        private Coordination areaSize;

        public Rectangle Rectangle { get; private set; }
        public Memory<CharacterInformation> CharacterBuffer { get; private set; }

        //TODO: Create size structure, instead of
        //Coordination structures
        public AreaWriter(Coordination position, Coordination size)
        {
            areaPosition = position;
            areaSize = size;
            Rectangle = new(0, 0, size.X, size.Y);

            int charactersLength = areaSize.X * areaSize.Y;
            CharacterBuffer = new CharacterInformation[charactersLength];
            fileHandle = InteropHelper.CreateFile(ReadName, ReadHandle, 2, 0, FileMode.Open, 0, 0);
        }

        public void Write(char character, Coordination position) =>
            Write(character, position, CharacterColor.DefaultColor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char character, Coordination position, CharacterColor color) 
        {
            var relativePosition = CalculateRelativePosition(position, areaSize);

            int currentIndex = relativePosition.Y * relativePosition.X;
            var currentInformation = new CharacterInformation(character, color);
            CharacterBuffer.Span[currentIndex] = currentInformation;
        }

        public virtual void Display() 
        {
            var rectangleReference = Rectangle;
            _ = InteropHelper.WriteConsoleOutputW(fileHandle, CharacterBuffer.ToArray(), areaSize, areaPosition, ref rectangleReference);
        }

        public virtual void Clear() =>
            CharacterBuffer = new CharacterInformation[areaSize.X * areaSize.Y];

        private Coordination CalculateRelativePosition(Coordination position, Coordination size) 
        {
            int xDifference = position.X - size.X;
            int yDifference = position.Y - size.Y;

            int positionX = (size.X + xDifference) % (position.X - xDifference);
            int positionY = (size.Y + yDifference) % (position.Y - yDifference);
            return new Coordination(positionX, positionY);
        }

        public void Dispose()
        {
            fileHandle.Dispose();
        }
    }
}
