using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RevealerTorch : MonoBehaviour
{
    private string realEnemyTag = "RealEnemy";
    private string fakeEnemyTag = "FakeEnemy";

    [SerializeField] float revealRadius = 2f;
    [SerializeField] LayerMask enemyMask;
    private bool isFiring;
    private SpriteMask revealSpriteMask;

    [SerializeField] PlayerInput playerInput;
    private InputAction fireAction;

    private void Awake()
    {
        fireAction = playerInput.actions["Fire"];
        revealSpriteMask = GetComponent<SpriteMask>();

    }

    private void OnEnable()
    {
        fireAction.performed += FireInputListener;
        fireAction.canceled += FireInputListener;
    }

    private void OnDisable()
    {
        fireAction.performed -= FireInputListener;
        fireAction.canceled -= FireInputListener;
    }

    private void Update()
    {
        RevealEnemy();
    }

    private void FireInputListener(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(value == 1) { isFiring = true; }
        else { isFiring = false; }
    }

    private void RevealEnemy()
    {
        if (isFiring)
        {
            revealSpriteMask.enabled = true;
            //Debug.Log("is Firing");
            Collider2D[] revealedEnemy = Physics2D.OverlapCircleAll(transform.position, revealRadius, enemyMask);
            foreach (Collider2D enemy in revealedEnemy)
            {
                enemy.GetComponent<DamageDealer>().Hit();
            }
        }
        else
        {
            revealSpriteMask.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }
}
