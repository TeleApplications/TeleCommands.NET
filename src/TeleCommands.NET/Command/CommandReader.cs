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
        //I'am still not sure about this type of implementation,
        //it's possible that this will be changed
        public static CommandData CommandData { get; private set; }

        private ReadOnlyMemory<KeyAction<CommandData>> keyActions =
            new KeyAction<CommandData>[]
            {
                new KeyAction<CommandData>(ConsoleKey.Spacebar, (data) =>
                {
                    if(data.CommandName.Length == 0)
                    {
                        int index = data.OptionsData.Index;

                        var currentMemory = data.OptionsData.Memory[0..(index)];
                        data.CommandName = new char[currentMemory.Length];
                        currentMemory.CopyTo(data.CommandName);

                        data.OptionsData.Index = 0;
                    }
                    return Task.FromResult(data);
                }),
                new KeyAction<CommandData>(ConsoleKey.Enter, async(data) =>
                {
                    await CommandHelper.RunCommandAsync(data);

                    data.CommandName = new char[0];
                    data.OptionsData.Index = 0;
                    return data;
                })
            };

        private KeyInputHandler<CommandData> inputHandler;
        public CommandData indexCommandData;

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
            indexCommandData = new()
            {
                OptionsData = new IndexMemory<char>(maxCommandLength)
            };

            inputHandler = new(process, indexCommandData);
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
                var optionsData = indexCommandData.OptionsData;
                optionsData.Memory.Span[optionsData.Index] = (char)currentKey;
                optionsData.Index++;
            }

            await inputHandler.UpdateAsync();
            CommandData = inputHandler.InvokeObject;
        }

        public async Task OnReadAsync() =>
            OnReadAction.Invoke();
        public void Dispose() =>
            IsListening = false;
    }
}
