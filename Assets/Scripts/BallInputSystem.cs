using UnityEngine;
using UnityEngine.InputSystem;

public class BallInputSystem : MonoBehaviour
{

    private Rigidbody BallRB;
    private PlayerInput playerInput;
    private MovementActions playerInputActions;
    
    private Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        
        BallRB = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new MovementActions();
        playerInputActions.Gameplay.Enable();
        // performed == triggered once
        playerInputActions.Gameplay.Jump.performed += Jump;
    }

    private void Update()
    {
        Vector2 inputVector = playerInputActions.Gameplay.Movement.ReadValue<Vector2>();
        inputVector.x = inputVector.x * -1;
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        move = move.x * cameraTransform.forward.normalized + move.z * cameraTransform.right.normalized;
        //move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        float speed = 15f;
        BallRB.AddTorque(move * speed, ForceMode.Force);      //.AddForce(move * speed, ForceMode.Force);
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        BallRB.AddForce(Vector3.up * 18f, ForceMode.Impulse);
    }
}
