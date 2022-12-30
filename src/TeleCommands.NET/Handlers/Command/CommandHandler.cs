using TeleCommands.NET.API.Attributes;
using TeleCommands.NET.Command;

namespace TeleCommands.NET.Handlers.Command
{
    public abstract class CommandHandler : IHandler
    {
        protected static ReadOnlyMemory<CommandAttribute> commandAttributes { get; private set; }
        private Memory<char> lastCommandMemory;

        public IntPtr Handle { get; }

        public CommandHandler(Process process)
        {
            Handle = process.Handle;
            if (commandAttributes.Length == 0)
                commandAttributes = CommandHelper.CommandAttributes.ToArray();
        }

        public virtual async Task UpdateAsync() 
        {
            var currentCommandData = CommandReader.CommandData;
            if (currentCommandData.CommandName.Length > 0)
                return;

            var optionData = currentCommandData.OptionsData;
            var currentOptionData = optionData.Memory[0..(optionData.Index)];

            //Maybe I should compare them by their length
            if (!(currentOptionData.Equals(lastCommandMemory))) 
            {
                int optionDataLength = currentOptionData.Length;
                //TODO: Change it to normal array, instead of
                //creating new instance of memory
                lastCommandMemory = new char[optionDataLength];
                currentOptionData.CopyTo(lastCommandMemory);

                await OnWriteAsync(currentOptionData);
            }
        }

        protected abstract Task OnWriteAsync(ReadOnlyMemory<char> data);

        public async Task OnReadAsync() 
        {
            await Task.CompletedTask; 
        }
    }
}
