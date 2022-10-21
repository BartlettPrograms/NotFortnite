using UnityEngine;
using EnemyAI;

// Stop focusing on target action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Exit Focus")]
public class ExitFocusAction : Action
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
		controller.variables.feelAlert = false;
		controller.variables.hearAlert = false;
		controller.Strafing = false;
		controller.nav.destination = controller.personalTarget;
		controller.nav.speed = 0f;
	}
}
