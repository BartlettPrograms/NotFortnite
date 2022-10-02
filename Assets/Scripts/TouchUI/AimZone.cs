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
    

    private bool AimZoneActive = false;
    private Rect aimZoneSize;


    private void Start()
    {
        aimZoneSize = _aimZone.gameObject.GetComponent<RectTransform>().rect;
        //Debug.Log($"X: {aimZoneSize.width}  -|-  Y: {aimZoneSize.height}  -|-  Pos: {aimZoneSize.position}");
    }

    // Update is called once per frame
    void Update()
    {
        // If touching swipe zone turn aim controller on (this is on the cm vcam controller)
        if (AimZoneActive)
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
        AimZoneActive = true;
        // We should st the inputaction reference of the camera rotation to the finger that touched the aim zone
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            if (touch.startScreenPosition.x > aimZoneSize.position.x)
            {
                //Debug.Log("Aimzone TouchID: " + UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count);
                // Sets the camera controls to the approximate touch input that selected the aim zone. Works most of the time.
                cinemachineInput.XYAxis = fingerID[UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count - 1];
            }
        }

        
    }

    public void SetAimZoneInactive()
    {
        AimZoneActive = false;
    }
}
