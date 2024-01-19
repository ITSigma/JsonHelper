using System;
using System.IO;

namespace JsonHelper.UserInterface
{
    public class HelpCommand : ConsoleCommand
    {
        private readonly Lazy<ICommandsExecutor> executor;
        private readonly TextWriter writer;

        public HelpCommand(Lazy<ICommandsExecutor> executor, TextWriter writer)
            : base("h", "h      # prints available commands list", 0)
        {
            this.executor = executor;
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            CheckArgumentsCount(writer, args);
            var commands = executor.Value.GetAvailableCommandName();
            writer.WriteLine("Available commands: " + string.Join(", ", commands));
            foreach (var command in commands)
                writer.WriteLine(executor.Value.FindCommandByName(command).Help);
        }
    }
}