using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDisplay : MonoBehaviour
{
    GameObject player;

    [Header("Health")]
    [SerializeField] Slider healthSlider;
    Health playerHealth;

    [Header("Torch")]
    [SerializeField] Slider torchSlider;
    [SerializeField] Image torchIcon;
    [SerializeField] Color overUsedColor;
    Color normalTorchColor;
    RevealerTorch playerTorch;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        playerTorch = player.GetComponentInChildren<RevealerTorch>();
        normalTorchColor = torchIcon.color;
    }

    private void Start()
    {
        healthSlider.maxValue = playerHealth.GetHealth();
        torchSlider.maxValue = playerTorch.GetCharge();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateCharge();
    }

    private void UpdateHealth()
    {
        healthSlider.value = playerHealth.GetHealth();
    }

    private void UpdateCharge()
    {
        torchSlider.value = playerTorch.GetCharge();
        if (playerTorch.isOverUsed) { torchIcon.color = overUsedColor; }
        else { torchIcon.color = normalTorchColor; }

    }
}
