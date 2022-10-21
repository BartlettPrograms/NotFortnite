using System.Collections;
using UnityEngine;
using EnemyAI;

// This class is created for the example scene. There is no support for this script.
public class PlayerShooting : MonoBehaviour
{
	public Transform shotOrigin, drawShotOrigin;
	public LayerMask shotMask;
	public WeaponMode weaponMode = WeaponMode.SEMI;
	public int RPM = 600;
	public enum WeaponMode
	{
		SEMI,
		AUTO
	}

	private LineRenderer laserLine;
	private float weaponRange = 100f;
	private float bulletDamage = 10f;
	private bool canShot;

	private AudioSource gunAudio;

	private WaitForSeconds halfShotDuration;// = new WaitForSeconds(0.06f);

    // Start is called before the first frame update
    void Start()
    {
		laserLine = GetComponent<LineRenderer>();
		gunAudio = GetComponent<AudioSource>();
		canShot = true;
		float waitTime = 60f / RPM;
		halfShotDuration = new WaitForSeconds(waitTime/2);
	}

    // Update is called once per frame
    void Update()
    {
		if(weaponMode == WeaponMode.SEMI && Input.GetButtonDown("Fire1") && canShot)
		{
			Shoot();
		}
		else if(weaponMode == WeaponMode.AUTO && Input.GetButton("Fire1") && canShot)
		{
			Shoot();
		}
    }

	void Shoot()
	{
		StartCoroutine(ShotEffect());
		laserLine.SetPosition(0, drawShotOrigin.position);
		Physics.SyncTransforms();
		if (Physics.Raycast(shotOrigin.position, shotOrigin.forward, out RaycastHit hit, weaponRange, shotMask))
		{
			laserLine.SetPosition(1, hit.point);

			// Call the damage behaviour of target if exists.
			if(hit.collider)
				hit.collider.SendMessageUpwards("HitCallback", new HealthManager.DamageInfo(hit.point, shotOrigin.forward, bulletDamage, hit.collider), SendMessageOptions.DontRequireReceiver);
		}
		else
			laserLine.SetPosition(1, drawShotOrigin.position + (shotOrigin.forward * weaponRange));

		// Call the alert manager to notify the shot noise.
		GameObject.FindGameObjectWithTag("GameController").SendMessage("RootAlertNearby", shotOrigin.position, SendMessageOptions.DontRequireReceiver);
	}

	private IEnumerator ShotEffect()
	{
		gunAudio.Play();
		// Turn on our line renderer
		laserLine.enabled = true;
		canShot = false;

		yield return halfShotDuration;

		// Deactivate our line renderer after waiting
		laserLine.enabled = false;

		yield return halfShotDuration;

		if (weaponMode == WeaponMode.SEMI)
		{
			yield return halfShotDuration;
			yield return halfShotDuration;
		}

		canShot = true;
	}

	// Player dead callback.
	public void PlayerDead()
	{
		canShot = false;
	}
}
