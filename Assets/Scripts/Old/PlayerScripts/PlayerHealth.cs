using System;
using BlortNet;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    private int currentHealth;
    [SerializeField] int armour = 10;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private YouDied deathText;
    [SerializeField] private KillPlayerControls controlsController;

    public bool playerDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(currentHealth);
    }

    private void Update()
    {
        // Reset level. Hard coded for now
        if (!playerDead && currentHealth < 1)
        {
            playerDead = true;
            playerDied();
        }
    }

    private void playerDied()
    {
        deathText.displayDeathText();
        // cut off controls
        controlsController.KillControls();
        Invoke("resetLevel", 4f);
    }

    private void resetLevel()
    {
        SceneManager.LoadScene("Full Level Prototype");
    }
    

    public void takeDamage(int damage)
    {
        int damageTaken = damage - armour;
        currentHealth -= damageTaken;
        healthBar.SetHealth(currentHealth);
        Debug.Log("Oof!");
    }

    public void resetHealth()
    {
        currentHealth = 100;
    }
}
