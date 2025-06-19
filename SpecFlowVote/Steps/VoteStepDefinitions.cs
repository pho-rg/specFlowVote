using Xunit;

namespace SpecFlowVote.Steps;

[Binding]
public sealed class VoteStepDefinitions
{
    // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

    private readonly ScenarioContext _scenarioContext;
    private readonly Vote _vote = new Vote();

    private Dictionary<string, int> _votes = new Dictionary<string, int>();
    private string _electedCandidate;
    
    public VoteStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"the targeted pooling state is closed")]
    public void GivenTheTargetedPoolingStateIsClosed()
    {
        _vote.IsPollingClosed = true;
    }

    [Given(@"the targeted pooling round is (.*)")]
    public void GivenTheTargetedPoolingRoundIs(int round)
    {
        _vote.CurrentRound = round;
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

}