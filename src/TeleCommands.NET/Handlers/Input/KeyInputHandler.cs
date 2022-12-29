using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;
using TeleCommands.NET.Handlers.Enums;

namespace TeleCommands.NET.Handlers.Input
{
    public sealed class KeyInputHandler<T> : InputHandler where T : new()
    {
        private static readonly char emptyCharacter = '\0';
        private static readonly uint[] virtualKeys =
            (typeof(InputKey).GetEnumValues() as uint[])!;

        private static readonly int keyCount = (byte.MaxValue ^ 87);

        public T InvokeObject { get; set; }
        public ReadOnlyMemory<KeyAction<T>> KeyActions { get; set; }

        public KeyInputHandler(Process process, T invokeObject) : base(process)
        {
            InvokeObject = invokeObject;
        }

        protected override async Task OnInputAsync(uint key)
        {
            if (TryGetCurrentKeyAction(out KeyAction<T> action, key))
                InvokeObject = await action.Action.Invoke(InvokeObject);
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        protected override async ValueTask<uint> GetInputAsync()
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

                    if (finalResult != (uint)InputKey.Shift || finalResult != (uint)InputKey.Menu)
                    {
                        bool shiftState = InteropHelper.GetAsyncKeyState((uint)InputKey.Shift) > 0;
                        bool altState = InteropHelper.GetAsyncKeyState((uint)InputKey.Menu) > 0;

                        return ConvertVirtualKey(finalResult, shiftState, altState);
                    }
                }
            }
            await Task.CompletedTask;
            return (uint)InputKey.UnknownKey;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char ConvertVirtualKey(uint key, bool isShift = false, bool isAlt = false) 
        {
            int length = byte.MaxValue;
            uint scanKey = InteropHelper.MapVirtualKeyA(key, 0);

            var keyboardBuffer = new byte[length];
            var stringBuilder = new StringBuilder(1);

            if(isShift)
                keyboardBuffer[(int)(uint)InputKey.Shift] = (byte)InputKey.None;
            if (isAlt) 
            {
                keyboardBuffer[(int)(uint)InputKey.Menu] = (byte)InputKey.None;
                keyboardBuffer[(int)(uint)InputKey.Control] = (byte)InputKey.None;
            }

            InteropHelper.ToAscii(key, scanKey, keyboardBuffer, stringBuilder, 0);
            return stringBuilder.Length == 0 ? emptyCharacter : stringBuilder[0]; 
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

        //TODO: Create a vectorized implementation
        //of this method
        private KeyAction<T> GetCurrentKeyAction(uint key)
        {
            byte keyByte = (byte)key;
            int keyActionsLength = KeyActions.Length;

            for (int i = 0; i < keyActionsLength; i++)
            {
                byte currentByte = (byte)KeyActions.Span[i].Key;
                if (currentByte == keyByte)
                    return KeyActions.Span[i];
            }
            return default;
        }
    }
}
