namespace JsonHelper.Application
{
    internal abstract class FileNameBuilderBase<T>
    {
        protected Func<T, string> valueToNameFunc { get; }
        protected string label { get; }

        public FileNameBuilderBase(Func<T, string> valueToNameFunc, string label)
        {
            this.valueToNameFunc = valueToNameFunc;
            this.label = label;
        }

        public abstract string BuildName(T value, string fileExtension);

        protected string BuildNameDefault(T value, string fileExtension)
            => $"{label}_{valueToNameFunc(value)}.{fileExtension}";
    }
}
