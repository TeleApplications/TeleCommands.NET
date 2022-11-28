using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.Options
{
    internal sealed class DataOption : Option<DataOption, bool>
    {
        public static readonly char DefaultDataSeparator = ',';
        private char dataSeparator = DefaultDataSeparator;

        public override ReadOnlyMemory<Argument> Arguments => base.Arguments;
        public override OptionBarrier CharacterBarrier { get; } =
            new('[', ']');

        public IResult<bool> TryGetData([NotNullWhen(true)] out ReadOnlyMemory<string> result, OptionData data) 
        {
            var executionResult = Task.Run(async () => await ExecuteOptionAsync(data)).Result;
            if(executionResult.Value)

        }

        public override Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            return base.ExecuteOptionAsync(data);
        }
    }
}
