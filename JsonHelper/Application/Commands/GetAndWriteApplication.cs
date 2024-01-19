using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonHelper.Infrastructure;

namespace JsonHelper.Application
{
    internal class GetAndWriteApplication<TGet, TWrite> : IApplicationCommand<string>
    {
        private string directoryPath { get; }
        private IFileNameBuilder<TWrite> nameBuilder { get; }
        private Func<TGet, TWrite> getToWriteValueConverter { get; }
        private static readonly HttpRequestHandler requestHandler = new HttpRequestHandler();
        private static readonly object lockObject = new object();

        public GetAndWriteApplication(string directoryPath,
            IFileNameBuilder<TWrite> nameBuilder,
            Func<TGet, TWrite> getToWriteValueConverter)
        {
            this.directoryPath = directoryPath;
            this.nameBuilder = nameBuilder;
            this.getToWriteValueConverter = getToWriteValueConverter;
        }

        public async void Execute(string url)
        {
            var getValue = await requestHandler.GetRequestAsync<TGet>(url);
            var valueToWrite = getToWriteValueConverter(getValue);
            if (valueToWrite is null)
                throw new NullReferenceException($"Received from {url} value is null.");
            var filename = nameBuilder.BuildName(valueToWrite, "json");
            lock (lockObject)
            {
                DefaultFileWriter<TWrite>.SaveToFileAsync(valueToWrite, directoryPath, filename);
            }
        }
    }
}
