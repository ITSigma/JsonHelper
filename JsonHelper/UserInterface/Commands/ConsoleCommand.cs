namespace JsonHelper.UserInterface
{
    public abstract class ConsoleCommand
    {
        protected ConsoleCommand(string name, string help, int argsCount)
        {
            Name = name;
            Help = help;
            this.argsCount = argsCount;
        }

        public string Name { get; }
        public string Help { get; }
        protected int argsCount { get; }

        public abstract void Execute(string[] args);

        protected bool CheckArgumentsCount(TextWriter writer, string[] args)
        {
            if (args.Length != argsCount)
            {
                writer.WriteLine("Error!");
                writer.WriteLine($"Error message: except {argsCount} arguments but {args.Length} was given.");
                return false;
            }
            return true;
        }
    }
}