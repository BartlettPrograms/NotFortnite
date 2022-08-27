using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletDecal;
    
    private float timeToDestroy = 3f;

    // Where to make bullet travel to
    public Vector3 target { get; set; }
    public bool hit { get; set; }
    public int damage { get; set; }
    public float bulletSpeed { get; set; }

    private void OnEnable()
    {
        // Destroy after 3 seconds
        Destroy(gameObject, timeToDestroy);
    }

    void Update()
    {
        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, target, bulletSpeed * Time.deltaTime);
        // When touch target (get real close), destroy game object
        if (!hit && Vector3.Distance(transform.position, target) < .01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ContactPoint contact = other.GetContact(0);
        GameObject gameObjectHit = contact.otherCollider.gameObject;

        // If bullet collide with enemy, damage it
        if (gameObjectHit.CompareTag("Enemy"))
        {
            gameObjectHit.GetComponent<EnemyScript>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!gameObjectHit.CompareTag("Bullet"))
        {
            GameObject.Instantiate(bulletDecal, contact.point + contact.normal * .0001f, Quaternion.LookRotation(contact.normal));
            Destroy(gameObject);
        }
    }
}
