using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction jumpAction;
    private InputAction moveAction;
    private Vector2 moveInput;

    private Rigidbody2D myRigidbody;

    [Header("Move Stat")]
    [SerializeField] float moveSpeed = 0.25f;
    [SerializeField] float maxHorizontalSpeed = 3f;
    [SerializeField] float maxVerticalSpeed = 8f;
    [SerializeField] float hVelocityDampingWhenStopping = 0.5f;
    [SerializeField] float hVelocityDampingWhenTurning = 0.5f;

    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float jumpBufferDuration = 0.2f;
    [SerializeField] float jumpBufferCountdown = 0f;
    [SerializeField] float coyoteTimeDuration = 0.2f;
    [SerializeField] float coyoteTimeCountdown = 0f;

    [Header("Ground Check")]
    [SerializeField] Vector2 boxCastSize = new Vector2(0.5f, 0.5f);
    [SerializeField] float groundCheckDistance = 0.5f;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
    }

    private void OnEnable()
    {
        moveAction.started += MoveInputListener;
        moveAction.performed += MoveInputListener;
        moveAction.canceled += MoveInputListener;

        jumpAction.performed += JumpInputListener;
        jumpAction.canceled += JumpInputListener;
    }

    private void OnDisable()
    {
        moveAction.started -= MoveInputListener;
        moveAction.performed -= MoveInputListener;
        moveAction.canceled -= MoveInputListener;

        jumpAction.performed -= JumpInputListener;
        jumpAction.canceled -= JumpInputListener;
    }

    void Update()
    {
        JumpTimerCountdown();

        Move();
        Jump();
        FlipSprite();
    }

    void MoveInputListener(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        //Debug.Log("MoveListenerCalled");
    }

    void Move()
    {
        Vector2 currentVelocity = myRigidbody.velocity;
        currentVelocity += new Vector2(moveInput.x * moveSpeed, 0);

        if (Mathf.Abs(currentVelocity.x) < 0.01f)
        {
            currentVelocity.x = 0f;
        }
        else if (Mathf.Abs(moveInput.x) < 0.01f)
        {
            currentVelocity.x *= 1f - hVelocityDampingWhenStopping;
        }
        else if (Mathf.Sign(moveInput.x) != Mathf.Sign(currentVelocity.x))
        {
            currentVelocity.x *= 1f - hVelocityDampingWhenTurning;
        }

        currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
        myRigidbody.velocity = currentVelocity;
    }

    void JumpInputListener(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if(value == 1) { jumpBufferCountdown = jumpBufferDuration; }
    }

    void Jump()
    {
        if(jumpBufferCountdown > 0 && coyoteTimeCountdown > 0)
        {
            jumpBufferCountdown = 0f;
            coyoteTimeCountdown = 0f;

            Vector2 newVerticalVelocity = new Vector2(myRigidbody.velocity.x, jumpSpeed);
            newVerticalVelocity.y = Mathf.Clamp(newVerticalVelocity.y, 0f, maxVerticalSpeed);
            myRigidbody.velocity = newVerticalVelocity;
        }
    }

    void JumpTimerCountdown()
    {
        if (jumpBufferCountdown > 0) { jumpBufferCountdown -= Time.deltaTime; }

        if (CheckIfGrounded()) { coyoteTimeCountdown = coyoteTimeDuration; }
        else if (!CheckIfGrounded()) { coyoteTimeCountdown -= Time.deltaTime; }

        jumpBufferCountdown = Mathf.Clamp(jumpBufferCountdown, 0f, jumpBufferDuration);
        coyoteTimeCountdown = Mathf.Clamp(coyoteTimeCountdown, 0f, coyoteTimeDuration);
    }

    private bool CheckIfGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Platform");
        return Physics2D.BoxCast(transform.position, boxCastSize, 0f, transform.up, groundCheckDistance * Mathf.Sign(Physics2D.gravity.y), groundMask);
    }

    void FlipSprite()
    {
        if (Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), transform.localScale.y);
        }
    }

    public float GetHorizontalVelocity()
    {
        return myRigidbody.velocity.x;
    }

    public float GetVerticalVelocity()
    {
        return myRigidbody.velocity.y;
    }

    public bool GetIsGrounded()
    {
        return CheckIfGrounded();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + (transform.up * groundCheckDistance * Mathf.Sign(Physics2D.gravity.y)), boxCastSize);
    }
}
