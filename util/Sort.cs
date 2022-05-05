namespace BiggestClown;

public class Sort
{
    public static List<Player> sortPlayersByLossCount(List<Player> players)
    {
        for(int i = 0; i < players.Count; i++)
        {
            Player currentPlayer = players[i];
            bool flag = false;
            for(int j = i - 1; j >= 0 && !flag; )
            {
                if(currentPlayer.lossCounter < players[j].lossCounter)
                {
                    players[j+1] = players[j];
                    j--;
                    players[j+1] = currentPlayer;
                }
                else
                {
                    flag = true;
                }
            }
        }
        return players;
    }
}