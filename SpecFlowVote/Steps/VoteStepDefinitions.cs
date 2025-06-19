using Xunit;

namespace SpecFlowVote.Steps;

using VoteAPI;

[Binding]
public sealed class VoteStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly Vote _vote = new Vote();

    private string _electedCandidate;
    private Dictionary<string, VoteResult> _poolingResults;
    
    public VoteStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the targeted pooling state is closed")]
    public void GivenTheTargetedPoolingStateIsClosed()
    {
        _vote.IsPoolingClosed = true;
    }

    [Given(@"the targeted pooling round is (.*)")]
    public void GivenTheTargetedPoolingRoundIs(int round)
    {
        _vote.PoolingRound = round;
    }

    [Given(@"the following candidates have votes:")]
    public void GivenTheFollowingCandidatesHaveVotes(Table table)
    {
        _vote.Votes.Clear();
        foreach (var row in table.Rows)
        {
            var candidateName = row["Candidate"];
            var votes = int.Parse(row["Votes"]);
            _vote.Votes[candidateName] = votes;
        }
    }

    [When(@"the round is evaluated")]
    public void WhenTheRoundIsEvaluated()
    {
        _electedCandidate = _vote.EvaluateRound();
    }

    [Then(@"the elected candidate should be (.*)")]
    public void ThenTheElectedCandidateShouldBe(string candidate)
    {
        Assert.Equal(candidate, _electedCandidate);
    }
    
    [Then(@"the second round should be (.*)")]
    public void ThenTheSecondRoundShouldBePlanned(bool secondRound)
    {
        Assert.Equal(secondRound, _vote.IsSecondRoundPlanned);
    }

    [Then(@"candidates should be (.*) and (.*)")]
    public void ThenCandidatesShouldBeAnd(string candidate1, string candidate2)
    {
        var secondRoundCandidates = _vote.GetSecondRoundCandidates();
        
        Assert.Contains(candidate1, secondRoundCandidates);
        Assert.Contains(candidate2, secondRoundCandidates);
    }
    
    [Then(@"the result should include:")]
    public void ThenTheResultShouldInclude(Table table)
    {
        _poolingResults = _vote.ComputePooling(_vote.Votes);
        
        foreach (var row in table.Rows)
        {
            var candidate = row["Candidate"];
            var expectedPercentage = row["Percentage"];

            Assert.True(_poolingResults.ContainsKey(candidate));
            var result = _poolingResults[candidate];
            Assert.Equal(expectedPercentage, result.Percentage);
        }
    }
}