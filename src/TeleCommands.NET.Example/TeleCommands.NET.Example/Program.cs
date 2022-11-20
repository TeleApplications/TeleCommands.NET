using System.Diagnostics;
using TeleCommands.NET.Command;

bool isRunning = true;
int lastMillisecond = 0;
var currentProcess = Process.GetCurrentProcess();
var commandReader = new CommandReader(currentProcess, 128);

Task.Run(async () =>
{
    while (isRunning)
    {
        int currentMillisecond = DateTime.Now.Millisecond;
        if (currentMillisecond > lastMillisecond + 1000)
            isRunning = false;

        await commandReader.UpdateAsync();
        lastMillisecond = currentMillisecond;

        await Task.Delay(1);
    }
});
while (isRunning) { Console.ReadLine(); };
