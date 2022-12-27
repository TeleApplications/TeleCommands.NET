using System.Diagnostics;
using TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler.Attributes;

namespace TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler
{
    public sealed class InformationAttributeHandler : OptionAttributeHandler<InformationOptionAttribute, string>
    {
        private ConsoleColor informationColor;
        public InformationAttributeHandler(Process handle, ConsoleColor color)
            : base(handle)
        {
            informationColor = color;
        }

        protected override async Task OnOptionAttributeAsync(string attributeData)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = informationColor;
            Console.Write($"{attributeData} ");
            Console.ForegroundColor = oldColor;
        }
    }
}
