using UnityEngine;
using UnityEngine.AI;


namespace BlortNet
{
    public class EnemyAITutorial : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerToBall playerToBallScript;
        [SerializeField] private float walkPointRange;
        private bool walkPointSet;
    }
}


// pathfindingMovement.MoveTo(playerToBallScript.activeCharacterRef.transform.position);