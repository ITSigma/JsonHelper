using System;
using System.IO;
using System.Threading;

namespace JsonHelper
{
    public class TimerCommand : ConsoleCommand
    {
        private readonly TextWriter writer;

        public TimerCommand(TextWriter writer) : base("timer", "timer <ms>      # starts timer for <ms> milliseconds")
        {
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            if (args.Length != 1)
            {
                writer.WriteLine("Error!");
                return;
            }
            var timeout = TimeSpan.FromMilliseconds(int.Parse(args[0]));
            writer.WriteLine("Waiting for " + timeout);
            Thread.Sleep(timeout);
            writer.WriteLine("Done!");
        }
    }
}