using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    internal interface IApplicationCommand<T>
    {
        public void Execute(T arg);
    }
}
