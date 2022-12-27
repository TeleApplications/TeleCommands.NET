using System.Diagnostics;
using TeleCommands.NET.Command;
using TeleCommands.NET.Interfaces;
using TeleCommands.NET.Handlers;
using TeleCommands.NET.Handlers.Option.OptionHandlers.ColorHandler;
using TeleCommands.NET.Handlers.Option.OptionHandlers.InformationHandler;

var currentProcess = Process.GetCurrentProcess();
var handlerManager =
    new HandlerManager(new IHandler[]
    {
        new CommandReader(currentProcess, 128)
        {
            OnReadAction = async() =>
            {
                Console.Write($"{DateTime.Now.ToShortTimeString()}");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{CommandReader.defaultCommandSymbol} ");
                Console.ForegroundColor = ConsoleColor.White;
                await Task.CompletedTask;
            }
        },
        new ColorAttributeHandler(currentProcess),
        new InformationAttributeHandler(currentProcess, ConsoleColor.DarkGray)
    });
await handlerManager.StartHandlersUpdateAsync();
Console.ReadLine();
