using UnityEngine;
using EnemyAI;

// The aim callibration action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Debug Aim")]
public class DebugAimAction : Action
{
	private HearDecision hearDecision; // Decision to update target position upon movement.

	// Triggered once in a lifetime.
	private void OnEnable()
	{
		// Setup the references.
		hearDecision = ScriptableObject.CreateInstance<HearDecision>();
	}
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		if (hearDecision.Decide(controller))
		{
			// Focus on target.
			controller.focusSight = controller.targetInSight = true;
			controller.personalTarget = controller.aimTarget.position;
			// DEBUG: draw alignment lines.
			Debug.DrawRay(controller.enemyAnimation.gunMuzzle.position, -controller.enemyAnimation.gunMuzzle.right, Color.red);
			Debug.DrawLine(controller.enemyAnimation.gunMuzzle.position, controller.aimTarget.position, Color.green);
		}
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Activate NPC AIM.
		controller.enemyAnimation.ActivatePendingAim();
		// Set initial aim focus.
		controller.personalTarget = controller.transform.position + controller.transform.forward * controller.perceptionRadius;
	}
}
