using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Experimental.Rendering.Universal;

public class RevealerTorch : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] float revealRadius = 2f;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float maxChargeTime = 2f;
    [SerializeField] float overUsedChargeDelay = 2f;
    private float chargeTime;
    private bool isFiring;
    private SpriteMask revealSpriteMask;
    public bool isOverUsed = false;

    [SerializeField] PlayerInput playerInput;
    private InputAction fireAction;

    [Header("Renderer")]
    [ColorUsage(true, true)]
    [SerializeField] Color litTorchColor;
    private Material torchMaterial;
    private Color torchColor;

    private Light2D torchLight;
    private float baseLightIntensity;

    private void Awake()
    {
        fireAction = playerInput.actions["Fire"];
        revealSpriteMask = GetComponent<SpriteMask>();
        torchMaterial = GetComponentInChildren<SpriteRenderer>().material;
        torchColor = torchMaterial.GetColor("_Color");

        torchLight = GetComponentInChildren<Light2D>();
        baseLightIntensity = torchLight.intensity;

        chargeTime = maxChargeTime;
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
        useCharge();
    }

    private void FireInputListener(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(value == 1) { isFiring = true; }
        else { isFiring = false; }
    }

    private void RevealEnemy()
    {
        // Take all collider inside overlapcirle
        Collider2D[] revealedEnemy = Physics2D.OverlapCircleAll(transform.position, revealRadius, enemyMask);
        
        if (isFiring && !isOverUsed)
        {
            revealSpriteMask.enabled = true;
            torchMaterial.SetColor("_Color", litTorchColor);
            torchLight.intensity = 1f;

            //Debug.Log("is Firing");
            foreach (Collider2D enemy in revealedEnemy)
            {
                enemy.GetComponent<DamageDealer>().Hit();
            }
        }
        else
        {
            revealSpriteMask.enabled = false;
            torchMaterial.SetColor("_Color", torchColor);
            torchLight.intensity = baseLightIntensity;

            foreach (Collider2D enemy in revealedEnemy)
            {
                enemy.GetComponent<DamageDealer>().UnHit();
            }
        }
    }

    private void useCharge()
    {
        if (isFiring && chargeTime > 0.01)
        {
            chargeTime -= Time.deltaTime;
            if (chargeTime <= 0.01)
            {
                Debug.Log("OverUsed");
                isOverUsed = true;
                StartCoroutine(overUsedCharge());
            }
        }
        else if(!isOverUsed && chargeTime < maxChargeTime)
        {
            chargeTime += Time.deltaTime;
        }

        chargeTime = Mathf.Clamp(chargeTime, -0.5f, maxChargeTime);
    }

    private IEnumerator overUsedCharge()
    {
        yield return new WaitForSeconds(overUsedChargeDelay);
        isOverUsed = false;
    }

    public float GetCharge()
    {
        return chargeTime;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, revealRadius);
    }
}
