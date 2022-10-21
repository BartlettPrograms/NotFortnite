using UnityEngine;

// This is a template script for in-game object health manager.
// Any in-game entity that reacts to a shot must have this script with the public function TakeDamage().
public class HealthManager : MonoBehaviour
{
	// Class to encapsulate damage parameters for the callback function.
	public class DamageInfo
	{
		public Vector3 location, direction;      // Hit location and direction.
		public float damage;                     // Damage ammount.
		public Collider bodyPart;               // The body part (Collider) that was hit (optional).
		public GameObject origin;                // The game object that generated the hit (optional).

		public DamageInfo(Vector3 location, Vector3 direction, float damage, Collider bodyPart=null, GameObject origin=null)
		{
			this.location = location;
			this.direction = direction;
			this.damage = damage;
			this.bodyPart = bodyPart;
			this.origin = origin;
		}
	}

	[HideInInspector] public bool dead;          // Is this entity dead?

	// This is the mandatory function that receives damage from shots.
	// You may remove the 'virtual' keyword before coding the content.
	public virtual void TakeDamage(Vector3 location, Vector3 direction, float damage, Collider bodyPart=null, GameObject origin=null)
	{
		Debug.Log("Ow!");
	}

	// This is the message receiver for damage taken by a child gameObject rigidbody (ex.: ragdoll)
	public void HitCallback(DamageInfo damageInfo)
	{
		this.TakeDamage(damageInfo.location, damageInfo.direction, damageInfo.damage, damageInfo.bodyPart, damageInfo.origin);
	}
}
