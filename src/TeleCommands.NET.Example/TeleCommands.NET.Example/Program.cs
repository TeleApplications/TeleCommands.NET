using System.Diagnostics;
using TeleCommands.NET.Command;

Console.Title = "Example";
var commandReader = new CommandReader(Process.GetCurrentProcess(), 128);
Task.Run(async() => { while (true) { await commandReader.UpdateAsync(); } });
Console.ReadLine();
