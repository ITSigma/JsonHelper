using System;
using System.IO;
using System.Text;

namespace JsonHelper
{
    public class PromptConsoleWriter : TextWriter
    {
        public override void WriteLine(string s)
        {
            Console.Out.WriteLine("> " + s);
        }

        public override Encoding Encoding => Console.Out.Encoding;
    }
}