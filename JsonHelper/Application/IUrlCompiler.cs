namespace JsonHelper.Application
{
    internal interface IUrlCompiler
    {
        public string BaseUrl { get; }
        public string Compile(Dictionary<string, string> headerValues);
    }
}
