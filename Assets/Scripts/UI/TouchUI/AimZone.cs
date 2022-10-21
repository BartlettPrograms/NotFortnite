using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using MoreMountains.Tools;
using PhysicsBasedCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class AimZone : MonoBehaviour
{
    [SerializeField] private MMTouchButton _aimZone;
    [SerializeField] private InputReader input;
    [SerializeField] private CinemachineInputProvider cinemachineInput;

    [Header("TouchDeltaInputs")] 
    [SerializeField] private InputActionReference[] fingerID;
    

    private bool aimZoneActive = false;
    private Rect aimZoneSize;

    private int aimFingerIndex;


    private void Start()
    {
        aimZoneSize = gameObject.GetComponent<RectTransform>().rect;
        //Debug.Log($"X: {aimZoneSize.width}  -|-  Y: {aimZoneSize.height}  -|-  Pos: {aimZoneSize.position}");
    }

    // Update is called once per frame
    void Update()
    {
        // If touching swipe zone turn aim controller on (this is on the cm vcam controller)
        if (aimZoneActive)
        {
            //Debug.Log(input.cameraInput);
            cinemachineInput.enabled = true;
        }
        // Not touching aim zone = aim controls disabled
        else cinemachineInput.enabled = false;
    }

    // Triggered when touch input detected on AimZone
    public void SetAimZoneActive()
    {
        aimZoneActive = true;
        // We should st the inputaction reference of the camera rotation to the finger that touched the aim zone
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.startScreenPosition.x > aimZoneSize.position.x)
            {
                Debug.Log("Aimzone TouchID: " + (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count - 1));
                // Sets the camera controls to the approximate touch input that selected the aim zone.
                // Works at the start, needs to be pulled through with an update() function to secure touch ID
                cinemachineInput.XYAxis = fingerID[touch.finger.index];
            }
        }
    }

    public void AimZoneActive()
    {
        /*
         * find finger with the most right screenPosX value
         */
        // float maxX = 0f;
        // aimFingerIndex = 0;
        
        // Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.fingers[0].screenPosition);
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.finger.screenPosition.x > aimZoneSize.position.x)
            {
                //Debug.Log($"{UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count} active touches. Current Touch index: {touch.finger.index}");
                //Debug.Log($"touch.startScreenPos: {touch.startScreenPosition} | {touch.finger.screenPosition}");

                // Sets the camera controls to...
                cinemachineInput.XYAxis = fingerID[touch.finger.index];
            }
        }
    }
    public void SetAimZoneInactive()
    {
        aimZoneActive = false;
    }
}
