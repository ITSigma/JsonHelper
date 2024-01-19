using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using JsonHelper.Domain;
using JsonHelper.Infrastructure;

namespace JsonHelper.Application
{
    internal class GetAndWriteApplication<TGet, TWrite> 
    {
        private string directoryPath { get; }
        private IFileNameBuilder<TWrite> nameBuilder { get; }
        private Func<TGet, TWrite> getToWriteValueConverter { get; }
        private readonly HttpRequestHandler requestHandler;
        private UrlCompiler urlCompiler;
        private KeyGetter keyGetter;
        private readonly object lockObject = new object();

        public GetAndWriteApplication(string directoryPath,
            IFileNameBuilder<TWrite> nameBuilder,
            Func<TGet, TWrite> getToWriteValueConverter,
            string baseUrl,
            KeyGetter keyGetter = null,
            int requestDelay = 1000)
        {
            this.getToWriteValueConverter = getToWriteValueConverter;
            this.directoryPath = directoryPath;
            this.nameBuilder = nameBuilder;
            this.keyGetter = keyGetter;
            requestHandler = new HttpRequestHandler(requestDelay);
            urlCompiler = new UrlCompiler(baseUrl);
        }

        public async void Execute(Dictionary<string, string> headers = null)
        {
            var url = GetUrl(headers);
            var getValue = await requestHandler.GetRequestAsync<TGet>(url);
            if (getValue is null)
                throw new NullReferenceException($"Received from {url} value is null.");
            WriteValue(getValue);
        }

        private string GetUrl(Dictionary<string, string> headers = null)
        {
            if (keyGetter is not null)
                if (headers is not null)
                    headers["key"] = keyGetter.GetNextKey();
                else
                    headers = new Dictionary<string, string>()
                    {
                        ["key"] = keyGetter.GetNextKey()
                    };
            return urlCompiler.Compile(headers);
        }

        private void WriteValue(TGet getValue)
        {
            var valueToWrite = getToWriteValueConverter(getValue);
            var filename = nameBuilder.BuildName(valueToWrite, "json");
            lock (lockObject)
            {
                DefaultFileWriter<TWrite>.SaveToFileAsync(valueToWrite, directoryPath, filename);
            }
        }
    }
}
