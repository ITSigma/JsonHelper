using Newtonsoft.Json;

namespace JsonHelper.Infrastructure
{
    internal class FileReader<T>
    {
        public static async Task<T> ReadFromFileAsync(string filePath, bool seekBegin = false)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"There is no such file in {filePath}");
            T? deserializedValue;
            using (var stream = new FileStream(filePath, FileMode.Open,
                    FileAccess.Read, FileShare.None, bufferSize: Constants.BufferSize, useAsync: true))
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
