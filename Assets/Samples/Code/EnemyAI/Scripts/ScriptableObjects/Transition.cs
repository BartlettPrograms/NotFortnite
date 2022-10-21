using UnityEngine;

namespace EnemyAI
{
	// The transition class, used to define FSM state transitions, based on a decision.
	[System.Serializable]
	public class Transition
	{
		[Tooltip("The decision to trigger the corresponding transition.")]
		public Decision decision;
		[Tooltip("The state to go in case the decision is true")]
		public State trueState;
		[Tooltip("The state to go in case the decision is false")]
		public State falseState;
	}
}
