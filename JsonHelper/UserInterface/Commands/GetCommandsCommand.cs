using System;
using System.IO;

namespace JsonHelper.UserInterface
{
    public class GetCommandsCommand : ConsoleCommand
    {
        private readonly Lazy<ICommandsExecutor> executor;
        private readonly TextWriter writer;

        public GetCommandsCommand(Lazy<ICommandsExecutor> executor, TextWriter writer)
            : base("commands", "commands      # prints available commands list", 0)
        {
            this.executor = executor;
            this.writer = writer;
        }

        public override void Execute(string[] args)
        {
            if (!CheckArgumentsCount(writer, args))
                return;
            var commands = executor.Value.GetAvailableCommandName();
            writer.WriteLine("Available commands: " + string.Join(", ", commands));
        }
    }
}