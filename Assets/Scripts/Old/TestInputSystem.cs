using UnityEngine;
using UnityEngine.InputSystem;

public class TestInputSystem : MonoBehaviour
{

    private Rigidbody blobRB;
    private MovementActions playerInputActions;
    
    private Transform cameraTransform;
    
    private float rotationSpeed = 1.5f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        
        blobRB = GetComponent<Rigidbody>();
        playerInputActions = new MovementActions();
        playerInputActions.Gameplay.Enable();
        // performed == triggered once
        playerInputActions.Gameplay.Jump.performed += Jump;
    }

    private void Update()
    {
        // Player Movement
        Vector2 inputVector = playerInputActions.Gameplay.Movement.ReadValue<Vector2>();
        Vector3 move = new Vector3(inputVector.x, 0, inputVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        float speed = 5f;
        blobRB.AddForce(move * speed, ForceMode.Force);
        
        // Player Rotation towards camera direction
        //Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        blobRB.AddForce(Vector3.up * 5f, ForceMode.Impulse);
    }
}
