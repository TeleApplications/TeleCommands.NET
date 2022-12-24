using System.Reflection;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Handlers.Option.Attributes;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Handlers.Option
{
    internal abstract class OptionAttributeHandler<T, TSource> : IHandler where T : OptionAttribute<TSource>
    {
        private static readonly string optionsPropertyName = "Options";
        protected ReadOnlyMemory<TSource> attributeData { get; private set; }
        private CommandData currentData;

        public IntPtr Handle { get; }

        public OptionAttributeHandler(IntPtr handle, CommandData commandData)
        {
            Handle = handle;
            currentData = commandData;
        }

        private ReadOnlyMemory<TSource> GetOptionAttributeData(Type commandType) 
        {
            var optionProperty = commandType.GetProperty(optionsPropertyName);
            int optionsLength = CalculateOptionsLength(optionProperty!, commandType);

            var attributes = (T[])optionProperty!.GetCustomAttributes(typeof(T), true);
            int attributesLength = attributes.Length;

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
        }

        public virtual Task OnReadAsync() { return Task.CompletedTask; }
    }
}
