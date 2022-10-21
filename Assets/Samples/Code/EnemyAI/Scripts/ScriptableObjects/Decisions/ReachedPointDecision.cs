using UnityEngine;
using EnemyAI;

// The decision of whether or not the spot was reached.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Reached Point")]
public class ReachedPointDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		if (controller.nav.remainingDistance <= controller.nav.stoppingDistance && !controller.nav.pathPending)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
