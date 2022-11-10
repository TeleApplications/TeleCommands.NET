using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET
{
    public static class CommandHelper
    {
        private static ImmutableArray<CommandAttribute> commandAttributes =
            ImmutableArray.CreateRange
            (
                Assembly.GetExecutingAssembly().GetCustomAttributes<CommandAttribute>()
            );

        public static async Task RunCommandAsync(CommandData commandData) 
        {
            if (!TryGetCommandAttribute(out CommandAttribute attribute, commandData.CommandName))
                throw new Exception("Command was not found");

            var commandInstance = (ICommand<bool>)Activator.CreateInstance(attribute.Type)!;
            var options = commandData.OptionsData;

            var data = options.Memory[0..options.Index];
            ReadOnlyMemory<OptionData> optionsdata = await SeparateOptionsAsync(data, commandInstance);

            await commandInstance.ExecuteCommandAsync(optionsdata);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static async Task<ReadOnlyMemory<OptionData>> SeparateOptionsAsync(ReadOnlyMemory<char> commandData, ICommand<bool> command)
        {
            var optionsData = new List<OptionData>();
            int lastIndex = 0;

            var commandOptions = command.Options;
            for (int i = 0; i < commandOptions.Length; i++)
            {
                if (lastIndex == commandData.Length)
                    return optionsData.ToArray();

                ReadOnlyMemory<char> argument;
                ReadOnlyMemory<char> optionData;

                var optionBarrier = commandOptions[i].CharacterBarrier;
                //Also this should be done, by just creating a simple reference method
                //however I don't found it necessary. It's possible that this implementation
                //will be changed
                int firstSeparatorIndex = await GetFirstSeparatorIndexAsync(commandData[lastIndex..], optionBarrier.Start);
                argument = commandData[lastIndex..(firstSeparatorIndex)];
                lastIndex = firstSeparatorIndex;

                int secondSeparatorIndex = await GetFirstSeparatorIndexAsync(commandData[lastIndex..], optionBarrier.End);
                optionData = commandData[lastIndex..(secondSeparatorIndex)];
                lastIndex = secondSeparatorIndex;

                var currentData = new OptionData(argument, optionData);
                optionsData.Add(currentData);
            }
            return optionsData.ToArray();
        }

        private static async Task<int> GetFirstSeparatorIndexAsync(ReadOnlyMemory<char> sequence, char separator) 
        {
            int index = 0;

            await Task.Run(() => 
            {
                char currentChar = sequence.Span[index];
                while (currentChar != separator) { index++; }
            });
            return index;
        }

        //TODO: Create vertorized type of this method
        private static bool TryGetCommandAttribute(out CommandAttribute attribute, string commandName) 
        {
            int commandCount = commandAttributes.Length;
            for (int i = 0; i < commandCount; i++)
            {
                var currentAttribute = commandAttributes[i];
                if (currentAttribute.Name == commandName) 
                {
                    attribute = currentAttribute;
                    return true;
                }
            }

            attribute = null!;
            return false;
        }
    }
}
