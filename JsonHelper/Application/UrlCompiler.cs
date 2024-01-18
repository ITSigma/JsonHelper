namespace JsonHelper.Application
{
    internal class UrlCompiler : IUrlCompiler
    {
        public string BaseUrl { get; }

        public UrlCompiler(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string Compile(Dictionary<string, string> headerValues)
        {
            var headers = string.Join('&', headerValues
                .Select(header => $"{header.Key}={header.Value}")
                .ToArray());
            return headers.Length > 0 ? $"{BaseUrl}?{headers}&" : BaseUrl;
        }
    }
}
