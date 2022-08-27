using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCam : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Canvas thirdPersonCanvas;
    [SerializeField] private Canvas aimCanvas;
    [SerializeField] private GameObject vCamContainer;

    private CMCameraController cameraController;
    private InputAction aimAction;

    private void Awake()
    {
        aimAction = playerInput.actions["Aim"];
        cameraController = vCamContainer.GetComponent<CMCameraController>();
    }

    private void OnEnable()
    {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable()
    {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim()
    {
        cameraController.setCmVcAimPriority(19);
        //aimCanvas.enabled = true;
        //thirdPersonCanvas.enabled = false;
    }

    private void CancelAim()
    {
        cameraController.setCmVcAimPriority(9);
        //aimCanvas.enabled = false;
        //thirdPersonCanvas.enabled = true;
    }
}
