using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    internal class MinMaxFileNameBuilder<T> : FileNameBuilderBase<IEnumerable<T>>
    {
        private string nameTClass { get => typeof(T).Name; }

        public override string BuildName(string directoryPath, IEnumerable<T> values, string fileExtension)
            => BuildNameDefault(directoryPath, $"{nameTClass}_{values.Min()}-{values.Max()}",
                fileExtension);
    }
}
