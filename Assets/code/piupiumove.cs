using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class piupiumove : MonoBehaviour
{
    public GameObject pointA1;
    public GameObject pointB1;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform currentPoint;
    public float speed;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB1.transform;
        anim.SetBool("isRunning", true);
    }


    // Update is called once per frame
    public void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB1.transform)
        {
            rb.velocity = new Vector2(speed, 0);
        }
        else
        {
            rb.velocity = new Vector2(-speed, 0);
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB1.transform)
        {
            flip();
            currentPoint = pointA1.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA1.transform)
        {
            flip();
            currentPoint = pointB1.transform;
        }

    }
    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA1.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB1.transform.position, 0.5f);
        Gizmos.DrawLine(pointA1.transform.position, pointB1.transform.position);
    }

}

 