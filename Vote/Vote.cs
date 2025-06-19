namespace Vote;

public class Vote
{
    public bool IsPoolingClosed { get; set; }
    public int PoolingRound { get; set; }
    public Dictionary<string, int> Votes { get; set; } = new Dictionary<string, int>();
    
    public string EvaluateRound()
    {
        if (!IsPoolingClosed)
        {
            throw new InvalidOperationException("The polling must be closed");
        }

        if (Votes == null)
        { 
            throw new InvalidOperationException("No votes to evaluate");
        }
        
        int totalVotes = Votes.Values.Sum();
            
        var winner = Votes.OrderByDescending(v => v.Value).First();
            
        double winnerPercentage = (double)winner.Value / totalVotes * 100;
        
        if (winnerPercentage > 50)
        {
            return winner.Key;
        }

        return null;
    }
}