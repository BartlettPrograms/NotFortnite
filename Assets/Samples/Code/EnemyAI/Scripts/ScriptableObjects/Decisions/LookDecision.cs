using UnityEngine;
using EnemyAI;

// The decision to see the target. Sense of sight.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Look")]
public class LookDecision : Decision
{
	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		// Reset sight status on loop before checking.
		controller.targetInSight = false;
		// Check sight.
		return Decision.CheckTargetsInRadius(controller, controller.viewRadius, MyHandleTargets);
	}

	// The delegate for results of overlapping targets in look decision.
	private bool MyHandleTargets(StateController controller, bool hasTargets, Collider[] targetsInViewRadius)
	{
		// Is there any sight on view radius?
		if(hasTargets)
		{
			Vector3 target = targetsInViewRadius[0].transform.position;
			// Check if target is in field of view.
			Vector3 dirToTarget = target - controller.transform.position;
			bool inFOVCondition = (Vector3.Angle(controller.transform.forward, dirToTarget) < controller.viewAngle / 2);
			// Is target in FOV and NPC have a clear sight?
			if (inFOVCondition && !controller.BlockedSight())
			{
				// Set current target parameters.
				controller.targetInSight = true;
				controller.personalTarget = controller.aimTarget.position;
				return true;
			}
		}
		// No target on sight.
		return false;
	}
}
