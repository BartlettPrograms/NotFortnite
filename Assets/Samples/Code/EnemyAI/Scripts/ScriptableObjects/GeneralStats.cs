using UnityEngine;

namespace EnemyAI
{
	// NPC common stats.
	[CreateAssetMenu(menuName = "Enemy AI/General Stats")]
	public class GeneralStats : ScriptableObject
	{
		[Header("General")]
		[Tooltip("NPC patrolling speed (clear state).")]
		public float patrolSpeed = 2f;
		[Tooltip("NPC search speed (warning state).")]
		public float chaseSpeed = 5f;
		[Tooltip("NPC evade speed (engage state).")]
		public float evadeSpeed = 15f;
		[Tooltip("How long to wait on a waypoint.")]
		public float patrolWaitTime = 2f;
		[Tooltip("The obstacle layer mask.")]
		public LayerMask obstacleMask;
		[Header("Animation")]
		[Tooltip("Clearance angle (deadzone) to avoid aim flickering.")]
		public float angleDeadzone = 5f;
		[Tooltip("Damping time for the speed parameter.")]
		public float speedDampTime = 0.4f;
		[Tooltip("Damping time for the angularSpeed parameter.")]
		public float angularSpeedDampTime = 0.2f;
		[Tooltip("Response time for turning an angle into angularSpeed.")]
		public float angleResponseTime = 0.2f;
		[Header("Cover")]
		[Tooltip("Low cover height to consider crouch when taking cover.")]
		public float aboveCoverHeight = 1.5f;
		[Tooltip("The cover layer mask.")]
		public LayerMask coverMask;
		[Header("Shoot")]
		[Tooltip("Layer mask to cast shots.")]
		public LayerMask shotMask;
		[Tooltip("Layer mask of target(s).")]
		public LayerMask targetMask;
	}
}
