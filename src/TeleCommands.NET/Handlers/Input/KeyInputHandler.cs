﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using TeleCommands.NET.ConsoleInterface.Structs;

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

        public KeyInputHandler(Process process, T invokeObject) : base(process)
        {
            this.invokeObject = invokeObject;
        }

        protected override void OnHookProc(uint wParam, uint lParam)
        {
            var message = Marshal.PtrToStructure<Message>((IntPtr)lParam);
            CurrentPressedKey = (ConsoleKey)message.LParam;
            if (TryGetCurrentKeyAction(out KeyAction<T> action, CurrentPressedKey))
                Task.Run(async() => await action.Action.Invoke(invokeObject));
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