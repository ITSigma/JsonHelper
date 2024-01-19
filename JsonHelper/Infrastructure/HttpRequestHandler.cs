using Newtonsoft.Json;
using Polly;

namespace JsonHelper.Infrastructure
{
    internal class HttpRequestHandler
    {
        private const int DelayHttpClient = 10000;
        private const int DelayErrors = 2000;
        private int delayRequests { get; }
        private static readonly HttpClient sharedHttpClient;

        static HttpRequestHandler()
        {
            sharedHttpClient = new HttpClient
            {
                Timeout = TimeSpan.FromMilliseconds(DelayHttpClient)
            };
        }

        public HttpRequestHandler(int delayRequests = 1000)
        {
            this.delayRequests = delayRequests;
        }

        public async Task<T> GetRequestAsync<T>(string url, Action retryAction = null)
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    if (retryAction is not null)
                        retryAction();
                    return TimeSpan.FromMilliseconds(DelayErrors);
                },
                (exception, timeSpan, context) => { });

            var response2 = await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.Delay(delayRequests);
                var response = await sharedHttpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"Response code from {url} is {response.StatusCode} with message {response.RequestMessage}."
                        + $"\nCheck that args are correct.");
                var responseBody = await response.Content.ReadAsStringAsync();
                var deserializeResponse = JsonConvert.DeserializeObject<T>(responseBody);

                return deserializeResponse;
            });
            return response2;
        }
    }
}
