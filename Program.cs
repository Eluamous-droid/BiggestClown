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
            
            String summonerNamesToFetch = props.get("summoners","Eluamous");
            String[] summonerNames = summonerNamesToFetch.Split(',');
            Summoner[] summoners = GetSummoners(summonerNames).GetAwaiter().GetResult();
            double lastWeek = DateTime.Now.AddDays(-7).Subtract(new DateTime(1970,1,1)).TotalSeconds;
            GetMatchHistories(summoners, lastWeek).GetAwaiter().GetResult();
                       
        }

        private static async Task<Summoner[]> GetSummoners(String[] summonerNames){

            String url = "https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/";
            List<Summoner> summoners = new List<Summoner>();
            foreach(String summoner in summonerNames)
            {
                summoners.Add(await HTTPClientWrapper<Summoner>.Get(url+summoner));
            }
            return summoners.ToArray();
        }

        private static async Task GetMatchHistories(Summoner[] summoners, double StartTime)
        {

        }


        
    }
}
