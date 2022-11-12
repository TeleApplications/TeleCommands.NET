using System.Diagnostics;
using System.Runtime.InteropServices;
using TeleCommands.NET.Command;


Console.Title = "Example";
var commandReader = new CommandReader(Process.GetCurrentProcess(), 128);
Task.Run(async() => await commandReader.StartListeningAsync());
while (true) { Console.ReadLine(); }
