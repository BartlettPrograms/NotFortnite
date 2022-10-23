using Cinemachine;
using MoreMountains.Tools;
using PhysicsBasedCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AimZone : MonoBehaviour
{
    [SerializeField] private MMTouchButton _aimZone;
    [SerializeField] private InputReader input;
    [SerializeField] private CinemachineInputProvider cinemachineInput;

    [Header("TouchInputs")] 
    [SerializeField] private InputActionReference[] fingerDeltas;
    [SerializeField] private InputActionReference[] fingerPositions;
    [SerializeField] private InputActionReference[] fingerInContacts;

    [SerializeField] private Image[] Buttons;
    
    private bool aimZoneActive = false;
    private Rect aimZoneRect;

    private int aimFingerIndex;


    private void Start()
    {
        // Enable tracking touch controls
        foreach (InputActionReference fingerPosition in fingerPositions)
        {
            fingerPosition.action.Enable();
        }
        foreach (InputActionReference fingerInContact in fingerInContacts)
        {
            fingerInContact.action.Enable();
        }
        
        // Track aim zone size and location
        aimZoneRect = gameObject.GetComponent<RectTransform>().rect;
    }

    
    // Triggered when touch input detected on AimZone
    public void SetAimZoneActive()
    {
        // Enable camera turning
        cinemachineInput.enabled = true;
        
        // disable overlapping touch buttons
        foreach (Image button in Buttons)
        {
            button.raycastTarget = false;
        }
    }

    
    public void AimZoneActive()
    {
        int fingerI = 0;
        foreach (InputActionReference fingerPosition in fingerPositions)
        {
            if (fingerInContacts[fingerI].action.IsPressed())
            {
                if (fingerPosition.action.ReadValue<Vector2>().x > 1000f)
                {
                    cinemachineInput.XYAxis = fingerDeltas[fingerI];
                    // Debug.Log($"{cinemachineInput.XYAxis} - {fingerPosition.action.ReadValue<Vector2>().x}  >  {1000f}");

                }
            }
            
            fingerI++;
        }
    }
    
    
    public void SetAimZoneInactive()
    {
        // disable camera turning
        cinemachineInput.enabled = false;
        
        // enable touch buttons
        foreach (Image button in Buttons)
        {
            button.raycastTarget = true;
        }
    }
}
