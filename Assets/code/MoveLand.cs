using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLand : MonoBehaviour
{
    public Transform platform;
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 1.5f;

    private Transform currentTarget;
    public bool isBossDead = false;

    void Start()
    {
        // Đăng ký sự kiện BossDied
        bossHealth.BossDied += StartMoving;
        currentTarget = startPoint;
    }

    void OnDestroy()
    {
        // Hủy đăng ký sự kiện BossDied để tránh rò rỉ bộ nhớ
        bossHealth.BossDied -= StartMoving;
    }

    void StartMoving()
    {
        isBossDead = true;
    }

    private void FixedUpdate()
    {
        // Di chuyển nền chỉ khi boss đã chết
        if (isBossDead)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        platform.position = Vector2.MoveTowards(platform.position, currentTarget.position, speed * Time.deltaTime);

        if (Vector2.Distance(platform.position, currentTarget.position) < 0.1f)
        {
            ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        if (currentTarget == startPoint)
        {
            currentTarget = endPoint;
        }
        else
        {
            currentTarget = startPoint;
        }
    }

    private void OnDrawGizmos()
    {
        if (platform != null && startPoint != null && endPoint != null)
        {
            Gizmos.DrawLine(platform.transform.position, startPoint.position);
            Gizmos.DrawLine(platform.transform.position, endPoint.position);
        }
    }

    // Xử lý va chạm để gắn nhân vật vào nền di chuyển
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(platform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
