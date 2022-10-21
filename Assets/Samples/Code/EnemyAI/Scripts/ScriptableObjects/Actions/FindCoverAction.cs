using System.Collections;
using UnityEngine;
using EnemyAI;

// The find a cover spot action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Find Cover")]
public class FindCoverAction : Action
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
		controller.enemyAnimation.AbortPendingAim();
		controller.enemyAnimation.anim.SetBool("Crouch", false);
		// Get the best cover spot, considering current NPCs and target positions.
		ArrayList nextCoverData = controller.coverLookup.GetBestCoverSpot(controller);
		Vector3 potentialCover = (Vector3)nextCoverData[1];
		// No cover spot.
		if (Vector3.Equals(potentialCover, Vector3.positiveInfinity))
		{
			controller.nav.destination = controller.transform.position;
			return;
		}
		// Closer cover spot, update spot position.
		else if ((controller.personalTarget - potentialCover).sqrMagnitude < (controller.personalTarget - controller.CoverSpot).sqrMagnitude
			&& !controller.IsNearOtherSpot(potentialCover, controller.nearRadius))
		{
			controller.coverHash = (int)nextCoverData[0];
			controller.CoverSpot = potentialCover;
		}
		// Set navigation parameters.
		controller.nav.destination = controller.CoverSpot;
		controller.nav.speed = controller.generalStats.evadeSpeed;
		// Fullfill current round shots.
		controller.variables.currentShots = controller.variables.shotsInRound;
	}
}
