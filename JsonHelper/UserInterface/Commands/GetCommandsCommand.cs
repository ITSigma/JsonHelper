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

        public async override Task Execute(string[] args)
        {
            CheckArgumentsCount(args);
            var commands = executor.Value.GetAvailableCommandName();
            writer.WriteLine("Available commands: " + string.Join(", ", commands));
        }
    }
}