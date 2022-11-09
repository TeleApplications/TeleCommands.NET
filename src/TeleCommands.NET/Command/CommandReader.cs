using System.Diagnostics;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Handlers.Input;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.ConsoleInterface.Structs;

namespace TeleCommands.NET.Command
{
    internal sealed class CommandReader : IHandler, IDisposable
    {
        private ReadOnlyMemory<KeyAction<CommandData>> keyActions =
            new KeyAction<CommandData>[]
            {
                new KeyAction<CommandData>(ConsoleKey.Spacebar, async(data) =>
                {
                    if(data.CommandName is null)
                    {
                        int index = data.OptionsData.Index;
                        data.CommandName = data.OptionsData.Memory[0..(index + 1)].ToString();
                        data.OptionsData.Index = 0;
                    }
                })
            };

        private KeyInputHandler<CommandData> inputHandler;
        private CommandData commandData;

        public bool IsListening { get; set; }
        public uint Handle { get; }

        public CommandReader(Process process, int maxCommandLength)
        {
            inputHandler = new(process, commandData);
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
