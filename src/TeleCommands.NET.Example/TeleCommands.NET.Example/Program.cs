using System.Diagnostics;
using TeleCommands.NET.Command;

var commandReader = new CommandReader(Process.GetCurrentProcess(), 128);
await commandReader.StartListeningAsync();
