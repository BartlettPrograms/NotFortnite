using UnityEngine;
using EnemyAI;

// The decision to check if sight to target is clear.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Clear Shot")]
public class ClearShotDecision : Decision
{
	[Header("Extra Decisions")]
	[Tooltip("The NPC near sense decision.")]
	public FocusDecision targetNear;

	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		return targetNear.Decide(controller) || HaveClearShot(controller);
	}
	// Cast sphere for near obstacles, and line to personal target (not the aim target) for clean shot.
	private bool HaveClearShot(StateController controller)
	{
		Vector3 shotOrigin = controller.transform.position + Vector3.up * (controller.generalStats.aboveCoverHeight + controller.nav.radius);
		Vector3 shotDirection = controller.personalTarget - shotOrigin;

		// Cast sphere in target direction to check for obstacles in near radius.
		bool obscuredShot = Physics.SphereCast(shotOrigin, controller.nav.radius, shotDirection, out RaycastHit hit,
			controller.nearRadius, controller.generalStats.coverMask | controller.generalStats.obstacleMask);
		if (!obscuredShot)
		{
			// No near obstacles, cast line to target position and check for clear shot.
			obscuredShot = Physics.Raycast(shotOrigin, shotDirection, out hit, shotDirection.magnitude,
				controller.generalStats.coverMask | controller.generalStats.obstacleMask);
			// Hit something, is it the target? If true, shot is clear.
			if(obscuredShot)
				obscuredShot = !(hit.transform.root == controller.aimTarget.root);
		}
		return !obscuredShot;
	}
}
