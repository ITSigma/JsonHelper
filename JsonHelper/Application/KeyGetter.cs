using JsonHelper.Domain;
using JsonHelper.Infrastructure;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;

namespace JsonHelper.Application
{
    internal static class KeyGetter<T>
        where T: IKey
    {
        private static int keyPointer { get; set; }
        private static int maxKeyPointer => apiKeys.Count;
        private static readonly List<T> apiKeys;
        private static readonly object apiKeyLock = new();

        static KeyGetter()
        {
            var filePath = typeof(T)
                .GetProperty("KeysFilePath", BindingFlags.Public 
                    | BindingFlags.Static 
                    | BindingFlags.FlattenHierarchy)?
                .GetValue(null);
            if (filePath is null)
                throw new NullReferenceException($"KeysFilePath in {typeof(T).Name} is null.");
            apiKeys = Task.Run(() => FileReader<List<T>>
                .ReadFromFileAsync((string)filePath))
                .Result;
            if (apiKeys.IsNullOrEmpty())
                throw new NullReferenceException($"There is no any keys in {filePath}.");
        }

        public static string GetNextKey()
        {
            lock (apiKeyLock)
            {
                var key = apiKeys[keyPointer];
                keyPointer = keyPointer++ % maxKeyPointer;
                return key.Key;
            }
        }
    }
}
