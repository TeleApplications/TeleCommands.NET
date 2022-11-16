
namespace TeleCommands.NET.Handlers.Input
{
    public readonly struct KeyActivationHolder
    {
        public uint KeyCode { get; }
        public int LastActivation { get; }

        public KeyActivationHolder(uint keyCode, int lastActivation) 
        {
            KeyCode = keyCode;
            LastActivation = lastActivation;
        }

        public static KeyActivationHolder CreateCurrentActivation(uint keyCode) =>
            new(keyCode, ((byte)DateTime.Now.Second));
    }
}
