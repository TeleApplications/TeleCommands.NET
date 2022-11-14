using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.Handlers.Input;

namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public sealed class KeyInputHandler<T> : InputHandler where T : new()
    {
        private readonly int keyCount = byte.MaxValue + 1;
        private readonly T invokeObject;

        public uint CurrentPressedKey { get; private set; }
        public ReadOnlyMemory<KeyAction<T>> KeyActions { get; set; }

        public KeyInputHandler(Process process, T invokeObject) : base(process)
        {
            this.invokeObject = invokeObject;
        }

        protected override async Task OnInputAsync(uint key)
        {
            if (TryGetCurrentKeyAction(out KeyAction<T> action, key))
                await action.Action.Invoke(invokeObject);
            CurrentPressedKey = key;
            Debug.Write($"{CurrentPressedKey}");
        }

        protected override ValueTask<uint> GetInputAsync()
        {
            int halfKeyCount = (keyCount) / 2;
            for (int i = 0; i < halfKeyCount; i++)
            {
                int firstState = ((InteropHelper.GetAsyncKeyState(i)) * i);

                int lastKey = (keyCount - 1) - i;
                int lastState = ((InteropHelper.GetAsyncKeyState(lastKey)) * lastKey);

                if ((firstState + lastState) > 0)
                    return new ValueTask<uint>((uint)((i * CalculateReverseIndex(firstState)) | (lastKey * CalculateReverseIndex(lastState))));
            }
            return new ValueTask<uint>(UnknownKey);
        }

        private int CalculateReverseIndex(int value) 
        {
            int indexValue = (value + Math.Abs(value));
            int currentIndex = indexValue / (1) + ((Math.Abs(indexValue - 1) | 0));

            int deriveValue = (value + ((Math.Abs(currentIndex)) ^ 1));

            return deriveValue;
        }

        private bool TryGetCurrentKeyAction([NotNullWhen(true)] out KeyAction<T> keyAction, uint key) 
        {
            var currentKeyAction = GetCurrentKeyAction(key);
            keyAction = currentKeyAction;

            return currentKeyAction.Action is not null;
        }

        private KeyAction<T> GetCurrentKeyAction(uint key)
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
