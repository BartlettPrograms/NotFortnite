using UnityEngine;

namespace EnemyAI
{
	// The FSM state scriptable object definition, containing actions and transitions.
	[CreateAssetMenu(menuName = "Enemy AI/State")]
	public class State : ScriptableObject
	{
		[Tooltip("The actions to perform when on the state.")]
		public Action[] actions;
		[Tooltip("The transitions to check when on the state.")]
		public Transition[] transitions;
		[Tooltip("The state category color (blue: clear, yellow: warning, red: engage).")]
		public Color sceneGizmoColor = Color.grey;

		// Perform the state corresponding actions.
		public void DoActions(StateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				actions[i].Act(controller);
			}
		}

		// Trigger the state action once when the state is becomes the current one.
		public void OnEnableActions(StateController controller)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				// Trigger on enable for all actions once, when the state is activated.
				actions[i].OnEnableAction(controller);
			}
			for (int i = transitions.Length - 1; i >= 0; i--)
			{
				// Trigger on enable for all decisions once, when the state is activated.
				transitions[i].decision.OnEnableDecision(controller);
			}
		}

		// Check the state corresponding transitions to other FSM states.
		public void CheckTransitions(StateController controller)
		{
			// First decisions has precedence over the last ones.
			for (int i = 0; i < transitions.Length; i++)
			{
				bool decision = transitions[i].decision.Decide(controller);
				if (decision)
				{
					// Go to true state.
					controller.TransitionToState(transitions[i].trueState, transitions[i].decision);
				}
				else
				{
					// Go to false state.
					controller.TransitionToState(transitions[i].falseState, transitions[i].decision);
				}
				// If a transition was performed to another state, trigger on enable for all actions of new state.
				if (controller.currentState != this)
				{
					controller.currentState.OnEnableActions(controller);
					// No need to check remaining transitions.
					break;
				}
			}
		}
	}
}
