namespace JsonHelper
{
    internal interface IFileNameBuilder<T>
    {
        public string BuildName(T value, string fileExtension);

    }
}
