using System;
using System.IO;

namespace JsonHelper.UserInterface
{
    public class HelpCommand : ConsoleCommand
    {
        private readonly Lazy<ICommandsExecutor> executor;
        private readonly TextWriter writer;

        public HelpCommand(Lazy<ICommandsExecutor> executor, TextWriter writer)
            : base("h", "h      # prints available commands list")
        {
            this.executor = executor;
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            writer.WriteLine("Available commands: " + string.Join(", ", executor.Value.GetAvailableCommandName()));
        }
    }
}