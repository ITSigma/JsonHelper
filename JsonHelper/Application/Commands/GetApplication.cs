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
    internal class GetApplication<T> 
    {
        private readonly HttpRequestHandler requestHandler;
        private UrlCompiler urlCompiler;
        private KeyGetter keyGetter;

        public GetApplication(string baseUrl,
            KeyGetter keyGetter = null,
            int requestDelay = 1000)
        {
            this.keyGetter = keyGetter;
            requestHandler = new HttpRequestHandler(requestDelay);
            urlCompiler = new UrlCompiler(baseUrl);
        }

        public async Task<T> GetValueAsync(Dictionary<string, string> headers = null,
            Action retryAction = null)
        {
            var url = GetUrl(headers);
            var getValue = await requestHandler.GetRequestAsync<T>(url, retryAction);
            return getValue;
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
    }
}
