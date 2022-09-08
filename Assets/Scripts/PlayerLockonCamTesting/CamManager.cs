using System;
using PhysicsBasedCharacterController;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public InputReader input;

    [SerializeField]
    private GameObject cmVcamNormal;
    [SerializeField] private CompCameraController compCamera;

    private bool _lockedOn;
    private bool toggleReleased = true;
    
    public bool LockedOn { get => _lockedOn; }

    private void Update()
    {
        ToggleLockOn();
        checkLockOn();
    }


    private void checkLockOn()
    {
        // Toggle Lock on
        // We want to check if enemies are in range before we disable VCameras.
        if (_lockedOn && cmVcamNormal.activeInHierarchy && compCamera.CheckEnemyTargets())
        {
            cmVcamNormal.SetActive(false);
        } else if (!_lockedOn && !cmVcamNormal.activeInHierarchy)
        {
            cmVcamNormal.SetActive(true);
        } else if (!compCamera.LockedOn)
        {
            _lockedOn = false;
            cmVcamNormal.SetActive(true);
        }
    }


    public void ToggleLockOn()
    {
        // Toggle Lock on
        if (input.targetLock && toggleReleased)
        {
            if (_lockedOn)
            {
                compCamera.RemoveTarget();
            }
            _lockedOn = !_lockedOn;
            toggleReleased = false;
        }
        else if (!input.targetLock) toggleReleased = true;
    }
}
