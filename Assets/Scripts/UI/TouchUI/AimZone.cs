using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private Image[] Buttons;


    private bool aimZoneActive = false;
    private Rect aimZoneRect;

    private int aimFingerIndex;


    private void Start()
    {
        foreach (InputActionReference fingerPosition in fingerPositions)
        {
            // probably take this out later
            fingerPosition.action.Enable();
        }
        aimZoneRect = gameObject.GetComponent<RectTransform>().rect;
        //Debug.Log($"X: {aimZoneRect.width}  -|-  Y: {aimZoneRect.height}  -|-  Pos: {aimZoneRect.position}");
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
            if (fingerPosition.action.ReadValue<Vector2>().x > 1000f)
            {
                Debug.Log($"{fingerPosition.action.ReadValue<Vector2>().x}  >  {1000f}");
                cinemachineInput.XYAxis = fingerDeltas[fingerI];
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
