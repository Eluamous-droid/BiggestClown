public struct Player
{
    public string puuid {get;set;}
    public string summonerName {get;set;}
    public List<String> matchIds{get;set;}
    public List<Match> matches {get;set;}
    public Summoner summoner {get;set;}
    public int lossCounter {get;set;}

}