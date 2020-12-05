using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float runSpeed = 20f;
    private bool isFacingRight = true;
    private int facingDirection = 1;
    private float moveInput;
    private Rigidbody2D rb;
    private bool canMove = true;

    [Header("Grounded")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    private float groundedRadius = .2f;
    private bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 300;
    [SerializeField] private float fallJumpMultiplier = 5f;

    [Header("Wall Jump")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance = .9f;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallJumpForce = 5f;
    [SerializeField] private Vector2 wallJumpDirection;
    private bool isTouchingWall;
    private bool isWallSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckSurroundings();
        CheckWallSliding();

        bool jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
                jump = true;

        if(canMove) {
            Move(jump);
        }

        jump = false;
    }

    public void Move(bool jump)
    {
        moveInput = Input.GetAxis("Horizontal") * runSpeed;
        rb.velocity = new Vector2(moveInput * runSpeed, rb.velocity.y);

        if(isWallSliding) 
        {
            if(rb.velocity.y < -wallSlideSpeed) 
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }

        HandleFlip();
        HandleJump(jump);
        HandleFallJumpMultiplier(jump);
    }

    private void HandleJump(bool jump)
    {
        if(isGrounded && jump && !isWallSliding) 
        {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        } 
        else if (jump && isWallSliding)
        {
            Vector2 force = new Vector2(wallJumpForce * wallJumpDirection.x * -facingDirection, wallJumpForce * wallJumpDirection.y);
            rb.velocity = Vector2.zero;

            rb.AddForce(force, ForceMode2D.Impulse);

            // Retira Temporariamente o controle do personagem
            StartCoroutine("StopMove");
        }
    }

    IEnumerator StopMove() {
        canMove = false;
        Flip();
        yield return new WaitForSeconds(.3f);

        Flip();
        canMove = true;
    }

    private void HandleFallJumpMultiplier(bool jump) 
    {
        if (rb.velocity.y < 0)
            rb.velocity += Time.deltaTime * Physics2D.gravity.y * (fallJumpMultiplier - 1) * Vector2.up;
        else if(rb.velocity.y > 0 && !jump) 
            rb.velocity += Time.deltaTime * Physics2D.gravity.y * (fallJumpMultiplier - 1) * Vector2.up;
    }

    private void HandleFlip() 
    {
        facingDirection = isFacingRight ? 1 : -1;
        if (moveInput > 0 && !isFacingRight)
            Flip();
        if(moveInput < 0 && isFacingRight)
            Flip();
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void CheckSurroundings() {
        bool wasGrounded = isGrounded;
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                if (!wasGrounded) {
                    Debug.Log("Tocou o chão!");
                    //Criar partículas de poeira
                }
            }
        }

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
        Debug.Log("isTouchingWall: " + isTouchingWall);
    }

    private void CheckWallSliding() {
        if(isTouchingWall && !isGrounded && rb.velocity.y < 0 && moveInput != 0)
            isWallSliding = true;
        else
            isWallSliding = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        if(isFacingRight)
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
        else    
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x - wallCheckDistance, wallCheck.position.y));
    }

}
