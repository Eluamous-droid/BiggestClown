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

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            HandleSummoners().GetAwaiter().GetResult();
        }

        private static async Task HandleSummoners(){
            Summoner sum = await HTTPClientWrapper<Summoner>.Get(URL+"Eluamous");
            Console.WriteLine(sum.name);

        }


        
    }
}
