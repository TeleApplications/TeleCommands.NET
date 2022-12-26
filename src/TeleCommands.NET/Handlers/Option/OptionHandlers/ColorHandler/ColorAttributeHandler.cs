using System.Diagnostics;
using TeleCommands.NET.Handlers.Option.OptionHandlers.ColorHandler.Attributes;

namespace TeleCommands.NET.Handlers.Option.OptionHandlers.ColorOptionHandler
{
    //This is not actually a best example of this attribute handler,
    //but in a future it will containg more options for multiple colors
    public sealed class ColorAttributeHandler : OptionAttributeHandler<ColorOptionAttribute, ConsoleColor>
    {
        public ColorAttributeHandler(Process handle)
            : base(handle)
        {
        }

        protected override async Task OnOptionAttributeAsync(ConsoleColor attributeData)
        {
            Console.ForegroundColor = attributeData;
            await Task.CompletedTask;
        }
    } 
}
