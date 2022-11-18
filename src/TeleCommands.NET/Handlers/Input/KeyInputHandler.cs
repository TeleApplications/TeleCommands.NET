using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using TeleCommands.NET.Handlers.Enums;
using TeleCommands.NET.Handlers.Input;

namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public sealed class KeyInputHandler<T> : InputHandler where T : new()
    {
        private static readonly uint[] virtualKeys =
            (typeof(InputKey).GetEnumValues() as uint[])!;

        private readonly int keyCount = (byte.MaxValue ^ 84);
        private T invokeObject;

        public KeyActivationHolder KeyInformations { get; private set; }
        public ReadOnlyMemory<KeyAction<T>> KeyActions { get; set; }

        public KeyInputHandler(Process process, T invokeObject) : base(process)
        {
            this.invokeObject = invokeObject;
        }

        protected override async Task OnInputAsync(uint key)
        {
            if (TryGetCurrentKeyAction(out KeyAction<T> action, key))
                await action.Action.Invoke(invokeObject);
            KeyInformations = KeyActivationHolder.CreateCurrentActivation(key);
        }

        protected override Task<uint> GetInputAsync()
        {
            int halfKeyCount = (keyCount) / 2;

            for (int i = 0; i < halfKeyCount; i++)
            {
                uint firstKey = virtualKeys[i];
                int lastIndex = (keyCount - 1) - i;
                uint lastKey = virtualKeys[lastIndex];

                uint firstState = ((InteropHelper.GetAsyncKeyState(firstKey) & 1) * (uint)i);
                uint lastState = ((InteropHelper.GetAsyncKeyState(lastKey) & 1) * (uint)lastIndex);

                if ((firstState + lastState) > 0)
                {
                    uint firstResult = (firstKey) * (uint)CalculatePositiveIndex((int)firstState);
                    uint lastResult = (lastKey) * (uint)CalculatePositiveIndex((int)lastState);

                    uint finalResult = (firstResult | lastResult);
                    return Task.FromResult(uint.Parse(ConvertVirtualKey(finalResult)));
                }
            }
            return Task.FromResult((uint)InputKey.UnknownKey);
        }

        private string ConvertVirtualKey(uint key, bool isShift = false) 
        {
            int length = byte.MaxValue;
            var keyboardBuffer = new byte[length];
            var stringBuilder = new StringBuilder(length);

            if(isShift)
                keyboardBuffer[13] = 0xff;
            InteropHelper.ToUnicode(key, 0, keyboardBuffer, out stringBuilder, length - 1, 0);
            return stringBuilder.ToString();
        }

        private int CalculatePositiveIndex(int value)  
        {
            int indexValue = (value + Math.Abs(value));
            int devideValue = 1 + (Math.Abs((indexValue * 1) - 1));
            return indexValue / devideValue;
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
