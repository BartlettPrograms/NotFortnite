
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private int health = 30;
    [SerializeField] private int armour;
    [SerializeField] private GameObject enemy;

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken - armour;
        if (health <= 0)
        {
            Destroy(enemy);
        }
    }
}
