using UnityEngine;

public class GunShooter : MonoBehaviour
{
    private GunSystemv2 gun;

    public void gunTrigger()
    {
        if (gun)
        {
            gun.gunTrigger();
        }
        else
        {
            gun = gameObject.GetComponentInChildren<GunSystemv2>();
            if (gun)
                gun.gunTrigger();
        }
    }
    public void stopGunTrigger()
    {
        if (gun)
            gun.stopGunTrigger();
    }
}
