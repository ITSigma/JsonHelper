using System;
using System.IO;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace JsonHelper
{
    public class CommandsModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind(f =>
            {
                var commandTypes = f.FromThisAssembly().Select(typeof(ConsoleCommand).IsAssignableFrom);
                commandTypes.BindAllBaseClasses().Configure(c => c.InSingletonScope());
            });
            Kernel.Bind<ICommandsExecutor>().To<CommandsExecutor>().InSingletonScope();
            Kernel.Bind<TextWriter>().To<PromptConsoleWriter>()
                .WhenInjectedInto<ConsoleCommand>().InSingletonScope();
            Kernel.Bind<TextWriter>().To<RedTextConsoleWriter>()
                .WhenInjectedInto<CommandsExecutor>().InSingletonScope();
        }
    }

    public class Program
    {
        private static ICommandsExecutor CreateExecutor()
        {
            var container = new StandardKernel(new CommandsModule());
            return container.Get<ICommandsExecutor>();
        }

        static void Main(string[] args)
        {
            ICommandsExecutor executor = CreateExecutor();
            if (args.Length > 0)
                executor.Execute(args);
            else
                RunInteractiveMode(executor);
        }

        public static void RunInteractiveMode(ICommandsExecutor executor)
        {
            while (true)
            {
                var line = Console.ReadLine();
                if (line == null || line == "exit") return;
                executor.Execute(line.Split(' '));
            }
        }
    }
}