using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float runSpeed = 20f;
    [SerializeField] private float movementSmoothing = .1f;
    private float horizontalMove;
    private Rigidbody2D rb;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Grounded")]
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    private float groundedRadius = .2f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 300;
    public float fallJumpMultiplier = 5f;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;
        Debug.Log(isGrounded);

        bool jump = false;
        if (Input.GetKeyDown(KeyCode.Space))
                jump = true;

        Move(jump);
        jump = false;
    }

    private void FixedUpdate() {
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
    }

    public void Move(bool jump)
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * Time.fixedDeltaTime * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity,movementSmoothing);

        HandleJump(jump);
        HandleFallJumpMultiplier(jump);
    }

    private void HandleJump(bool jump)
    {
        if(isGrounded && jump) {
            isGrounded = false;
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void HandleFallJumpMultiplier(bool jump) 
    {
        if (rb.velocity.y < 0)
            rb.velocity += Time.deltaTime * Physics2D.gravity.y * (fallJumpMultiplier - 1) * Vector2.up;
        else if(rb.velocity.y > 0 && !jump) 
            rb.velocity += Time.deltaTime * Physics2D.gravity.y * (fallJumpMultiplier - 1) * Vector2.up;
    }

}
