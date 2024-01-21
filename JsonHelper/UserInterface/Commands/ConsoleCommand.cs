using Castle.Core.Internal;

namespace JsonHelper.UserInterface
{
    public abstract class ConsoleCommand
    {
        protected ConsoleCommand(string name, string help, params int[] availibleArgsCount)
        {
            Name = name;
            Help = help;
            if (availibleArgsCount.IsNullOrEmpty())
                throw new ArgumentException("Please provide availible argumets count");
            this.availibleArgsCount = new HashSet<int>(availibleArgsCount);
        }

        public string Name { get; }
        public string Help { get; }
        protected HashSet<int> availibleArgsCount { get; }

        public abstract Task Execute(string[] args);

        protected void CheckArgumentsCount(string[] args)
        {
            if (!availibleArgsCount.Contains(args.Length))
                throw new ArgumentException($"Except one of [ {string.Join(", ", availibleArgsCount)} ] " +
                    $"argument's count but {args.Length} was given.");
        }
    }
}