using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health = 100;
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) { Die(); }
    }

    private void Die()
    {
        Debug.Log("Die");
    }

    public int GetHealth()
    {
        return health;
    }
}
