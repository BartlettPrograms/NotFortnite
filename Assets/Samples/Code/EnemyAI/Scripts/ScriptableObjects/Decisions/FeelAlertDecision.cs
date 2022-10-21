using UnityEngine;
using EnemyAI;

// The decision to react to an alert received.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Feel Alert")]
public class FeelAlertDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		return controller.variables.feelAlert;
	}
}
