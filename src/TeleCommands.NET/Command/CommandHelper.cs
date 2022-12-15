using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.CompilerServices;
using TeleCommands.NET.API.CommandOption.Results;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.CommandOption.OptionStructs;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET
{
    public static class CommandHelper
    {
        private static readonly CommandResult unknownCommandResult =
            new(null) { Message = "Command was not found" };
        private static ImmutableArray<CommandAttribute> commandAttributes =
            ImmutableArray.CreateRange(GetCommandAttributes(AppDomain.CurrentDomain.GetAssemblies()));

        public static async Task<CommandResult> RunCommandAsync(CommandData commandData) 
        {
            if (!TryGetCommandAttribute(out CommandAttribute attribute, commandData.CommandName))
                return unknownCommandResult;

            var commandInstance = (ICommand<bool>)Activator.CreateInstance(attribute.Type)!;
            var options = commandData.OptionsData;

            var data = options.Memory[0..options.Index];
            if(data.Length == 0)
                return await commandInstance.ExecuteCommandAsync(null);

            ReadOnlyMemory<OptionData> optionsdata = await SeparateOptionsAsync(data, commandInstance);
            return await commandInstance.ExecuteCommandAsync(optionsdata);
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
                lastIndex++;

                ReadOnlyMemory<char> argument;
                ReadOnlyMemory<char> optionData;

                var optionBarrier = commandOptions[i].CharacterBarrier;
                //Also this should be done, by just creating a simple reference method
                //however I don't found it necessary. It's possible that this implementation
                //will be changed
                int firstSeparatorIndex = await GetFirstSeparatorIndexAsync(commandData[(lastIndex)..], optionBarrier.Start);
                argument = commandData[(lastIndex)..((lastIndex + firstSeparatorIndex))];
                lastIndex = firstSeparatorIndex + 2;

                int secondSeparatorIndex = await GetFirstSeparatorIndexAsync(commandData[(lastIndex)..], optionBarrier.End);
                optionData = commandData[(lastIndex)..(secondSeparatorIndex + lastIndex)];
                lastIndex += secondSeparatorIndex;

                var currentData = new OptionData(argument, optionData);
                optionsData.Add(currentData);
            }
            return optionsData.ToArray();
        }

        public static async Task<int> GetFirstSeparatorIndexAsync(ReadOnlyMemory<char> sequence, char separator) 
        {
            int sequanceLength = sequence.Length;
            int index = 0;

            await Task.Run(() =>
            {
                while (sequence.Span[index] != separator && (sequanceLength - 1) != (index)) { index++; }
            });
            return index;
        }

        //TODO: Create vertorized type of this method
        public static bool TryGetCommandAttribute(out CommandAttribute attribute, string commandName) 
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

        private static IEnumerable<CommandAttribute> GetCommandAttributes(ReadOnlyMemory<Assembly> assemblies) 
        {
            var attributesTypes = GetCommandAttributesTypes(assemblies).ToArray();

            for (int i = 0; i < attributesTypes.Length; i++)
            {
                var attributes = attributesTypes[i].GetCustomAttributes<CommandAttribute>().ToArray();
                for (int j = 0; j < attributes.Length; j++)
                {
                    yield return attributes[j];
                }
            }
        }

        private static IEnumerable<Type> GetCommandAttributesTypes(ReadOnlyMemory<Assembly> assemblies) 
        {
            int assembliesCount = assemblies.Length;
            for (int i = 0; i < assembliesCount; i++)
            {
                var currentAssembly = assemblies.Span[i];

                var assemblyTypes = currentAssembly.GetTypes();
                for (int j = 0; j < assemblyTypes.Length; j++)
                {
                    if (assemblyTypes[j].GetCustomAttributes<CommandAttribute>().ToArray().Length > 0)
                        yield return assemblyTypes[j];
                }
            }
        }
    }
}
