using Newtonsoft.Json;

namespace JsonHelper
{
    internal static class FileSaverBase<T> 
    {
        //public string DirectoryPath { get; }
        private const int BufferSize = 8192; //TODO: одно упоминание 
        //private FileNameBuilderBase<T> nameBuilder { get; }
        //private string fileExtension { get; }

        //public FileSaverBase(FileNameBuilderBase<T> nameBuilder,
        //    string directoryPath, string fileExtension)
        //{
        //    DirectoryPath = directoryPath;
        //    this.nameBuilder = nameBuilder;
        //    this.fileExtension = fileExtension;
        //}

        //public async Task SaveToFileAsync(T valueToSave)
        public static async Task SaveToFileAsync(T valueToSave, string directoryPath,
            string fileName)
        {
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            var filePath = $"{directoryPath}\\{fileName}";

            //if (File.Exists(filePath)) 
            //    valueToSave = await GetJoinedValues(valueToSave, filePath);

            using (var stream = new FileStream(filePath, FileMode.OpenOrCreate,
                FileAccess.Write, FileShare.None, bufferSize: BufferSize, useAsync: true))
            using (var writer = new StreamWriter(stream))
            using (var jsonWriter = new JsonTextWriter(writer))
            {
                var serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(jsonWriter, valueToSave);
                await jsonWriter.FlushAsync();
            }
        }

        //protected abstract T JoinExistingAndNewValues(T existingValue, T valueToSave);

        //private async Task<T> GetJoinedValues(T valueToSave, string filePath)
        //{
        //    T? existingValue = await FileReader<T>.ReadFromFileAsync(filePath, true);
        //    valueToSave = JoinExistingAndNewValues(existingValue, valueToSave);
        //    return valueToSave;
        //}
    }
}
