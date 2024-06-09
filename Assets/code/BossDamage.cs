using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public Health playerHealth;
    public int damage = 30;
    public Animator anim;
    public float speed = 5;
    public Transform target;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 0f;
    private bool isDead = false;
    private bool facingRight = true; // Để theo dõi hướng hiện tại của boss

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("Animator chưa được gán và không tìm thấy Animator trên GameObject.");
            }
        }

        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Không tìm thấy đối tượng với tag 'Player'.");
            }
        }
    }

    void Update()
    {
        if (isDead) return;

        if (target != null && Time.time >= nextAttackTime)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;
            }
            else if (distanceToTarget <= chaseRange)
            {
                Chase();
            }
            else
            {
                anim.SetBool("isRunning", false);
            }
        }
    }

    void Chase()
    {
        anim.SetBool("isRunning", true);
        Vector3 direction = (target.position - transform.position).normalized;

        // Đảm bảo boss di chuyển về phía player
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Kiểm tra và lật hướng của boss nếu cần thiết
        if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void Attack()
    {
        if (isDead) return;

        int attackType = Random.Range(1, 3);
        if (attackType == 1)
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("Attack2");
        }

        float attackDelay = 0.25f;
        Invoke("DealDamage", attackDelay);
    }

    public void DealDamage()
    {
        if (isDead) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<Health>().takeDamage(damage);
            }
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        anim.SetTrigger("Die");
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
