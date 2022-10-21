using UnityEngine;
using EnemyAI;

// The return to cover spot action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Return to Cover")]
public class ReturnToCoverAction : Action
{
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		// Stop focusing on target, if there is a cover spot move to.
		if (!Equals(controller.CoverSpot, controller.transform.position))
			controller.focusSight = false;
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Is there a cover spot go?
		if (!Equals(controller.CoverSpot, Vector3.positiveInfinity))
		{
			// Set navigation parameters.
			controller.nav.destination = controller.CoverSpot;
			controller.nav.speed = controller.generalStats.chaseSpeed;
			// The cover spot not near the current NPC position, stop aiming.
			if (Vector3.Distance(controller.CoverSpot, controller.transform.position) > 0.5f)
			{
				controller.enemyAnimation.AbortPendingAim();
			}
		}
		// No cover spot, stand still.
		else
			controller.nav.destination = controller.transform.position;
	}
}
