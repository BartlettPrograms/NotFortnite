using Cinemachine;
using UnityEngine;

public class CMCameraController : MonoBehaviour
{
    
    public static CMCameraController Instance { get; private set; }
    
    // Virtual Camera Objects
    [SerializeField] private GameObject cmFreelookGameObject;
    [SerializeField] private GameObject tpVCNormalGameObject;
    //[SerializeField] private GameObject tpVCAimGameObject;

    // Virtual Camera data fields
    private CinemachineFreeLook cmFreelook;
    private CinemachineVirtualCamera cmVCNormal;
    private CinemachineVirtualCamera cmVCAim;
    
    // Camera Shaker properties
    private float shakeTimer;
    private CinemachineBasicMultiChannelPerlin normalCamShaker;
    private CinemachineBasicMultiChannelPerlin aimCamShaker;

    void Awake()
    {
        Instance = this;
        // Virtual Cameras
        cmFreelook = cmFreelookGameObject.GetComponent<CinemachineFreeLook>();
        cmVCNormal = tpVCNormalGameObject.gameObject.GetComponent<CinemachineVirtualCamera>();
        //cmVCAim = tpVCAimGameObject.gameObject.GetComponent<CinemachineVirtualCamera>();
        
        // camera shaker objects
        //normalCamShaker = cmVCNormal.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //aimCamShaker = cmVCAim.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        // Countdown to reduce CameraShake
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            // Right now code will take perlin noise to 0. however, we want to leave a little bit of noise
            if (shakeTimer <= 0f)
            {
                // Timer over!
                normalCamShaker.m_AmplitudeGain = 0f;
                normalCamShaker.m_FrequencyGain = 0f;
                aimCamShaker.m_AmplitudeGain = 0f;
                aimCamShaker.m_FrequencyGain = 0f;
            }
        }
    }

    public void setFreelookPriority(int setTo)
    {
        cmFreelook.Priority = setTo;
    }

    public void setCmVcNormalPriority(int setTo)
    {
        cmVCNormal.Priority = setTo;
    }

    public void setCmVcAimPriority(int setTo)
    {
        cmVCAim.Priority = setTo;
    }

    public void moveBallCamToAimCam()
    {
        cmFreelook.transform.position = cmVCNormal.transform.position;
    }

    public void cameraShake(float amplitude, float frequency, float time)
    {
        normalCamShaker.m_AmplitudeGain = amplitude;
        normalCamShaker.m_FrequencyGain = frequency;
        aimCamShaker.m_AmplitudeGain = amplitude;
        aimCamShaker.m_FrequencyGain = frequency;

        shakeTimer = time;
    }
}
