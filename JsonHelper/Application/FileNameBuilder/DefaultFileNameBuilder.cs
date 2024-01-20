using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Application
{
    internal class DefaultFileNameBuilder<T> : FileNameBuilderBase<T>
    {
        public DefaultFileNameBuilder(Func<T, string> valueToNameFunc, string label) 
            : base(valueToNameFunc, label)
        {
        }

        public override string BuildName(T value, string fileExtension)
            => BuildNameDefault(value, fileExtension);
    }
}
