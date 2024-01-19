using Castle.Core.Internal;

namespace JsonHelper.Application
{
    internal class UrlCompiler : IUrlCompiler
    {
        public string BaseUrl { get; }

        public UrlCompiler(string baseUrl)
        {
            BaseUrl = baseUrl;
        }

        public string Compile(Dictionary<string, string> headerValues = null)
        {
            if (headerValues.IsNullOrEmpty())
                return BaseUrl;
            var headers = string.Join('&', headerValues
                .Select(header => $"{header.Key}={header.Value}")
                .ToArray());
            return $"{BaseUrl}?{headers}";
        }
    }
}
