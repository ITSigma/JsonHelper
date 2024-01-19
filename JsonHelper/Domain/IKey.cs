using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Domain
{
    internal interface IKey
    {
        public static string KeysFilePath { get; }
        public string Key { get; }
    }
}
