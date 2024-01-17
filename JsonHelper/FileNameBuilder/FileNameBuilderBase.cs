namespace JsonHelper
{
    internal abstract class FileNameBuilderBase<T>
    {
        public abstract string BuildName(string directoryPath, T values, string fileExtension);

        public string BuildNameDefault(string directoryPath, string filename,
            string fileExtension)
            => $"{directoryPath}/{filename}.{fileExtension}";
    }
}
