using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Application
{
    internal class DefaultFileNameBuilder<T> : IFileNameBuilder<T>
    {
        public string BuildName(T value, string fileExtension)
            => $"{typeof(T).Name}_{value}.{fileExtension}";
    }
}
