using TeleCommands.NET.API.Attributes;
using TeleCommands.NET.API.ConsoleWriter.Writers;
using TeleCommands.NET.API.ConsoleWriter.Writers.Structures;

namespace TeleCommands.NET.Handlers.Command.CommandHandlers.AutoComplete
{
    public sealed class AutoCompleteHandler : CommandHandler
    {
        private ReadOnlyMemory<CommandAttribute> currentAttributes;
        private AreaWriter areaWriter;

        public ConsoleColor AreaColor { get; set; }
            = ConsoleColor.Blue;
        public int MaxCommandCount { get; } = 5;
        public int CurrentIndex { get; private set; } = 0;

        public AutoCompleteHandler(Process process, Area area, int maxCommandCount) : base(process)
        {
            areaWriter = new AreaWriter(area);
            MaxCommandCount = areaWriter.CalculateRelativePosition(new(0, maxCommandCount), area.Size).Y;
        }

        public override async Task UpdateAsync()
        {
            await base.UpdateAsync();
            if (currentAttributes.Length == 0)
                return;
            areaWriter.Clear();

            int signValue = CalculateSign(CurrentIndex);
            int attributesIndex = (Math.Abs(CurrentIndex) + CurrentIndex) >> 1;

            int indexDifference = CurrentIndex - currentAttributes.Length;
            int shiftIndex = ((Math.Abs(indexDifference) + (indexDifference * signValue)) >> 1) / MaxCommandCount;

            CurrentIndex = (currentAttributes.Length * (attributesIndex ^ shiftIndex)) + indexDifference;
            int currentLength = currentAttributes.Length < MaxCommandCount ? currentAttributes.Length : MaxCommandCount;
            for (int i = 0; i < currentLength; i++)
            {
                int relativeIndex = CalculateRelativeIndex(CurrentIndex, currentAttributes.Length) - 1;
                var currentCommand = currentAttributes.Span[relativeIndex];

                //This position is will be changed by current line number
                areaWriter.Write($"[{i}]: {currentCommand.Name}", new(0, i), new(ConsoleColor.White, AreaColor));
            }
            areaWriter.Display();
        }

        protected override Task OnWriteAsync(ReadOnlyMemory<char> data)
        {
            currentAttributes = GetSimilarAttributes(data);
            CurrentIndex = 1;
            return Task.CompletedTask;
        }

        private int CalculateRelativeIndex(int value, int max) 
        {
            if (value < 0 || value > max) 
            {
                int signedValue = CalculateSign(CurrentIndex) * -1;
                return value + (max * signedValue);
            }
            return value;
        }

        //I know that this implementation for getting
        //just a signed value is quite expensive due to
        //double devide. It will be fixed in a future
        private int CalculateSign(int value) 
        {
            int absValue = Math.Abs(value);
            int relativeIndex = (1 % (absValue + 1)) ^ 1;
            return absValue / (value + relativeIndex);
        }

        private ReadOnlyMemory<CommandAttribute> GetSimilarAttributes(ReadOnlyMemory<char> nameData)
        {
            int dataLength = nameData.Length;
            int attributesLength = commandAttributes.Length;

            Memory<CommandAttribute> returnAttributes = new CommandAttribute[attributesLength];
            int currentIndex = 0;
            for (int i = 0; i < attributesLength; i++)
            {
                var currentAttribute = commandAttributes.Span[i];
                var currentName = currentAttribute.Name[0..(dataLength)].ToCharArray();
                if (nameData.Span.StartsWith(currentName))
                {
                    returnAttributes.Span[currentIndex] = currentAttribute;
                    currentIndex++;
                }
            }
            return returnAttributes[0..(currentIndex)];
        }
    }
}
