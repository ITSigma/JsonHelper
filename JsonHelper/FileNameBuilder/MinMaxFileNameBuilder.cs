using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    internal class MinMaxFileNameBuilder<T> : IFileNameBuilder<IEnumerable<T>>
    {
        private string nameTClass { get => typeof(T).Name; }

        public string BuildName(IEnumerable<T> values, string fileExtension)
            => $"{nameTClass}_{values.Min()}-{values.Max()}.{fileExtension}";
    }
}
