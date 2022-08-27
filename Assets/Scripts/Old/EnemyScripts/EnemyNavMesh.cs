using System;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavMesh : MonoBehaviour
{
    [SerializeField] private Transform homePosition;
    
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        // Save NavMesh Agent
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        
    }

    public void MoveTo(Vector3 targetPosition)
    {
        if (navMeshAgent)
        {
            // Sets destination to a provided position
            navMeshAgent.destination = targetPosition;
        }
    }

    public void GoHome()
    {
        // Go to Home Position
        navMeshAgent.destination = homePosition.position;
    }

    public void setSpeed(float speed)
    {
        navMeshAgent.speed = speed;
    }

    public void setSpeedToDefault()
    {
        navMeshAgent.speed = 6f; // Hardcoded because I'm a baby
    }
}
