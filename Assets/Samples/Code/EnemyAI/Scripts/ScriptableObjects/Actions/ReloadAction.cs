using UnityEngine;
using EnemyAI;

// The NPC reload weapon action.
[CreateAssetMenu(menuName = "Enemy AI/Actions/Reload")]
public class ReloadAction : Action
{
	// The act function, called on Update() (State controller - current state - action).
	public override void Act(StateController controller)
	{
		if(!controller.reloading && controller.bullets <= 0)
		{
			// Set reloading animation state.
			controller.enemyAnimation.anim.SetTrigger("Reload");
			controller.reloading = true;
			// Play weapon reloading sound clip.
			AudioSource.PlayClipAtPoint(controller.classStats.reloadSound, controller.enemyAnimation.gunMuzzle.position, 2f);
		}
	}
}
