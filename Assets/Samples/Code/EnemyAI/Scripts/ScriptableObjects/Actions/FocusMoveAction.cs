using UnityEngine;
using EnemyAI;

// The strafe focusing on target action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Focus Move")]
public class FocusMoveAction : Action
{
	[Tooltip("The aim toggle decision while on focus.")]
	public ClearShotDecision clearShotDecision;

	private Vector3 currentDest;   // Current navigation destination.
	private bool aligned;          // Is the NPC orientation aligned to the target?

	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		// Align the NPC orientation.
		if (!aligned)
		{
			controller.nav.destination = controller.personalTarget;
			controller.nav.speed = 0f;
			// Only start strafing after orientation is aligned.
			if (controller.enemyAnimation.angularSpeed == 0)
			{
				controller.Strafing = true;
				aligned = true;
				controller.nav.destination = currentDest;
				controller.nav.speed = controller.generalStats.evadeSpeed;
			}
		}
		// Orientation is aligned, check if NPC has clear shot to the target.
		else
		{
			controller.haveClearShot = clearShotDecision.Decide(controller);
			// Grab clearShot instant (frame) change.
			if (controller.hadClearShot != controller.haveClearShot)
			{
				// Aim on target if sight is clear.
				controller.Aiming = controller.haveClearShot;
				// NPC is not returning to cover, will stop to shot.
				if (controller.haveClearShot && !Equals(currentDest, controller.CoverSpot))
					controller.nav.destination = controller.transform.position;
			}
			controller.hadClearShot = controller.haveClearShot;
		}
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.hadClearShot = controller.haveClearShot = false;
		currentDest = controller.nav.destination;
		controller.focusSight = true;
		aligned = false;
	}
}
