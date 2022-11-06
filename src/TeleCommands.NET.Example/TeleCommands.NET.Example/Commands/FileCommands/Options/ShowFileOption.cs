using System.Collections.Immutable;
using TeleCommands.NET.CommandOption;
using TeleCommands.NET.CommandOption.Interfaces;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.CommandOption.Results;

namespace TeleCommands.NET.Example.Commands.FileCommands.Options
{
    internal sealed class ShowFileOption : Option<ShowFileOption, bool>
    {
        private string ?filePath;

        public override ReadOnlyMemory<Argument> Arguments =>
            new Argument[]
            {
                new Argument('l', async() =>
                {
                    var lines = await File.ReadAllLinesAsync(filePath!);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        Console.WriteLine(lines[i]);
                    }
                }),
                new Argument('b', async() =>
                {
                    var lines = await File.ReadAllBytesAsync(filePath!);

                    for (int i = 0; i < lines.Length; i++)
                    {
                        Console.WriteLine(lines[i]);
                    }
                })
            };

        public override async Task<IResult<bool>> ExecuteOptionAsync(OptionData data)
        {
            filePath = data.Data.ToString();
            if (!File.Exists(filePath))
                return new ErrorResult<bool>(false, "File doesn't exist");

            return await base.ExecuteOptionAsync(data);
        }
    }
}
