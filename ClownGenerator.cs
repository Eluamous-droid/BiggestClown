using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;

namespace BiggestClown
{
    class ClownGenerator
    {
        private static Properties props = new Properties("BiggestClown.properties");

        private static List<Player> getSortedListOfClowns(){
            int lastWeek = (int)DateTime.Now.AddDays(-7).Subtract(new DateTime(1970,1,1)).TotalSeconds;           
            String summonerNamesToGet = props.get("summoners","Eluamous");
            String[] summonerNames = summonerNamesToGet.Split(',');
            List<Player> players = new List<Player>();
            foreach(String summonerName in summonerNames)
            {
                players.Add(buildPlayer(summonerName, lastWeek));
                //To avoid api call limit
                Thread.Sleep(60000);
            }
            return players = Sort.sortPlayersByLossCount(players);
        }

        private static Player buildPlayer(string summonerName, int fromDate)
        {
            Player player = new Player();
            player.summonerName = summonerName;
            player.summoner = GetSummoner(summonerName).GetAwaiter().GetResult();
            player.matchIds = GetMatchHistory(player.summoner, fromDate).GetAwaiter().GetResult();
            player.matches = new List<Match>();
            foreach (String matchId in player.matchIds)
            {
                player.matches.Add(GetMatch(matchId).GetAwaiter().GetResult());
            }
            player.lossCounter = getLossCounter(player);

            return player;
        }

        private static async Task<Summoner> GetSummoner(String summonerName){

            String url = "https://euw1.api.riotgames.com/lol/summoner/v4/summoners/by-name/";
            List<Summoner> summoners = new List<Summoner>();
            Summoner summoner = await HTTPClientWrapper<Summoner>.Get(url+summonerName);
            return summoner;
        }

        private static async Task<List<string>> GetMatchHistory(Summoner summoner, double startTime)
        {
            List<string> matchHistory = new List<string>();
            if (summoner.puuid != null)
            {
                String puuid = summoner.puuid;
                String url = $"https://europe.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids";
                UriBuilder uriBuilder = new UriBuilder(url);
                uriBuilder.Port = -1;
                NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["startTime"] = startTime.ToString();
                query["count"] = "100";

                uriBuilder.Query = query.ToString();
                matchHistory = await HTTPClientWrapper<List<string>>.Get(uriBuilder.ToString());
            }
            return matchHistory;
        }

        private static async Task<Match> GetMatch(String matchId){

            String url = "https://europe.api.riotgames.com/lol/match/v5/matches/";
            return await HTTPClientWrapper<Match>.Get(url+matchId);
        }

        private static int getLossCounter(Player player)
        {
            int lossCounter = 0;
            foreach (Match match in player.matches)
            {
                if (match.info?.participants != null)
                    foreach (Participant participant in match.info.participants)
                    {
                        if (participant.summonerName != null && participant.summonerName.Equals(player.summonerName) && !participant.win)
                        {
                            lossCounter++;
                        }
                    }
            }
            return lossCounter;
        }

        
    }
}
