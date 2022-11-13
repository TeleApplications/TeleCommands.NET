using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.ConsoleInterface.Structs;
using TeleCommands.NET.Structs;

namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public sealed class KeyInputHandler<T> : InputHandler where T : new()
    {
        private const uint WM_KEYDOWN = 0x100;
        private readonly T invokeObject;

        protected override uint InputMessage => 
            WM_KEYDOWN;

        public ConsoleKey CurrentPressedKey { get; private set; }
        public ReadOnlyMemory<KeyAction<T>> KeyActions { get; set; }

        private ConcurrentQueue<string> messageQueue = new();
        private bool isReading = false;

        public KeyInputHandler(Process process, T invokeObject) : base(process)
        {
            this.invokeObject = invokeObject;
        }

        protected override async Task OnInputMessage(InputRecord inputRecord)
        {
            var keyEvent = inputRecord.KeyEvent;
            uint keyCode = (uint)((nint)keyEvent.VirtualKeyCode >> 16) & 0xff;

            char keyCharacter = (char)keyCode;
            CurrentPressedKey = (ConsoleKey)keyCharacter;
            Console.WriteLine($"Pressed:{keyCharacter}");
            //if (TryGetCurrentKeyAction(out KeyAction<T> action, CurrentPressedKey))
                //await action.Action.Invoke(invokeObject);
        }

        private async Task StartWritingAsync() 
        {
            isReading = true;
            await Task.Run(() =>
            {
                while (true)
                {
                    if (messageQueue.TryPeek(out string? result))
                    {
                        Console.WriteLine(result);
                        messageQueue.TryDequeue(out _);
                    }
                }
            });
        }
        private bool TryGetCurrentKeyAction([NotNullWhen(true)] out KeyAction<T> keyAction, ConsoleKey key) 
        {
            var currentKeyAction = GetCurrentKeyAction(key);
            keyAction = currentKeyAction;

            return currentKeyAction.Action is not null;
        }

        private KeyAction<T> GetCurrentKeyAction(ConsoleKey key)
        {
            byte keyByte = (byte)key;
            int keyActionsLength = KeyActions.Length;

            for (int i = 0; i < keyActionsLength; i++)
            {
                byte currentByte = (byte)KeyActions.Span[i].Key;
                if ((keyByte & currentByte) == keyByte)
                    return KeyActions.Span[i];
            }
            return default;
        }
    }
}
