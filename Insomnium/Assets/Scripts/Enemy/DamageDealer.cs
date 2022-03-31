using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 20;
    [SerializeField] float durationToDestroy = 3f;
    [SerializeField] bool isReal = false;
    [SerializeField] ParticleSystem vanishParticle;
    private bool isRevealed = false;

    private AIDestinationSetter destination;

    [SerializeField] float slowedSpeed = 0.5f;
    private AIPath pathing;
    private float normalSpeed = 4f;

    private Transform revealedDestination;
    private AudioPlayer audioPlayer;
    private CapsuleCollider2D capsuleCollider;

    private RevealerTorch revealerTorch;

    private void Awake()
    {
        destination = GetComponent<AIDestinationSetter>();
        pathing = GetComponent<AIPath>();
        normalSpeed = pathing.maxSpeed;

        destination.target = GameObject.FindGameObjectWithTag("Player").transform;
        revealedDestination = GameObject.FindGameObjectWithTag("Destination").transform;

        audioPlayer = FindObjectOfType<AudioPlayer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        revealerTorch = FindObjectOfType<RevealerTorch>();
    }

    private void OnEnable()
    {
        revealerTorch.torchUnlit += UnHit;
    }

    private void OnDisable()
    {
        revealerTorch.torchUnlit -= UnHit;
    }

    public void Hit()
    {
        if (isReal)
        {
            durationToDestroy -= Time.deltaTime;
            pathing.maxSpeed = slowedSpeed;
            if (durationToDestroy <= 0)
            {
                PlayDieEffect();
                if (audioPlayer)
                {
                    audioPlayer.PlayVanishClip();
                }
                Destroy(gameObject);
            }
        }
        else if(!isReal)
        {
            isRevealed = true;
            destination.target = revealedDestination;
            capsuleCollider.isTrigger = true;
            //Debug.Log("is revealed : " + isRevealed);
        }
    }

    public void UnHit()
    {
        pathing.maxSpeed = normalSpeed;
    }

    private void PlayDieEffect()
    {
        if(vanishParticle != null)
        {
            ParticleSystem instance = Instantiate(vanishParticle, transform.position, Quaternion.identity);
            Destroy(instance.gameObject, (instance.main.duration + instance.main.startLifetime.constantMax) * 2); //instance.main.duration + instance.main.startLifetime.constantMax
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && !isRevealed)
        {
            Health playerHealth = collision.collider.GetComponent<Health>();
            playerHealth.TakeDamage(damage);
            PlayDieEffect();
            if (audioPlayer)
            {
                audioPlayer.PlayVanishClip();
            }
            Destroy(gameObject);
            //Debug.Log("Player Collide with enemy");
        }
    }

    private void OnBecameInvisible()
    {
        if (isRevealed)
        {
            Destroy(gameObject);
        }
        //Debug.Log("Invisible Called");
    }
}
