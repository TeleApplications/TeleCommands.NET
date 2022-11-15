using System.Diagnostics;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Handlers.Input;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.ConsoleInterface.Structs;

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

                        data.CommandName = data.OptionsData.Memory[0..(index + 1)].ToString();
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
            inputHandler = new(process, commandData);
            inputHandler.KeyActions = keyActions;
            Handle = inputHandler.Handle;

            commandData = new()
            {
                OptionsData = new IndexMemory<char>(maxCommandLength)
            };
        }

        public async Task UpdateAsync()
        {
            await inputHandler.UpdateAsync();
            byte currentKey = (byte)inputHandler.CurrentPressedKey;
            //if (currentKey != 0) 
            //{
                //var optionsData = commandData.OptionsData;
                //optionsData.Memory.Span[optionsData.Index] = (char)currentKey;
                //optionsData.Index++;
            //}
        }

        public void Dispose() =>
            IsListening = false;
    }
}
