using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dichuyen : MonoBehaviour
{
    private bool m_grounded = false;                             public bool isfacingRight = true;                            
    public float speed;                                          private float left_right;                                    
    private Rigidbody2D rb;                                      public float jump;
    public Vector2 boxSize;                                      public float castDistance;
    public LayerMask groundLayer;                                private Animator animator;
    float m_jumpForce = 10f; public float dashSpeed = 10f;      // Dash speed
    public float dashDuration = 0.5f;  // Duration of the dash
    public float dashCooldown = 2f;    // Cooldown duration for the dash
    public float normalSpeed = 6f;
    private float dashTime;            // Time when the dash started
    private bool isDashing = false;    // Whether the player is currently dashing
    private float lastDashTime;
    private Sensor_Bandit m_groundSensor;                        
    public bool isBoss2Dead = false;
    // Start is called before the first frame update
    void Start()
    {
        speed = normalSpeed;
        m_grounded = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boss2Health.Boss2Died += StartDashing;
    }
    void OnDestroy()
    {
        // Hủy đăng ký sự kiện BossDied để tránh rò rỉ bộ nhớ
        boss2Health.Boss2Died -= StartDashing;
    }

    void StartDashing()
    {
        isBoss2Dead = true;
    }

    private void FixedUpdate()
    {
        // Di chuyển nền chỉ khi boss đã chết
        if (isBoss2Dead)
        {
            Dash();
        }
    }

    // Update is called once per frame
    void Update()
    {
        left_right = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(left_right * speed, rb.velocity.y);
   
        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            rb.AddForce(new Vector2(rb.velocity.x, jump * 10));

        }
        animator.SetBool("IsJumping", !isGrounded());

        flip();

        animator.SetFloat("Run", Mathf.Abs(left_right));

        
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("CritAttack");
        }
        float longitude = transform.position.y;
         
    }
    public void Dash()
    {
        // Check if the player can dash (not already dashing and cooldown has elapsed)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && (Time.time - lastDashTime > dashCooldown))
        {
            animator.SetTrigger("dash");
            speed = dashSpeed;
            isDashing = true;
            dashTime = Time.time; // Record the time when the dash started
            lastDashTime = Time.time; // Record the time when the dash occurred
        }
        if (isDashing)
        {
            // Kiểm tra nếu thời gian kéo dài của cú lao nhanh đã hết
            if (Time.time - dashTime > dashDuration)
            {
                isDashing = false;
                speed = normalSpeed;
            }
        }
    }

    public bool isGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else

        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    void flip()
    {
        if (isfacingRight && left_right < 0 || !isfacingRight && left_right > 0)
        {
            isfacingRight = !isfacingRight;
            Vector3 size = transform.localScale;
            size.x = size.x * -1;
            transform.localScale = size;
        }
    }
}



