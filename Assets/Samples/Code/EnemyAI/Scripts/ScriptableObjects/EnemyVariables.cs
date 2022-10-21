namespace EnemyAI
{
	// Extra variables common to all NPC categories.
	[System.Serializable]
	public class EnemyVariables
	{
		// Feel Shot Decision
		public bool feelAlert;                     // Boolean to trigger when a feel alert is received by callback function.
		public bool hearAlert;                     // Boolean to trigger when a hear alert is received by callback function.
												   // Advance Cover Decision
		public bool advanceCoverDecision;          // The NPC will advance to a nearest cover, relative to the player?
		public int waitRounds;                     // Rounds to wait attacking, before consider advancing to another cover;
												   // Repeat Decision
		public bool repeatShot;                    // Repeat the attack (shot) action?
												   // Take Cover Action
		public float waitInCoverTimer;             // How long the NPC has already waited in cover this round.
		public float coverTime;                    // How long to wait in cover this round?
												   // Patrol Action
		public float patrolTimer;                  // How long the NPC has already waited in waypoint this round.
												   // Attack Action
		public float shotTimer;                    // Current shot time period (used to measure shot rate).
		public float startShootTimer;              // Start shoot delay timer.
		public float currentShots;                 // How many shots was given on the current round.
		public float shotsInRound;                 // How many shots (burst size) NPC will perform in the current round.
		public float blindEngageTimer;             // Current blind engaging timer (when player is out of sight).
	}
}
