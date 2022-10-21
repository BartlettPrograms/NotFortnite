using UnityEngine;
using EnemyAI;

// The decision to keep engaging the current target position.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Engage")]
public class EngageDecision : Decision
{
	[Header("Extra Decisions")]
	[Tooltip("The NPC sight decision.")]
	public LookDecision isViewing;
	[Tooltip("The NPC near sense decision.")]
	public FocusDecision targetNear;

	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		// The target is on sight, or is it in near sense?
		if(isViewing.Decide(controller) || targetNear.Decide(controller))
		{
			controller.variables.blindEngageTimer = 0;
		}
		// The blind engage timer surpassed the maximum time?
		else if(controller.variables.blindEngageTimer >= controller.blindEngageTime)
		{
			// Stop engaging.
			controller.variables.blindEngageTimer = 0;
			return false;
		}
		// Keep engaging.
		return true;
	}
}
