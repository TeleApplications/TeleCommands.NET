using System.Diagnostics;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Handlers.Input;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.ConsoleInterface.Structs;
using TeleCommands.NET.Handlers.Enums;

namespace TeleCommands.NET.Command
{
    public sealed class CommandReader : IHandler, IDisposable
    {
        private ReadOnlyMemory<KeyAction<CommandData>> keyActions =
            new KeyAction<CommandData>[]
            {
                new KeyAction<CommandData>(ConsoleKey.Spacebar, (data) =>
                {
                    if(data.CommandName is null)
                    {
                        int index = data.OptionsData.Index;
                        data.CommandName = data.OptionsData.Memory[0..(index)].ToString();
                        data.OptionsData.Index = 0;
                    }
                    return Task.FromResult(data);
                }),
                new KeyAction<CommandData>(ConsoleKey.Enter, async(data) =>
                {
                    await CommandHelper.RunCommandAsync(data);

                    data.CommandName = null!;
                    data.OptionsData.Index = 0;
                    return data;
                })
            };

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
            if (!IsListening)
                return;

            var currentKey = inputHandler.CurrentPressedKey;
            if (currentKey != (uint)InputKey.UnknownKey && currentKey != (uint)InputKey.Return)
            {
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
