using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float durationToDestroy = 3f;
    [SerializeField] bool isReal = false;
    private bool isRevealed = false;

    public void Hit()
    {
        if (isReal)
        {
            durationToDestroy -= Time.deltaTime;
            if (durationToDestroy <= 0)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            isRevealed = true;
            //Debug.Log("is revealed : " + isRevealed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isRevealed)
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);
            //Debug.Log("Player Collide with enemy");
        }
    }
}
