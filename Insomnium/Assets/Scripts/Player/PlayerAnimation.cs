using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator playerAnimator;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        SetParameters();
    }

    private void SetParameters()
    {
        playerAnimator.SetFloat("VerticalSpeed", playerMovement.GetVerticalVelocity());
        playerAnimator.SetFloat("HorizontalSpeed", Mathf.Abs(playerMovement.GetHorizontalVelocity()));
        playerAnimator.SetBool("IsGrounded", playerMovement.GetIsGrounded());
    }
}
