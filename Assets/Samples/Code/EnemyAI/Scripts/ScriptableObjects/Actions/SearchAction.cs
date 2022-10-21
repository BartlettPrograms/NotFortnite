using UnityEngine;
using EnemyAI;

// The search for point of interest action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Search")]
public class SearchAction : Action
{
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		if (Equals(controller.personalTarget, Vector3.positiveInfinity))
			controller.nav.destination = controller.transform.position;
		else
		{
			// Set navigation parameters.
			controller.nav.speed = controller.generalStats.chaseSpeed;
			controller.nav.destination = controller.personalTarget;
		}
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.focusSight = false;
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		controller.CoverSpot = Vector3.positiveInfinity;
	}
}
