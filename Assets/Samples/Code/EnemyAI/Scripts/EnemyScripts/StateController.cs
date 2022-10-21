using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EnemyAI
{
	// This class controls the NPC Finite State Machine (FSM).
	public class StateController : MonoBehaviour
	{
		[Tooltip("NPC common stats.")]
		public GeneralStats generalStats;
		[Tooltip("NPC class specific stats.")]
		public ClassStats classStats;
		[Space(10)]
		[Tooltip("Current NPC FSM state.")]
		public State currentState;
		[Tooltip("Dummy state reference, used by FSM decisions when not transitioning.")]
		public State remainState;
		[Space(10)]
		[Tooltip("Target reference for aim.")]
		public Transform aimTarget;
		[Tooltip("The location waypoints to patrol.")]
		public List<Transform> patrolWayPoints;
		[Tooltip("Current bullets on weapon mag.")]
		public int bullets;
		[Space(10)]
		[Tooltip("Radius of NPC Field of View (FOV) area.")]
		[Range(0, 50)] public float viewRadius;
		[Tooltip("Angle of NPC FOV area.")]
		[Range(0, 360)] public float viewAngle;
		[Tooltip("Radius of NPC perception area.")]
		[Range(0, 25)] public float perceptionRadius;

		[HideInInspector] public float nearRadius;                  // Radius of NPC near area.
		[HideInInspector] public NavMeshAgent nav;                  // Reference to the NPC NavMesh agent.
		[HideInInspector] public int waypointIndex;                 // Reference to current waypoint.
		[HideInInspector] public int maximumBurst = 7;              // The maximum burst size on a round.
		[HideInInspector] public float blindEngageTime = 30f;       // Time to keep targeting last seen position after target leaves sight.
		[HideInInspector] public bool targetInSight;                // Is target on sight?
		[HideInInspector] public bool focusSight;                   // Will focus on sight position?
		[HideInInspector] public bool reloading;                    // Is the NPC reloading?
		[HideInInspector] public bool hadClearShot;                 // The NPC had a clear sight of the target to shoot before?
		[HideInInspector] public bool haveClearShot;                // The NPC has a clear sight of the target to shoot now?
		[HideInInspector] public int coverHash = -1;                // Unique reference to the used cover, if any;
		[HideInInspector] public EnemyAnimation enemyAnimation;     // Reference to the enemy animation script.
		[HideInInspector] public EnemyVariables variables;          // Reference to extra variables, common to all NPC categories.
		[HideInInspector] public CoverLookup coverLookup;           // Reference to the Game Controller's cover lookup script.
		[HideInInspector] public Vector3 personalTarget;            // The current personal target, if any.

		private int magBullets;                                     // Maximum bullet capacity of the weapon mag.
		private bool aiActive;                                      // Is the NPC AI active?
		private static Dictionary<int, Vector3> coverSpot;          // The cover position for each NPC, if any.
		private bool strafing;                                      // Is the NPC strafing?
		private bool aiming;                                        // Is the NPC aiming?
		private bool checkedOnLoop, blockedSight;                   // Blocked sight test related variables.

		// Reset cover position.
		private void OnDestroy()
		{
			coverSpot.Remove(this.GetHashCode());
		}

		// Get and Set current cover spot.
		public Vector3 CoverSpot
		{
			get
			{
				return coverSpot[this.GetHashCode()];
			}
			set
			{
				coverSpot[this.GetHashCode()] = value;
			}
		}

		// Get and Set for strafing and aiming states.
		public bool Strafing
		{
			get => strafing;
			set
			{
				enemyAnimation.anim.SetBool("Strafe", value);
				strafing = value;
			}
		}
		public bool Aiming
		{
			get => aiming;
			set
			{
				if (aiming != value)
				{
					enemyAnimation.anim.SetBool("Aim", value);
					aiming = value;
				}
			}
		}

		// Liberate aim for a short period of time, alowing NPC body realignment.
		public IEnumerator UnstuckAim(float delay)
		{
			Aiming = false;
			yield return new WaitForSeconds(delay);
			Aiming = true;
		}

		void Awake()
		{
			// Setup the references.
			if (coverSpot == null)
				coverSpot = new Dictionary<int, Vector3>();
			coverSpot[this.GetHashCode()] = Vector3.positiveInfinity;
			nav = GetComponent<NavMeshAgent>();
			aiActive = true;
			enemyAnimation = this.gameObject.AddComponent<EnemyAnimation>();
			magBullets = bullets;
			variables.shotsInRound = maximumBurst;
			// Near sense radius is half of perception radius.
			nearRadius = perceptionRadius / 2;
			// Get/create Game Controller.
			GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
			if (gameController == null)
			{
				gameController = new GameObject("GameController")
				{
					tag = "GameController"
				};
			}
			// Attach cover lookup component to Game Controller and/or get reference.
			coverLookup = gameController.GetComponent<CoverLookup>();
			if (coverLookup == null)
			{
				coverLookup = gameController.AddComponent<CoverLookup>();
				coverLookup.Setup(generalStats.coverMask);
			}
			// Ensure the target has a health manager component to receive shots.
			Debug.Assert(aimTarget.root.GetComponent<HealthManager>(), "You must add a health manager to the target");
		}

		public void Start()
		{
			// Trigger initial state enable function.
			currentState.OnEnableActions(this);
		}

		void Update()
		{
			// Reset blocked sight test on current game loop iteration.
			checkedOnLoop = false;
			// Do not execute FSM if AI is not active.
			if (!aiActive)
				return;
			// Execute current FSM state actions.
			currentState.DoActions(this);
			// Check current FSM state transition conditions.
			currentState.CheckTransitions(this);
		}

		// Change the current FSM state (called externally).
		public void TransitionToState(State nextState, Decision decision)
		{
			if (nextState != remainState)
			{
				// DEBUG: show state transitions for NPC.
				//Debug.Log(transform.name + " :" + decision.name + " : " + currentState.name + "->" + nextState.name);
				currentState = nextState;
			}
		}

		// DEBUG: Draw orb above NPC to indicate the current FSM state category (editor only).
		private void OnDrawGizmos()
		{
			if (currentState != null)
			{
				Gizmos.color = currentState.sceneGizmoColor;
				Gizmos.DrawWireSphere(transform.position + Vector3.up * 2.5f, .2f);
			}
		}

		// End the reload action (called by animator controller).
		public void EndReloadWeapon()
		{
			reloading = false;
			bullets = magBullets;
		}

		// This is the message receiver for alert events triggered by nearby objects (ex.: other NPC alert about a noise).
		public void AlertCallback(Vector3 target)
		{
			if (!aimTarget.root.GetComponent<HealthManager>().dead)
			{
				this.variables.hearAlert = true;
				this.personalTarget = target;
			}
		}

		// Verify if the spot is near any spot used by other NPCs. Default comparison distance is 1.
		public bool IsNearOtherSpot(Vector3 spot, float margin = 1f)
		{
			foreach (KeyValuePair<int, Vector3> usedSpot in coverSpot)
			{
				if (usedSpot.Key != gameObject.GetHashCode() && Vector3.Distance(spot, usedSpot.Value) <= margin)
					return true;
			}
			return false;
		}

		// The common cast to target test, used by decisions that is based on NPC senses.
		public bool BlockedSight()
		{
			// The test was already performed on that game loop iteration?
			if (!checkedOnLoop)
			{
				checkedOnLoop = true;
				Vector3 target = default;
				try
				{
					target = aimTarget.position;
				}
				catch (UnassignedReferenceException)
				{
					// Ensure the NPC has an aim target set.
					Debug.LogError("Assign an aim target to " + transform.name);
				}
				// Get cast to target parameters.
				Vector3 castOrigin = transform.position + Vector3.up * generalStats.aboveCoverHeight;
				Vector3 dirToTarget = target - castOrigin;

				// Hit anything other than target? Uses cover and obstacle masks.
				blockedSight =
					Physics.Raycast(castOrigin, dirToTarget, out RaycastHit hit, dirToTarget.magnitude,
					generalStats.coverMask | generalStats.obstacleMask);
			}
			return blockedSight;
		}
	}
}