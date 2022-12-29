using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.API.CommandOption;
using TeleCommands.NET.API.CommandOption.Interfaces;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Command;

namespace TeleCommands.NET.Example.Commands.SubnetCommand.Options
{
    internal sealed class DataOption : Option<DataOption, bool>
    {
        private char dataSeparator = DefaultDataSeparator;
        public static readonly char DefaultDataSeparator = ',';

        public override ReadOnlyMemory<Argument> Arguments => base.Arguments;
        public override OptionBarrier CharacterBarrier { get; } =
            new('[', ']');

        public IResult<bool> TryGetData([NotNullWhen(true)] out ReadOnlyMemory<string> result, OptionData data) 
        {
            if (data.Data.Length == 0) 
            {
                result = null;
                return new ErrorResult<bool>(false, "No data were found");
            }
            var executionResult = Task.Run(async () => await ExecuteOptionAsync(data)).Result;
            var currentData = data.Data;

            result = Task.Run(async () => await SeparateOptionDataAsync(currentData, dataSeparator)).Result;
            return new SuccesfulResult<bool>(true);
        }

        private async Task<ReadOnlyMemory<string>> SeparateOptionDataAsync(ReadOnlyMemory<char> data, char separator) 
        {
            int index = 0;
            var separateData = new List<string>();
            while (index < data.Length) 
            {
                int currentIndex = await CommandHelper.GetFirstSeparatorIndexAsync(data[index..], separator);
                if (currentIndex == (data.Length - 1)) 
                {
                    var lastData = data.Span[(index)..].ToString();
                    separateData.Add(lastData);
                    return separateData.ToArray();
                }

                int length = index + currentIndex;
                var currentData = data[index..(length)].ToString();

                separateData.Add(currentData);
                index = length + 1;
            }
            return separateData.ToArray();
        }

        public override Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            return base.ExecuteOptionAsync(data);
        }
    }
}
