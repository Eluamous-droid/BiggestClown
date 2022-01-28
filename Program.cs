using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace BiggestClown
{
    class Program
    {
        private const string URL = "https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/";
        private static string urlParameters = "";

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            Properties props = new Properties("BiggestClown.properties");
            urlParameters = props.get("api_key");
            RunAsync().GetAwaiter().GetResult();
            client.Dispose();
        }

        static async Task RunAsync()
        {
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Riot-Token", urlParameters);
            
            try
            {
                Summoner summoner = await GetSummoner("Eluamous");
                Console.WriteLine(summoner.puuid);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }


        private async static  Task<Summoner> GetSummoner(String username)
        {

            // Add an Accept header for JSON format.
            
            HttpResponseMessage response = await client.GetAsync(URL+username);  // Blocking call! Program will wait here until a response is received or a timeout occurs.
            Summoner dataObject = null;
            if (response.IsSuccessStatusCode)
            {
                 // Parse the response body.
                 dataObject = await response.Content.ReadAsAsync<Summoner>(); //Make sure to add a reference to System.Net.Http.Formatting.dll
                  
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }

            return dataObject;
        }

        
    }
}
