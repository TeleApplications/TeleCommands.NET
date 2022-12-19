using TeleCommands.NET.ConsoleInterface.Interfaces;

namespace TeleCommands.NET.Handlers
{
    public sealed class HandlerManager : IDisposable
    { 
        public bool IsRunning { get; private set; } = true;

        private ReadOnlyMemory<IHandler> currentHandlers;
        private long lastTicks = 0;
        private long tickDifference = 0;

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
                    long currentTicks = DateTime.Now.Ticks;
                    if (currentTicks >= lastTicks + tickDifference)
                    {
                        await Task.WhenAll(handlersTask);
                        lastTicks = currentTicks;
                    }

                    tickDifference = (DateTime.Now.Ticks - currentTicks);
                    await Task.Delay(1);
                }
            });

            //TODO: Try to avoid this by creating an "echo" mode for
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
