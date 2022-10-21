using UnityEngine;
using EnemyAI;

// The decision to end the shot burst on current engage round.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/End Burst")]
public class EndBurstDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		return controller.variables.currentShots >= controller.variables.shotsInRound;
	}
}
