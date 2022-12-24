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
        public static readonly char defaultCommandSymbol = '>';
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
        public CommandData CommandData { get; }

        public bool IsListening { get; set; } = true;
        public IntPtr Handle { get; }

        //TODO: This will be also changed, due to creating
        //a better implementation of writing to the console
        //buffer
        public Action OnReadAction { get; set; } = () =>
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{defaultCommandSymbol} ");
            Console.ForegroundColor = ConsoleColor.White;
        };

        public CommandReader(Process process, int maxCommandLength)
        {
            CommandData = new()
            {
                OptionsData = new IndexMemory<char>(maxCommandLength)
            };

            inputHandler = new(process, CommandData);
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
                var optionsData = CommandData.OptionsData;
                optionsData.Memory.Span[optionsData.Index] = (char)currentKey;
                optionsData.Index++;
            }
            await inputHandler.UpdateAsync();
        }

        public async Task OnReadAsync() =>
            OnReadAction.Invoke();
        public void Dispose() =>
            IsListening = false;
    }
}
