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
        {
            return commands.Select(c => c.Name).ToArray();
        }
        
        public ConsoleCommand FindCommandByName(string name)
        {
            return commands
                .FirstOrDefault(c => string
                .Equals(c.Name, name, StringComparison.OrdinalIgnoreCase));
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