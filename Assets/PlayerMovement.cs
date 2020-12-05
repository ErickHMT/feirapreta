using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float horizontalMove;
    private Rigidbody2D rb;

    [SerializeField] public float runSpeed = 5f;
    [SerializeField] private float movementSmoothing = .1f;
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Grounded")]
    private bool isGrounded;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    private float groundedRadius = .2f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 400f;
    public float fallJumpMultiplier = 2.5f;

    void Start()
    {
        isGrounded = false;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * runSpeed;
    }

    private void FixedUpdate() {
        Move();
    }

    public void Move()
    {
        Vector3 targetVelocity = new Vector2(horizontalMove * 10f, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity,movementSmoothing);
    }

}
