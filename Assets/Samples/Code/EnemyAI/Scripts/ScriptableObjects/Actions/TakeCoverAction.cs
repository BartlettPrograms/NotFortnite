using UnityEngine;
using EnemyAI;

// The NPC take cover action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Take Cover")]
public class TakeCoverAction : Action
{
	private readonly int coverMin = 2;  // Minimum period to stay on cover.
	private readonly int coverMax = 5;  // Maximum period to stay on cover.

	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		// Only count cover time if NPC is not reloading the weapon.
		if(!controller.reloading)
			controller.variables.waitInCoverTimer += Time.deltaTime;

		// Count blind engage timer.
		controller.variables.blindEngageTimer += Time.deltaTime;
		// If crouching, orientate NPC to current target.
		if (controller.enemyAnimation.anim.GetBool("Crouch"))
			Rotating(controller);
	}
	// Rotate the NPC to face the target.
	private void Rotating(StateController controller)
	{
		Quaternion targetRotation = Quaternion.LookRotation(controller.personalTarget - controller.transform.position);
		if(Quaternion.Angle(controller.transform.rotation, targetRotation) > 5f)
			controller.transform.rotation = Quaternion.Slerp(controller.transform.rotation, targetRotation, 10f * Time.deltaTime);
	}
	// The action on enable function, triggered once after a FSM state transition.
	public override void OnEnableAction(StateController controller)
	{
		// Setup initial values for the action.
		controller.variables.feelAlert = false;
		controller.variables.waitInCoverTimer = 0f;
		// Set current round cover time.
		if (!Equals(controller.CoverSpot, Vector3.positiveInfinity))
		{
			controller.enemyAnimation.anim.SetBool("Crouch", true);
			controller.variables.coverTime = Random.Range(coverMin, coverMax);
		}
		else
		{
			controller.variables.coverTime = 0.1f;
		}
		// Abort pending aim requests to enter cover.
		controller.enemyAnimation.AbortPendingAim();
	}
}
