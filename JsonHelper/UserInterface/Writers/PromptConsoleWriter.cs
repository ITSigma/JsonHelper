using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace JsonHelper.UserInterface
{
    public class PromptConsoleWriter : TextWriter
    {
        public override void WriteLine(string s)
        {
            ChangeConsoleColor(() => Console.Out.WriteLine(s));
        }

        public override void Write(string s)
        {
            ChangeConsoleColor(() => Console.Out.Write(s));
        }

        private void ChangeConsoleColor(Action consoleAction)
        {
            var prev = Console.ForegroundColor;

            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                consoleAction();
            }
            finally
            {
                Console.ForegroundColor = prev;
            }
        }

        public override Encoding Encoding => Console.Out.Encoding;
    }
}