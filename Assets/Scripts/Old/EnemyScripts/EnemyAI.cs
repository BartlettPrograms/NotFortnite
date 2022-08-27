/*using System;
using UnityEngine;
using CodeMonkey.Utils;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private PlayerToBall playerToBallScript;
    [SerializeField] private GameObject playerContainer;
    [SerializeField] private int damageDealt = 20;
    
    // Who to chase
    private GameObject player;

    private GameObject targetPlayer;

    // Define states the enemy can be in
    private enum State
    {
        Roaming,
        ChaseTarget,
        JustAttacked,
    }    
    
    private EnemyNavMesh pathfindingMovement;
    private PlayerHealth playerHealth;
    private Vector3 startingPosition;
    private Vector3 roamPosition;
    private State state;
    
    // Ranges to detect
    private float targetRange = 30f;
    private float attackRange = 20f;
    
    // Speeds
    private float dashSpeed = 25f;

    private void Awake()
    {
        pathfindingMovement = GetComponent<EnemyNavMesh>();
        playerHealth = playerContainer.GetComponent<PlayerHealth>();
        state = State.Roaming; // Roaming is Default state
    }
    
    private void Start()
    {
        GetRoamingPosition();
        startingPosition = transform.position;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            // Roam around script (default)
            case State.Roaming:
                Roaming();
                break;
            
            // Chase player Script
            case State.ChaseTarget:
                ChasePlayer();
                if (detectPossibleAttack())
                {
                    attack();
                }
                else stopAttack();
                break;
            
            // Case for when enemy has damaged player
            case State.JustAttacked:
                //stay still
                pathfindingMovement.MoveTo(transform.position);
                Invoke("Roaming", 2f);
                break;
        }
    }

    private void GetRoamingPosition()
    {
        // Calculate roaming position
        Invoke("GetRoamingPosition", Random.Range(2.5f, 5f));
        roamPosition = startingPosition + UtilsClass.GetRandomDir() * Random.Range(5f, 20f);
    }
    
    private void NextRoam()
    {
        // Move to roamPosition
        pathfindingMovement.MoveTo(roamPosition);
    }
    
    private void ChasePlayer()
    {
        // Move to Player
        pathfindingMovement.MoveTo(playerToBallScript.activeCharacterRef.transform.position);
    }

    // detect if enemy can "see" the player. If so, enemy enters chase mode
    private void FindTarget()
    {
        if (Vector3.Distance(transform.position, playerToBallScript.activeCharacterRef.transform.position) < targetRange)
        {
            // Player within target range
            state = State.ChaseTarget;
        } else if (Vector3.Distance(transform.position, playerToBallScript.activeCharacterRef.transform.position) < targetRange)
        {
            // Player within target range
            state = State.Roaming;
        }
    }

    // Detect if player is in attack range, if so enemy will attack player. no state for this right now
    private bool detectPossibleAttack()
    {
        if (Vector3.Distance(transform.position, playerToBallScript.activeCharacterRef.transform.position) < attackRange)
        {
            return true;
        }
        return false;
    }

    // Attacking functions
    private void attack()
    {
        pathfindingMovement.setSpeed(dashSpeed);
    }
    private void stopAttack()
    {
        pathfindingMovement.setSpeedToDefault();
    }
    
    // Damage player
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player")
        {
            playerHealth.takeDamage(damageDealt);
            //enemy to chill out for a second
            state = State.JustAttacked;
        }
    }

    private void Roaming()
    {
        NextRoam();
        FindTarget();
    }
}
*/