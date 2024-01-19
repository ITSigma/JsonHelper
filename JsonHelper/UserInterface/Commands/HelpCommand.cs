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

        public override void Execute(string[] args)
        {
            if (!CheckArgumentsCount(writer, args))
                return;
            var command = args[0];
            writer.WriteLine(executor.Value.FindCommandByName(command).Help);
        }
    }
}
