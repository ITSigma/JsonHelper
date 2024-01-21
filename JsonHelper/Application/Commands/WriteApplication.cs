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
    internal class WriteApplication<T> 
    {
        private string directoryPath { get; }
        private FileNameBuilderBase<T> nameBuilder { get; }
        private readonly SemaphoreSlim matchesSemaphore = new SemaphoreSlim(1, 1);

        public WriteApplication(string directoryPath,
            FileNameBuilderBase<T> nameBuilder)
        {
            this.directoryPath = directoryPath;
            this.nameBuilder = nameBuilder;
        }

        public async Task WriteValueAsync(T value)
        {
            var filename = nameBuilder.BuildName(value, "json");
            await matchesSemaphore.WaitAsync();
            try
            {
                await DefaultFileWriter<T>.SaveToFileAsync(value, directoryPath, filename);
            }
            finally
            {
                matchesSemaphore.Release(); 
            }
        }
    }
}
