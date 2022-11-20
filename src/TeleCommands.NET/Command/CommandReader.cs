using System.Diagnostics;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Handlers.Input;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.ConsoleInterface.Structs;
using TeleCommands.NET.Handlers.Enums;
using TeleCommands.NET.Handlers.Input;

namespace TeleCommands.NET.Command
{
    public sealed class CommandReader : IHandler, IDisposable
    {
        private ReadOnlyMemory<KeyAction<CommandData>> keyActions =
            new KeyAction<CommandData>[]
            {
                new KeyAction<CommandData>(ConsoleKey.Spacebar, async(data) =>
                {
                    if(data.CommandName is null)
                    {
                        int index = data.OptionsData.Index;

                        data.CommandName = data.OptionsData.Memory[0..(index)].ToString();
                        data.OptionsData.Index = 0;
                    }
                }),
                new KeyAction<CommandData>(ConsoleKey.Enter, async(data) =>
                {
                    await CommandHelper.RunCommandAsync(data);
                    data.OptionsData.Index = 0;
                })
            };

        //This is temporary solutions due to uncomplemet command
        //handler manager, that will contain every specific handler
        private KeyInputHandler<CommandData> inputHandler;
        private CommandData commandData;

        public bool IsListening { get; set; } = true;
        public IntPtr Handle { get; }

        public CommandReader(Process process, int maxCommandLength)
        {
            commandData = new()
            {
                OptionsData = new IndexMemory<char>(maxCommandLength)
            };

            inputHandler = new(process, commandData);
            inputHandler.KeyActions = keyActions;
            Handle = inputHandler.Handle;
        }

        public async Task UpdateAsync()
        {
            var currentKey = inputHandler.CurrentPressedKey;
            if (currentKey != (uint)InputKey.UnknownKey && currentKey != 0)
            {
                Console.Write($"{(char)currentKey}");

                var optionsData = commandData.OptionsData;
                optionsData.Memory.Span[optionsData.Index] = (char)currentKey;
                optionsData.Index++;
            }
            await inputHandler.UpdateAsync();
        }

        public void Dispose() =>
            IsListening = false;
    }
}
