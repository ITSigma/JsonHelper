using System;
using System.IO;
using System.Linq;

namespace JsonHelper
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private TextWriter writer;
        private readonly ConsoleCommand[] commands;

        //Атрибут [Named("error")]добавляет зависимость от Ninject контейнера, 
        //поэтому используйте его, если это не критично для вас.
        public CommandsExecutor(ConsoleCommand[] commands, /*[Named("error")]*/TextWriter writer)
        {
            this.commands = commands;
            this.writer = writer;
        }

        public string[] GetAvailableCommandName()
        {
            return commands.Select(c => c.Name).ToArray();
        }
        
        public ConsoleCommand FindCommandByName(string name)
        {
            return commands.FirstOrDefault(c => string.Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        public void Execute(string[] args)
        {
            if (args[0].Length == 0)
            {
                writer.WriteLine("Please specify <command> as the first command line argument");
                return;
            }

            var commandName = args[0];
            var cmd = FindCommandByName(commandName);
            if (cmd == null)
                writer.WriteLine("Sorry. Unknown command {0}", commandName);
            else
                cmd.Execute(args.Skip(1).ToArray());
        }
    }
}