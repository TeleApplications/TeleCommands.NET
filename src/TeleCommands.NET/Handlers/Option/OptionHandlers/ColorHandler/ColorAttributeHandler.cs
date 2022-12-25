using System.Diagnostics;
using TeleCommands.NET.Handlers.Option.OptionHandlers.ColorHandler.Attributes;

namespace TeleCommands.NET.Handlers.Option.OptionHandlers.ColorOptionHandler
{
    public sealed class ColorAttributeHandler : OptionAttributeHandler<ColorOptionAttribute, ConsoleColor>
    {
        public ColorAttributeHandler(Process handle)
            : base(handle)
        {
        }
    } 
}
