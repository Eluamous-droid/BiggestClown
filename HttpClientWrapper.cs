using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

    /// <summary>
    /// A generic wrapper class to REST API calls
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class HTTPClientWrapper<T> where T : class
    {
        /// <summary>
        /// For getting the resources from a web api
        /// </summary>
        /// <param name="url">API Url</param>
        /// <returns>A Task with result object of type T</returns>
        public static async Task<T> Get(string url, String apiKey)
        {
            T? result = null;
            using (var httpClient = new HttpClient())
            {

                

                httpClient.DefaultRequestHeaders.Add("X-Riot-Token",apiKey);

                var response = httpClient.GetAsync(new Uri(url)).Result;

                response.EnsureSuccessStatusCode();
                await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                {
                    if (x.IsFaulted)
                    {
                        if(x.Exception != null)
                        {
                            throw x.Exception;
                        }
                        throw new ApplicationException("Rest call failed with no exception");
                    }

                    result = JsonConvert.DeserializeObject<T>   (x.Result);
                });
            }
            
            return result != null ? result : default!;
        }
    }