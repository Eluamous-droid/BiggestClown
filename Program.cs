using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;

namespace BiggestClown
{
    class Program
    {
        private static Properties props = new Properties("BiggestClown.properties");

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            
            String summonerNamesToFetch = props.get("summoners","Eluamous");
            String[] summonerNames = summonerNamesToFetch.Split(',');
            Summoner[] summoners = GetSummoners(summonerNames).GetAwaiter().GetResult();
            int lastWeek = (int)DateTime.Now.AddDays(-7).Subtract(new DateTime(1970,1,1)).TotalSeconds;
            Dictionary<string, List<string>> matchHistories = GetMatchHistories(summoners, lastWeek).GetAwaiter().GetResult();

            Dictionary<string, List<Match>> completeMatchHistories = new Dictionary<string, List<Match>>();

            foreach(KeyValuePair<string,List<string>> summoner in matchHistories)
            {
                Thread.Sleep(60000);
                completeMatchHistories.Add(summoner.Key, new List<Match>());
                foreach(string matchId in summoner.Value)
                {
                    completeMatchHistories[summoner.Key].Add(GetMatch(matchId).GetAwaiter().GetResult());  
                }
            }

            Dictionary<string,int> lossCounter = getLossCounter(completeMatchHistories);

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

        private static async Task<Dictionary<string, List<string>>> GetMatchHistories(Summoner[] summoners, double startTime)
        {      
            Dictionary<string, List<string>> matchHistories = new Dictionary<string,List<string>>();
            foreach(Summoner summoner in summoners)
            {
                if(summoner.puuid != null){
                    String puuid = summoner.puuid;
                    String url = $"https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids";
                    UriBuilder uriBuilder = new UriBuilder(url);
                    uriBuilder.Port = -1;
                    NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                    query["startTime"] = startTime.ToString();
                    query["count"] = "100";
                    
                    uriBuilder.Query = query.ToString();
                    List<string> matchHistory = await HTTPClientWrapper<List<string>>.Get(uriBuilder.ToString());
                    if(matchHistory != null && summoner.name != null)
                    {
                        matchHistories.Add(summoner.name,matchHistory);
                    }
                }
                
            }
            return matchHistories;
        }

        private static async Task<Match> GetMatch(String matchId){

            String url = "https://europe.api.riotgames.com/lol/match/v5/matches/";
            return await HTTPClientWrapper<Match>.Get(url+matchId);
        }

        private static Dictionary<string,int> getLossCounter(Dictionary<string, List<Match>> completeMatchHistories)
        {
            Dictionary<string,int> lossCounter = new Dictionary<string, int>();
            foreach(String player in completeMatchHistories.Keys)
            {
                lossCounter.Add(player,0);
                foreach(Match match in completeMatchHistories[player])
                {
                    if(match.info != null && match.info.participants != null)
                    foreach(Participant participant in match.info.participants)
                    {
                        if(participant.summonerName != null && participant.summonerName.Equals(player) && !participant.win)
                        {
                            lossCounter[player]++;
                        }
                    }
                }

            }

            return lossCounter;
        }    
    }
}
