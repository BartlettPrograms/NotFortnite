using EnemyAI;
using UnityEngine;

// In place focus on target action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Spot Focus")]
public class SpotFocusAction : Action
{
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		controller.nav.destination = controller.personalTarget;
		controller.nav.speed = 0f;
	}
}
