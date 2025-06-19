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
	Then the second round should be false
	And the elected candidate should be Lewis

Scenario: Evaluate first round with no winner
	Given the targeted pooling state is closed
	And the targeted pooling round is 1
	And the following candidates have votes:
	  | Candidate | Votes |
	  | Lewis     | 40    |
	  | Charles   | 40    |
	  | Fernando  | 20    |
	When the round is evaluated
	Then the second round should be true
	And candidates should be Lewis and Charles
   
Scenario: Evaluate second round with winner
	Given the targeted pooling state is closed
	And the targeted pooling round is 2
	And the following candidates have votes:
	  | Candidate | Votes |
	  | Lewis     | 60    |
	  | Charles   | 40    |
	When the round is evaluated
	Then the elected candidate should be Lewis

Scenario: Evaluate second round with no majority
	Given the targeted pooling state is closed
	And the targeted pooling round is 2
	And the following candidates have votes:
	  | Candidate | Votes |
	  | Lewis     | 30    |
	  | Charles   | 40    |
	When the round is evaluated
	Then the elected candidate should be Charles
	
	Scenario: Evaluate second round with no winner
		Given the targeted pooling state is closed
		And the targeted pooling round is 2
		And the following candidates have votes:
		  | Candidate | Votes |
		  | Lewis     | 40    |
		  | Charles   | 40    |
		When the round is evaluated
		Then the elected candidate should be no winner
		
	Scenario: Compute the pooling
		Given the targeted pooling state is closed
		And the following candidates have votes:
		  | Candidate | Votes |
		  | Lewis     | 40    |
		  | Charles   | 40    |
		  | Fernando  | 20    |
		Then the result should include:
		  | Candidate | Votes | Percentage |
		  | 1         | 40    | 40%        |
		  | 2         | 40    | 40%        |
		  | 3         | 20    | 20%        |