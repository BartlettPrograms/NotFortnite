using UnityEngine;
using EnemyAI;

// Decision to wait an ammount of time.
[CreateAssetMenu(menuName = "Enemy AI/Decisions/Waited")]
public class WaitedDecision : Decision
{
	public float maxTimeToWait;  // Maximum time to wait on a round.

	private float timeToWait;	 // Time to wait on current round.
	private float startTime;     // Timestamp of when the NPC began to wait.

	// The decide function, called on Update() (State controller - current state - transition - decision).
	public override bool Decide(StateController controller)
	{
		return (Time.time - startTime) >= timeToWait;
	}
	// The decision on enable function, triggered once after a FSM state transition.
	public override void OnEnableDecision(StateController controller)
	{
		// Calculate time to wait on current round.
		timeToWait = Random.Range(0, maxTimeToWait);
		// Set start waiting time.
		startTime = Time.time;
	}
}
