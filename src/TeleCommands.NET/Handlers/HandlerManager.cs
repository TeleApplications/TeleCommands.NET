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
            var updateTasks = GetHandlersTask(currentHandlers, (IHandler handler) => handler.UpdateAsync());
            var readTasks = GetHandlersTask(currentHandlers, (IHandler handler) => handler.OnReadAsync());

            _ = Task.Run(async () =>
            {
                while (IsRunning)
                { 
                    long currentTicks = DateTime.Now.Ticks;
                    if (currentTicks >= lastTicks + tickDifference)
                    {
                        await Task.WhenAll(updateTasks);
                        lastTicks = currentTicks;
                    }

                    tickDifference = (DateTime.Now.Ticks - currentTicks);
                    await Task.Delay(1);
                }
            });
            
            //TODO: Try to avoid this by creating an "echo" mode for
            //console input reading
            while (IsRunning) { await Task.WhenAll(readTasks); Console.ReadLine(); };
        }

        private IEnumerable<Task> GetHandlersTask(ReadOnlyMemory<IHandler> handlers, Func<IHandler, Task> returnFunction)
        {
            int handlersCount = handlers.Length;
            for (int i = 0; i < handlersCount; i++)
            {
                var currentHandlerTask = returnFunction(handlers.Span[i]);
                yield return currentHandlerTask;
            }
        }

        public void Dispose() { IsRunning = false; }
    }
}
