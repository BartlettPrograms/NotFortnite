using UnityEngine;
using UnityEngine.InputSystem;

public class SlimeInputSystem : MonoBehaviour
{

    private Rigidbody[] blobRBs;
    private MovementActions playerInputActions;
    
    private Transform cameraTransform;

    [SerializeField] private float speed = 28f;
    [SerializeField] private float jumpForce = 25f;
    private float rotationSpeed = 1.5f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        blobRBs = gameObject.transform.GetChild(0).gameObject.GetComponentsInChildren<Rigidbody>();//GetComponent<Rigidbody>();
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
        
        foreach (Rigidbody rb in blobRBs)
        {
            rb.AddForce(move * speed, ForceMode.Force);
        }
        
        
        // Player Rotation towards camera direction
        //Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    
    private void Jump(InputAction.CallbackContext context)
    {
        foreach (Rigidbody rb in blobRBs)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
