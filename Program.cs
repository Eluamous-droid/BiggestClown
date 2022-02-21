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
        private static Properties props = new Properties("BiggestClown.properties");

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            
            String summonersProperty = props.get("summoners","Eluamous");
            String[] summoners = summonersProperty.Split(',');
            foreach(String summoner in summoners)
            {
                HandleSummoner(summoner).GetAwaiter().GetResult();
            }
            
        }

        private static async Task HandleSummoner(String summoner){
            Summoner sum = await HTTPClientWrapper<Summoner>.Get(URL+summoner);
            

        }


        
    }
}
