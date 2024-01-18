using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper.Application
{
    internal class MinMaxFileNameBuilder<T> : IFileNameBuilder<IEnumerable<T>>
    {
        private string nameTClass { get => typeof(T).Name; }

        public string BuildName(IEnumerable<T> values, string fileExtension)
            => BuildName(values, fileExtension, value => value);

        public string BuildName<TOut>(IEnumerable<T> values, string fileExtension, 
            Func<T, TOut> minMaxArgFunc)
            => $"{nameTClass}_{values.MinBy(minMaxArgFunc)}-{values.MaxBy(minMaxArgFunc)}.{fileExtension}";
    }
}
