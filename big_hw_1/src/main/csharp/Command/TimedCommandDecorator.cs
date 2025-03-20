using System;
using System.Diagnostics;

namespace BankApplication.Command
{
    public class TimedCommandDecorator : Command
    {
        private readonly Command command;

        public TimedCommandDecorator(Command command)
        {
            this.command = command;
        }

        public void Execute()
        {
            var stopwatch = Stopwatch.StartNew();
            command.Execute();
            stopwatch.Stop();
            Console.WriteLine($"Command execution time: {stopwatch.ElapsedMilliseconds}ms");
        }
    }
} 