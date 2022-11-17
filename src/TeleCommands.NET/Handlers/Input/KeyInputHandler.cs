using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.Handlers.Input;

namespace TeleCommands.NET.ConsoleInterface.Handlers.Input
{
    public sealed class KeyInputHandler<T> : InputHandler where T : new()
    {
        private readonly int keyCount = (byte.MaxValue / 2) + 1;
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
                int firstKey = i;
                int lastKey = (keyCount - 1) - i;

                int firstState = ((InteropHelper.GetAsyncKeyState(firstKey) & 1) * firstKey);
                int lastState = ((InteropHelper.GetAsyncKeyState(lastKey) & 1) * lastKey);

                if ((firstState + lastState) > 0)
                {
                    int firstResult = (firstKey) * CalculatePositiveIndex(firstState);
                    int lastResult = (lastKey) * CalculatePositiveIndex(lastState);

                    int finalResult = (firstResult | lastResult);
                    finalResult = (InteropHelper.GetAsyncKeyState(16) & 0x8000) == 0 ? finalResult | 32 : finalResult;
                    return Task.FromResult((uint)(finalResult));
                }
            }
            return Task.FromResult(UnknownKey);
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
