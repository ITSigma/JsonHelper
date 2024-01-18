using JsonHelper.Infrastructure;
using Newtonsoft.Json;

namespace JsonHelper.Infrastructure
{
    internal static class DefaultFileWriter<T> 
    {
        //TODO: Настроить Paths
        public static async Task SaveToFileAsync(T valueToSave, string directoryPath,
            string fileName, Func<T, T, T> joinExistAndNewValues = null)
        {
            // directoryPath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);
            var filePath = Path.Combine(directoryPath, fileName);
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), directoryPath, fileName);

            if (joinExistAndNewValues is not null && File.Exists(filePath))
                valueToSave = await GetJoinedValues(valueToSave, filePath,
                    joinExistAndNewValues);

            using (var stream = new FileStream(filePath, FileMode.Create,
                FileAccess.Write, FileShare.None, bufferSize: Constants.BufferSize, useAsync: true))
            using (var writer = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, valueToSave);
                await jsonWriter.FlushAsync();
            }
        }

        private static async Task<T> GetJoinedValues(T valueToSave, string filePath,
            Func<T, T, T> joinExistAndNewValues)
        {
            var existingValue = await FileReader<T>.ReadFromFileAsync(filePath, true);
            valueToSave = joinExistAndNewValues(existingValue, valueToSave);
            return valueToSave;
        }
    }
}
