using Castle.Core.Internal;
using System;
using System.IO;

namespace JsonHelper.UserInterface
{
    public class CommandsExecutor : ICommandsExecutor
    {
        private TextWriter writer;
        private readonly ConsoleCommand[] commands;

        public CommandsExecutor(ConsoleCommand[] commands, TextWriter writer)
        {
            this.commands = commands;
            this.writer = writer;
        }

        public string[] GetAvailableCommandName()
            => commands
                .Select(c => c.Name)
                .ToArray();

        public ConsoleCommand FindCommandByName(string name)
            => commands
                .FirstOrDefault(c => string
                .Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));

        public async Task Execute(string[] args)
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
            {
                try
                {
                    await cmd.Execute(args
                    .Skip(1)
                    .Where(arg => !arg.IsNullOrEmpty())
                    .ToArray());
                }
                catch (Exception e)
                {
                    if (!e.Message.IsNullOrEmpty())
                        WriteLineError(e.Message);
                }
            }
        }

        private void WriteLineError(string errorMessage)
        {
            writer.WriteLine("Error!");
            writer.WriteLine($"Error message: {errorMessage}");
        }
    }
}