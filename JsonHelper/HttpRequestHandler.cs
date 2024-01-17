using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Polly;

namespace JsonHelper
{
    internal class HttpRequestHandler
    {
        private const int DelayHttpClient = 10000;
        private const int DelayErrors = 2000;
        private int delayRequests { get; set; }
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

        public async Task<T> GetRequestAsync<T>(string url, Action retryAction)
        {
            var retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>() 
                .WaitAndRetryAsync(3, retryAttempt =>
                {
                    retryAction(); 
                    return TimeSpan.FromMilliseconds(DelayErrors); 
                },
                (exception, timeSpan, context) =>
                {
                    //Console.WriteLine($"Retry after {timeSpan.Seconds} seconds due to: {exception.Message}");
                    //TODO: Логирование?
                });

            var response2 = await retryPolicy.ExecuteAsync(async () =>
            {

                await Task.Delay(delayRequests);
                var response = await sharedHttpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    //Console.WriteLine($"Failed to retrieve match data for {matchId}. HTTP Status: {response.StatusCode}");
                    //TODO: Логирование?
                    return default;
                }
                var responseBody = await response.Content.ReadAsStringAsync();
                var deserializeResponse = JsonConvert.DeserializeObject<T>(responseBody);

                return deserializeResponse;
            });

            return default;
        }
    }
}
