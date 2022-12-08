using TeleCommands.NET.ConsoleInterface.Interfaces;

namespace TeleCommands.NET.Handlers
{
    public sealed class HandlerManager : IDisposable
    {
        public bool IsRunning { get; private set; } = true;

        private ReadOnlyMemory<IHandler> currentHandlers;
        private int lastMilliseconds = 0;

        public HandlerManager(IHandler[] handlers) 
        {
            currentHandlers = handlers;
        }

        public async Task StartHandlersUpdateAsync() 
        {
            var handlersTask = GetHandlersTask(currentHandlers);
            _ = Task.Run(async () =>
            {
                while (IsRunning)
                {
                    int currentMillisecond = DateTime.Now.Millisecond;
                    if (currentMillisecond > lastMilliseconds + 1000)
                        IsRunning = false;

                    await Task.WhenAll(handlersTask);
                    lastMilliseconds = currentMillisecond;
                    await Task.Delay(1);
                }
            });

            //TODO: Try to avoid this type of creating "echo" mode for
            //console input reading
            while (IsRunning) { Console.ReadLine(); };
        }

        private IEnumerable<Task> GetHandlersTask(ReadOnlyMemory<IHandler> handlers)
        {
            int handlersCount = handlers.Length;
            for (int i = 0; i < handlersCount; i++)
            {
                var currentHandlerTask = handlers.Span[i].UpdateAsync();
                yield return currentHandlerTask;
            }
        }

        public void Dispose() { IsRunning = false; }
    }
}
