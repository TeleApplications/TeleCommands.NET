using System.Diagnostics;
using TeleCommands.NET.Console.Handlers.Input;
using TeleCommands.NET.Console.Interfaces;
using TeleCommands.NET.Console.Structs;

namespace TeleCommands.NET.Console
{
    internal sealed class CommandReader : IHandler, IDisposable
    {
        private ReadOnlyMemory<KeyAction> keyActions =>
            new KeyAction[]
            {
                new KeyAction(ConsoleKey.Spacebar, async() => 
                {
                    if(commandData.CommandName is null)
                    {
                        int index = commandData.OptionsData.Index;
                        commandData.CommandName = (commandData.OptionsData.Memory[0..(index + 1)]).ToString();
                        commandData.OptionsData.Index = 0;
                    }
                })
            };

        private KeyInputHandler inputHandler;
        private CommandData commandData;

        public bool IsListening { get; set; }
        public uint Handle { get; }

        public CommandReader(Process process, int maxCommandLength)
        {
            inputHandler = new(process);
            inputHandler.KeyActions = keyActions;
            Handle = inputHandler.Handle;

            commandData = new()
            {
                OptionsData = new IndexMemory<char>(maxCommandLength)
            };
            Task.Run(async () => await ListenCommandsAsync());
        }

        private async Task ListenCommandsAsync() 
        {
            while (IsListening) 
            {
                byte currentKey = (byte)inputHandler.CurrentPressedKey;
                if (currentKey != 0) 
                {
                    var optionsData = commandData.OptionsData;
                    optionsData.Memory.Span[optionsData.Index] = (char)currentKey;
                    optionsData.Index++;
                }
            }
        }

        public void CreateHandler() =>
            inputHandler.CreateHandler();
        public void Dispose() =>
            IsListening = false;
    }
}
