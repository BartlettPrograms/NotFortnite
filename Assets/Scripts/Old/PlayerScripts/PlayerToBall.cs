using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerToBall : MonoBehaviour
{
    //vcam cull: vCamController has been commented out.  
    
    private MovementActions movementActions;
    [SerializeField] private GameObject characterContainerStanding;
    [SerializeField] private GameObject characterContainerBall;
    [SerializeField] private GameObject characterBall;
    [SerializeField] private GameObject characterStanding;
    [SerializeField] CMCameraController cmCamController;
    [SerializeField] private GameObject slimeArmature;
    
    private Rigidbody rbBall;
    private Rigidbody rbStandingPlayer;

    private float characterHeight = 0.5f;
    private float playerMaxAngularVelocity = 35f;
    

    
    private void Awake()
    {
        movementActions = new MovementActions();
        movementActions.Enable();
        movementActions.Gameplay.Transform.performed += DoTransform;
        rbBall = characterBall.GetComponent<Rigidbody>();
        rbStandingPlayer = characterContainerStanding.GetComponent<Rigidbody>();
        
        // Gameplay Stuff
        Cursor.lockState = CursorLockMode.Locked;
        rbBall.maxAngularVelocity = playerMaxAngularVelocity;
    }

    private void Start()
    {

    }

    private void DoTransform(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            if (characterContainerStanding.activeInHierarchy)
            {
                // Transform into ball code here
                // align both characters with current position
                characterBall.transform.position = characterStanding.transform.position;
                // Sets balls velocity & anglr velocty
                rbBall.angularVelocity = Vector3.zero;
                standingVelocityToBall();
                // Unfuck the camera transition
                cmCamController.moveBallCamToAimCam();            // VCAM cull of 5/20
                // Swap active character
                characterContainerStanding.SetActive(false);
                characterContainerBall.SetActive(true);
                // Swap active cameras
                cmCamController.setCmVcNormalPriority(5);            // VCAM cull of 5/20

            }
            else if (characterContainerBall.activeInHierarchy)
            {
                // Transform into standing (slime)
                // I will try moving all bone positions to the player here...
                characterContainerStanding.transform.position = calcStandingPos();


                ballVelocityToStanding();
                
                characterContainerBall.SetActive(false);
                characterContainerStanding.SetActive(true);
                cmCamController.setCmVcNormalPriority(30);            // VCAM cull of 5/20
            }
        }
    }

    private Vector3 calcStandingPos()
    {
        Vector3 startPos = characterBall.transform.position;
        //startPos.y += 0.5f * characterHeight;
        return startPos;
    }

    public void killVelocityBall()
    {
        rbBall.angularVelocity = Vector3.zero;
        rbBall.velocity = Vector3.zero;
    }

    public void ballVelocityToStanding()
    {
        rbStandingPlayer.velocity = rbBall.velocity;
    }

    public void standingVelocityToBall()
    {
        rbBall.velocity = rbStandingPlayer.velocity;
    }
}