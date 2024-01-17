using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonHelper
{
    internal class FileReader<T>
    {
        private const int BufferSize = 8192;
        public static async Task<T> ReadFromFileAsync(string filePath, bool seekBegin = false)
        {
            T? deserializedValue;
            using (var stream = new FileStream(filePath, FileMode.Open,
                    FileAccess.Read, FileShare.None, bufferSize: BufferSize, useAsync: true))
            using (var reader = new StreamReader(stream))
            {
                var fileContent = await reader.ReadToEndAsync();
                deserializedValue = JsonConvert.DeserializeObject<T>(fileContent);
                if (seekBegin)
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    stream.SetLength(0);
                }
            }
            if (deserializedValue is null)
                throw new NullReferenceException($"Attempt to read null from {filePath}.");
            return deserializedValue;
        }
    }
}
