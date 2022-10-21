using UnityEngine;
using EnemyAI;

// The navigation to the spot with clear sight to target action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Go To Shot Spot")]
public class GoToShotSpotAction : Action
{
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.focusSight = false;
		controller.nav.destination = controller.personalTarget;
		controller.nav.speed = controller.generalStats.chaseSpeed;
		controller.enemyAnimation.AbortPendingAim();
	}
}
