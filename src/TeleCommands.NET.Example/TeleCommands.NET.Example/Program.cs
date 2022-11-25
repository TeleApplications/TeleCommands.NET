using System.Diagnostics;
using TeleCommands.NET.Command;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Handlers;


var currentProcess = Process.GetCurrentProcess();
var handlerManager =
    new HandlerManager(new IHandler[]
    {
        new CommandReader(currentProcess, 128),
    });
await handlerManager.StartHandlersUpdateAsync();
