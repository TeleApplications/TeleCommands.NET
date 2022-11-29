using System.Diagnostics.CodeAnalysis;
using TeleCommands.NET.API.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption.Results;

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
                if (currentIndex == 0) 
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
