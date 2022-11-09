using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.Command.DataStructures
{
    internal readonly struct CommandExecuteData
    {
        public Type CommandType { get; }
        public ReadOnlyMemory<OptionData> Options { get; }

        public CommandExecuteData(Type commandType, ReadOnlyMemory<OptionData> options)
        {
            CommandType = commandType;
            Options = options;
        }
    }
}
