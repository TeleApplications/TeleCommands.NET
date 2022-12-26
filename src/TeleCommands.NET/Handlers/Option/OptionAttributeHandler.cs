using System.Diagnostics;
using TeleCommands.NET.Attributes;
using TeleCommands.NET.Command;
using TeleCommands.NET.Command.DataStructures;
using TeleCommands.NET.ConsoleInterface.Interfaces;
using TeleCommands.NET.Interfaces;

namespace TeleCommands.NET.Handlers.Option
{
    public abstract class OptionAttributeHandler<T, TSource> : IHandler where T : IOptionAttribute<TSource>
    {
        private static readonly string optionsPropertyName = "Options";
        private CommandData lastCommandData;
        protected ReadOnlyMemory<TSource> attributeData { get; private set; }

        public IntPtr Handle { get; }

        public OptionAttributeHandler(Process handle)
        {
            Handle = handle.Handle;
        }

        private ReadOnlyMemory<TSource> GetOptionAttributeData(Type commandType) 
        {
            var optionProperty = commandType.GetProperty(optionsPropertyName);
            int optionsLength = CalculateOptionsLength(commandType);

            var attributes = optionProperty!.GetCustomAttributes(typeof(T), true) as T[];
            Memory<TSource> returnData = new TSource[optionsLength];
            for (int i = 0; i < attributes!.Length; i++)
            {
                var currentAttributeData = attributes![i].AttributeData;
                if (currentAttributeData.Length > optionsLength)
                    currentAttributeData = currentAttributeData[0..(optionsLength - 1)];

                currentAttributeData.CopyTo(returnData);
            }
            return returnData;
        }

        private int CalculateOptionsLength(Type commandType) 
        {
            var currentCommand = (ICommand<bool>)Activator.CreateInstance(commandType)!;
            return currentCommand.Options.Length;
        }

        public virtual async Task UpdateAsync() 
        {
            var currentData = CommandReader.CommandData;
            if (!currentData.CommandName.Equals(lastCommandData.CommandName)) 
            {
                string commandName = currentData.CommandName.ToString();
                if(CommandHelper.TryGetCommandAttribute(out CommandAttribute commandAttribute, commandName))
                    attributeData = GetOptionAttributeData(commandAttribute.Type);

                lastCommandData.CommandName = currentData.CommandName;
            }

            int currentIndex = currentData.OptionIndex;
            if (currentIndex > lastCommandData.OptionIndex) 
            {
                var currentAttributeData = attributeData.Span[currentIndex - 1];
                await OnOptionAttributeAsync(currentAttributeData);

                lastCommandData.OptionIndex = currentIndex;
            }
        }

        protected abstract Task OnOptionAttributeAsync(TSource attributeData);
        public virtual Task OnReadAsync() { return Task.CompletedTask; }
    }
}
