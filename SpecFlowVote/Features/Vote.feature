Feature: Vote

	
@mytag
Scenario: Evaluate first round with winner
	Given the targeted pooling state is closed
	And the targeted pooling round is 1
	And the following candidates have votes:
	  | Candidate | Votes |
	  | Lewis     | 60    |
	  | Charles   | 30    |
	  | Fernando  | 10    |
	When the round is evaluated
	Then the elected candidate should be Lewis

Scenario: Evaluate first round without winner
	Given the targeted pooling state is closed
	And the targeted pooling round is 1
	And the following candidates have votes:
	  | Candidate | Votes |
	  | Lewis     | 60    |
	  | Charles   | 30    |
	  | Fernando  | 10    |
	When the round is evaluated
	Then the second round should be planned
	And candidates should be 1 and 2
		
Scenario: Compute the pooling
	Given the targeted pooling state is closed
	And candidate 1 has 100 votes
	And candidate 2 has 20 votes
	And candidate 3 has 80 votes
	When the pooling is computed
	Then the result should include:
	  | Candidate | Votes | Percentage |
	  | 1         | 100   | 50%        |
	  | 2         | 20    | 10%        |
	  | 3         | 80    | 40%        |
   
Scenario: Evaluate second round with winner
	Given the targeted pooling state is closed
	And candidate 1 has 60 votes
	And candidate 2 has 40 votes
	When the round is evaluated
	Then the elected candidate should be 1
