using JsonHelper.Domain;
using JsonHelper.Infrastructure;
using Castle.Core.Internal;

namespace JsonHelper.Application
{
    internal class KeyGetter
    {
        public int Count => apiKeys.Count;
        private readonly Queue<string> apiKeys;
        private readonly object apiKeyLock = new();

        public KeyGetter(string keyFilePath)
        {
            apiKeys = Task.Run(() => FileReader<Queue<string>>
                .ReadFromFileAsync(keyFilePath))
                .Result;
            if (apiKeys is null || apiKeys.Count == 0)
                throw new NullReferenceException($"There is no any keys in {keyFilePath}.");
        }

        public string GetNextKey()
        {
            lock (apiKeyLock)
            {
                var key = apiKeys.Dequeue();
                apiKeys.Enqueue(key);
                return key;
            }
        }
    }
}
