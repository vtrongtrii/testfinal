using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dichuyen : MonoBehaviour
{
    private bool m_grounded = false;
    public bool isfacingRight = true;
    public float speed;
    private float left_right;
    private Rigidbody2D rb;
    public float jump;

    public Vector2 boxSize;
    public float castDistance;
    public LayerMask groundLayer;

    private Animator animator;
    float m_jumpForce = 10f;

    private Sensor_Bandit m_groundSensor;
    // Start is called before the first frame update
    void Start()
    {
        m_grounded = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        

    



        //Check if character just started falling
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



