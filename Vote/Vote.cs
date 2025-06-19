namespace VoteAPI;

public class VoteResult
{
    public string Candidate { get; set; }
    public int Votes { get; set; }
    public string Percentage { get; set; }
}

public class Vote
{
    public bool IsPoolingClosed { get; set; }
    public int PoolingRound { get; set; }
    public bool IsSecondRoundPlanned { get; set; }

    public Dictionary<string, int> Votes { get; set; } = new Dictionary<string, int>();
    public Dictionary<string, VoteResult> PoolingResults { get; set; } = new Dictionary<string, VoteResult>();

    public List<string> SecondRoundCandidates { get; set; } = new List<string>();
    
    public string EvaluateRound()
    {
        if (PoolingRound > 2)
        {
            throw new InvalidOperationException("Two round maximum");
        }
        
        if (!IsPoolingClosed)
        {
            throw new InvalidOperationException("The polling must be closed");
        }

        if (Votes == null)
        { 
            throw new InvalidOperationException("No votes to evaluate");
        }

        IsSecondRoundPlanned = false;
        
        int totalVotes = Votes.Values.Sum();

        var sortedResult = Votes.OrderByDescending(v => v.Value).ToList();
        var firstCandidate = sortedResult[0];
            
        double firstCandidatePercentage = (double)firstCandidate.Value / totalVotes * 100;
        
        if (firstCandidatePercentage > 50)
        {
            return firstCandidate.Key;
        }

        if (PoolingRound == 1)
        {
            IsSecondRoundPlanned = true;
            PlanSecondRound(sortedResult);
            return "second round";
        }

        if (PoolingRound == 2)
        {
            if (sortedResult[0].Value == sortedResult[1].Value)
            {
                // Second round - tie ; no winner
                return "no winner";
            }
            // Second round - no majority ; highest percentage win
            return firstCandidate.Key;
        }

        return null;
    }
    
    private void PlanSecondRound(List<KeyValuePair<string, int>> sortedCandidates)
    {
        SecondRoundCandidates.Clear();
        
        SecondRoundCandidates.Add(sortedCandidates[0].Key);
        SecondRoundCandidates.Add(sortedCandidates[1].Key);
    }
    
    public List<string> GetSecondRoundCandidates()
    {
        return SecondRoundCandidates.ToList();
    }
    
    public Dictionary<string, VoteResult> ComputePooling(Dictionary<string, int> votes)
    {
        if (votes == null || !votes.Any())
        { 
            throw new InvalidOperationException("No votes to evaluate");
        }
        
        int totalVotes = votes.Values.Sum();
        var results = new Dictionary<string, VoteResult>();
        
        // Trier les candidats par nombre de voix (décroissant)
        var sortedCandidates = votes.OrderByDescending(v => v.Value).ToList();
        
        int position = 1;
        foreach (var candidate in sortedCandidates)
        {
            double percentage = (double)candidate.Value / totalVotes * 100;
            
            results.Add(position.ToString(), new VoteResult
            {
                Candidate = position.ToString(),
                Votes = candidate.Value,
                Percentage = $"{percentage:F0}%"
            });
            
            position++;
        }
        
        PoolingResults = results;
        
        return results;
    }
}