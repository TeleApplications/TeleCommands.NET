using System.Diagnostics;
using System.Reflection;
using TeleCommands.NET.Command;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Handlers.Option
{
    public abstract class OptionAttributeHandler<T, TSource> : IHandler where T : IOptionAttribute<TSource>
    {
        private static readonly string optionsPropertyName = "Options";
        protected ReadOnlyMemory<TSource> attributeData { get; private set; }

        public IntPtr Handle { get; }

        public OptionAttributeHandler(Process handle)
        {
            Handle = handle.Handle;
        }

        private ReadOnlyMemory<TSource> GetOptionAttributeData(Type commandType) 
        {
            var optionProperty = commandType.GetProperty(optionsPropertyName);
            int optionsLength = CalculateOptionsLength(optionProperty!, commandType);

            var attributes = optionProperty!.GetCustomAttributes(typeof(T), true) as T[];
            int attributesLength = attributes!.Length;

            Memory<TSource> returnData = new TSource[optionsLength * attributesLength];
            for (int i = 0; i < attributesLength; i++)
            {
                var currentAttributeData = attributes[i].AttributeData;
                if (currentAttributeData.Length > attributesLength)
                    currentAttributeData = currentAttributeData[0..(attributesLength)];

                currentAttributeData.CopyTo(returnData);
            }
            return returnData;
        }

        private int CalculateOptionsLength(PropertyInfo property, Type commandType) 
        {
            var currentCommand = (ICommand<bool>)Activator.CreateInstance(commandType)!;
            var currentOptions = (IOption<bool>[])property.GetValue(currentCommand)!;

            return currentOptions.Length;
        }

        public virtual async Task UpdateAsync() 
        {
            string commandName = CommandReader.CommandData.CommandName.ToString();
            if(commandName != string.Empty)
                Console.Title = commandName;
        }

        public virtual Task OnReadAsync() { return Task.CompletedTask; }
    }
}
