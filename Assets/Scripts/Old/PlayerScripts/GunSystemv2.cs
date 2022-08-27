using UnityEngine;
using UnityEngine.InputSystem;

public class GunSystemv2 : MonoBehaviour
{
    //Gun stats
    [SerializeField] private int damage;
    [SerializeField] private float bulletSpeed = 120f;
    [SerializeField] private float shootIntoAirDistance, timeBetweenShooting, spread, range, reloadTime, timeBetweenBFShots; // timeBetweenShots = Burstfire variable. timeBetweenShooting = Automatic variable.
    [SerializeField] private int magazineSize, bulletsPerTap;
    [SerializeField] private bool allowButtonHold;
    private int bulletsLeft, bulletsShot;
    
    //bools
    private bool isShooting, readyToShoot, reloading;
    private bool triggerExtra = true; // for event that only happens once while trigger hold

    //Extra Graphics
    [SerializeField] private GameObject bulletHoleGraphic;
    [SerializeField] private GameObject muzzleFlashGraphic;
    [SerializeField] private GameObject bulletPrefab;
    
    // Bullet stuff
    [SerializeField] private Transform bulletParent;

    //Reference
    [SerializeField] private Camera fpsCam;
    private Transform fpsCamTransform;
    [SerializeField] private Transform attackPoint;
    private RaycastHit rayHit;
    
    //Audio
    private AudioSource audio;
    [SerializeField] private AudioClip reloadingAudio;
    [SerializeField] private AudioClip gunshotAudio;
    [SerializeField] private AudioClip noAmmoAudio;

    private void Awake()
    {
        // Instantiate and turn on controls
        NewControlScheme playerInputActions = new NewControlScheme();
        playerInputActions.Enable();
        
        // Setup Controls here
        // assign function to Shoot Control
        playerInputActions.Player.Shoot.performed += gunTrigger;
        playerInputActions.Player.Shoot.canceled += stopGunTrigger;
        // assign function to Reload Control
        playerInputActions.Player.Reload.performed += reloadAction;

        // Gun stats setup
        bulletsLeft = magazineSize; // fill with ammo
        readyToShoot = true;
        reloading = false;

        fpsCamTransform = fpsCam.transform;
        
        // Assign audio
        audio = gameObject.GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Fire Ammo
        if (readyToShoot && isShooting && !reloading && bulletsLeft > 0)
        {
            shoot();
        }
        // No ammo, trigger pulled
        if (bulletsLeft <= 0 && triggerExtra && readyToShoot && !reloading)
        {
            audio.clip = noAmmoAudio;
            audio.Play();           // I want this to trigger as soon as mouse clicks, not on release
            triggerExtra = false;
        }
    }

    //Shooting script
    private void shoot()
    {
        if (!allowButtonHold) isShooting = false; // Deactivate trigger if weapon semi-automatic
        readyToShoot = false;   // Deactivate chamber for bullet reload
        
        //Shooting Physics here
        for (int i = 0; i < bulletsPerTap; i++)
        {
            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);
            // Calculate Direction with Spread
            Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, z);
            
            
            
            
            
            //Shoot Raycast
            RaycastHit rayHit;
            GameObject bullet = GameObject.Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity, bulletParent);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, Mathf.Infinity))
            {
                bulletController.target = rayHit.point;
                bulletController.hit = true;
                bulletController.damage = damage;
                bulletController.bulletSpeed = bulletSpeed;
            }
            else
            {
                bulletController.target = fpsCamTransform.position + fpsCamTransform.forward * shootIntoAirDistance;
                bulletController.hit = true;
                bulletController.damage = damage;
                bulletController.bulletSpeed = bulletSpeed;
            }
        }
        
        
        
        
        
        //Shake Camera
        CMCameraController.Instance.cameraShake(0.5f, 7f,  .135f);
        
        // Muzzle flash Graphics
        Instantiate(muzzleFlashGraphic, attackPoint.position, Quaternion.identity, attackPoint);
        
        // Audio
        audio.Play();

        bulletsLeft--;
        Invoke("resetShot", timeBetweenShooting);
    }
    
    //Activate or Deactivate trigger from input
    private void gunTrigger(InputAction.CallbackContext context)
    {
        isShooting = true;
        triggerExtra = true;
    }
    private void stopGunTrigger(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    //Reload when given the input
    private void reloadAction(InputAction.CallbackContext context)
    {
        if (bulletsLeft < magazineSize && !reloading)
        {
            reloading = true;
            Invoke("reloadGun", reloadTime);
        }
    }

    private void reloadGun()
    {
        audio.clip = reloadingAudio;
        audio.Play();
        Invoke("reloadComplete", audio.clip.length);
        
    }

    private void reloadComplete()
    {
        setBulletAudio();
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
    // Reset the chamber, ready to fire another shot
    private void resetShot()
    {
        readyToShoot = true;
    }

    private void setBulletAudio()
    {
        audio.clip = gunshotAudio;
    }
}
