using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.UserInterface
{
    public class HelpCommand : ConsoleCommand
    {
        private readonly Lazy<ICommandsExecutor> executor;
        private readonly TextWriter writer;

        public HelpCommand(Lazy<ICommandsExecutor> executor, TextWriter writer)
            : base("help", "help <command>      # prints help for command", 1)
        {
            this.executor = executor;
            this.writer = writer;
        }

        public async override Task Execute(string[] args)
        {
            CheckArgumentsCount(args);
            var cmd = executor.Value.FindCommandByName(args[0]);
            if (cmd == null)
                throw new ArgumentException($"Sorry. Unknown command {args[0]}");
            else
                writer.WriteLine(cmd.Help);

        }
    }
}
