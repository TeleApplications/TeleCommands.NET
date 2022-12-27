using System.Diagnostics;
using TeleCommands.NET.Command;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Handlers;
using TeleCommands.NET.Handlers.Option.OptionHandlers.ColorOptionHandler;
using TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler;

var currentProcess = Process.GetCurrentProcess();
var handlerManager =
    new HandlerManager(new IHandler[]
    {
        new CommandReader(currentProcess, 128)
        {
            OnReadAction = () =>
            {
                Console.Write($"{DateTime.Now.ToShortTimeString()}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{CommandReader.defaultCommandSymbol} ");
                Console.ForegroundColor = ConsoleColor.White;
            }
        },
        new ColorAttributeHandler(currentProcess),
        new InformationAttributeHandler(currentProcess, ConsoleColor.DarkGray)
    });
await handlerManager.StartHandlersUpdateAsync();
Console.ReadLine();
