using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class  enemies_move : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    // Use this for initialization
    void Start()
    {  
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isRunning", true);
    }


    // Update is called once per frame
    public void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if(currentPoint == pointB.transform) 
        {
            rb.velocity = new Vector2(speed,0);
        }
        else
        {
            rb.velocity = new Vector2(-speed,0);
        } 
        if(Vector2.Distance(transform.position, currentPoint.position) < 1.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
            FlipHealthBar();
        }
        if(Vector2.Distance(transform.position, currentPoint.position) < 1.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
            FlipHealthBar();
        }

    }
    private void flip()
    {
        
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void FlipHealthBar()
    {
        // Gọi phương thức đảo hướng của thanh máu tại đây
        HealthBarFlip healthBarFlip = GetComponentInChildren<HealthBarFlip>();
        if (healthBarFlip != null)
        {
            healthBarFlip.Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }

}
