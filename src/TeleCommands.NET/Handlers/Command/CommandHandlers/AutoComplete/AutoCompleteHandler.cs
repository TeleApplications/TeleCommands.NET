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
        public int CurrentIndex { get; private set; }

        public AutoCompleteHandler(Process process, Area area, int maxCommandCount) : base(process)
        {
            areaWriter = new AreaWriter(area);
            maxCommandCount = areaWriter.CalculateRelativePosition(new(0, maxCommandCount), area.Size).Y;
        }

        public override async Task UpdateAsync()
        {
            int squaredIndex = CurrentIndex == 0 ? CurrentIndex : Math.Abs(CurrentIndex) / CurrentIndex;
            CurrentIndex = (((CurrentIndex - currentAttributes.Length) * (squaredIndex)) / MaxCommandCount);
            int commandsLength = MaxCommandCount;
            for (int i = 0; i < commandsLength; i++)
            {

            }

            await base.UpdateAsync();
        }

        protected override Task OnWriteAsync(ReadOnlyMemory<char> data)
        {
            currentAttributes = GetSimilarAttributes(data);
            CurrentIndex = 0;
            return Task.CompletedTask;
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
                if (nameData.Equals(currentName)) 
                {
                    returnAttributes.Span[currentIndex] = currentAttribute;
                    currentIndex++;
                }
            }
            return returnAttributes[0..(currentIndex)];
        }
    }
}
