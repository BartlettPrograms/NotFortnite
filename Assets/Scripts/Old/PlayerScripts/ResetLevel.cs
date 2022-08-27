using UnityEngine;
using UnityEngine.InputSystem;

public class ResetLevel : MonoBehaviour
{
    [SerializeField] 
    private GameObject characterBall;
    [SerializeField] 
    private GameObject characterStanding;
    [SerializeField]
    private GameObject spawnPoint;
    private Rigidbody rbBall;
    private Rigidbody rbPlayer;

    private Vector3 respawnPosStanding;
    private Vector3 respawnPosBall;

    private void Awake()
    {
        rbBall = characterBall.GetComponent<Rigidbody>();
        rbPlayer = characterStanding.GetComponent<Rigidbody>();
        
        
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        playerInputActions.Player.ResetLevel.performed += DoReset;

        respawnPosStanding.x = spawnPoint.transform.position.x;
        respawnPosStanding.y = spawnPoint.transform.position.y + 1f;
        respawnPosStanding.z = spawnPoint.transform.position.z;
        
        respawnPosBall.x = spawnPoint.transform.position.x;
        respawnPosBall.y = spawnPoint.transform.position.y + 0.5f;
        respawnPosBall.z = spawnPoint.transform.position.z;
    }

    private void Start()
    {
        characterStanding.transform.position = respawnPosStanding;
    }

    private void Update()
    {
        
    }

    private void DoReset(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (characterBall.activeInHierarchy)
            {
                rbBall.angularVelocity = Vector3.zero;
                rbBall.velocity = Vector3.zero;
                characterBall.transform.position = respawnPosBall;
            } else if (characterStanding.activeInHierarchy)
            {
                rbPlayer.velocity = Vector3.zero;
                characterStanding.transform.position = respawnPosStanding;
            }
        }
    }
}
