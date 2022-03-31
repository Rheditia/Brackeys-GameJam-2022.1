using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 100;
    int health;
    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
        health = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) { Die(); }
    }

    private void Die()
    {
        levelManager.LoadGameOver();
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return health;
    }
}
