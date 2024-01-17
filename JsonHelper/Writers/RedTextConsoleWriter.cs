using System;
using System.IO;
using System.Text;

namespace JsonHelper
{
    public class RedTextConsoleWriter : TextWriter
    {
        public override void Write(char value)
        {
            var prev = Console.ForegroundColor;

            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.Write(value);
            }
            finally
            {
                Console.ForegroundColor = prev;
            }
        }

        public override Encoding Encoding => Console.Out.Encoding;
    }
}