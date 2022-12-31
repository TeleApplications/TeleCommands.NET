using TeleCommands.NET.API.Attributes;
using TeleCommands.NET.Handlers.Command.CommandHandlers.AutoComplete.Structures;

namespace TeleCommands.NET.Handlers.Command.CommandHandlers.AutoComplete
{
    public sealed class AutoCompleteHandler : CommandHandler
    {
        private ConsoleColor areaColor;
        private Size areaSize;

        public int MaxCommandCount { get; set; } = 5;

        public AutoCompleteHandler(Process process, ConsoleColor menuColor, Size menuSize) : base(process)
        {
            areaColor = menuColor;
            areaSize = menuSize;
        }

        protected override async Task OnWriteAsync(ReadOnlyMemory<char> data)
        {

        }

        private static IEnumerable<CommandAttribute> GetSimilarAttributes(ReadOnlyMemory<char> nameData) 
        {
            int dataLength = nameData.Length;
            int attributesLength = commandAttributes.Length;
            for (int i = 0; i < attributesLength; i++)
            {
                var currentAttribute = commandAttributes.Span[i];
                var currentName = currentAttribute.Name[0..(dataLength)].ToCharArray();
                if (nameData.Equals(currentName))
                    yield return currentAttribute;
            }
        }
    }
}
