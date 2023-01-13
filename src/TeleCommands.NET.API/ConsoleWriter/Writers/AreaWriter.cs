using Microsoft.Win32.SafeHandles;
using System.Runtime.CompilerServices;
using TeleCommands.NET.API.ConsoleWriter.Interfaces;
using TeleCommands.NET.API.ConsoleWriter.Structures;
using TeleCommands.NET.API.ConsoleWriter.Structures.Character;
using TeleCommands.NET.API.ConsoleWriter.Writers.Structures;

namespace TeleCommands.NET.API.ConsoleWriter.Writers
{
    public class AreaWriter : IWriter, IDisposable
    {
        private const uint ReadHandle = 0x40000000;
        private const string ReadName = "CONOUT$";

        private SafeFileHandle fileHandle;
        private Area bufferArea;

        public Rectangle Rectangle { get; private set; }
        public Memory<CharacterInformation> CharacterBuffer { get; private set; }

        //TODO: Create size structure, instead of
        //Coordination structures
        public AreaWriter(Area area)
        {
            bufferArea = area;
            Rectangle = new(0, 0, bufferArea.Size.X, bufferArea.Size.Y);

            int charactersLength = bufferArea.Size.X * bufferArea.Size.Y;
            CharacterBuffer = new CharacterInformation[charactersLength];
            fileHandle = InteropHelper.CreateFile(ReadName, ReadHandle, 2, 0, FileMode.Open, 0, 0);
        }

        public void Write(string text, Coordination position) =>
            Write(text, position, CharacterColor.DefaultColor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(string text, Coordination position, CharacterColor color) 
        {
            int textLength = text.Length;
            for (int i = 0; i < textLength; i++)
            {
                var currentPosition = new Coordination(position.X + i, position.Y);
                var currentCharacter = text[i];

                Write(currentCharacter, currentPosition, color);
            }
        }

        public void Write(char character, Coordination position) =>
            Write(character, position, CharacterColor.DefaultColor);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char character, Coordination position, CharacterColor color) 
        {
            var relativePosition = CalculateRelativePosition(position, bufferArea.Size);

            int currentIndex = (relativePosition.Y + 1) * (relativePosition.X + 1);
            var currentInformation = new CharacterInformation(character, color);
            CharacterBuffer.Span[currentIndex] = currentInformation;
        }

        public virtual void Display() 
        {
            var rectangleReference = Rectangle;
            _ = InteropHelper.WriteConsoleOutputW(fileHandle, CharacterBuffer.ToArray(), bufferArea.Size, bufferArea.Position, ref rectangleReference);
        }

        public virtual void Clear() =>
            CharacterBuffer = new CharacterInformation[bufferArea.Size.X * bufferArea.Size.Y];

        public Coordination CalculateRelativePosition(Coordination position, Coordination size) 
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
