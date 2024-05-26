using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public Health playerHealth;
    public int damage = 30;
    public Animator anim;
    public enemies_move speed;
    public Transform target;
    public float chaseRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 2f;
    private bool isDead = false;

    void Start()
    {
        // Initialize any required components or variables
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
                StopChase();
            }
        }
    }
    void Chase()
    {
        anim.SetTrigger("Walking");
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed.speed * Time.deltaTime);
    }

    void StopChase()
    {
        anim.ResetTrigger("Walking");
    }

    public void Attack()
    {

        if (isDead) return;

        // Randomly select between attack1 and attack2
        int attackType = Random.Range(1, 3); // Returns either 1 or 2
        if (attackType == 1)
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            anim.SetTrigger("Attack2");
        }

        float attackDelay = 0.25f; // Adjust delay as needed
        Invoke("DealDamage", attackDelay);
    }

    public void DealDamage()
    {
        if (isDead) return;

        // Check if there's a target within attack range
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
        // Disable further actions
        speed.enabled = false;
        this.enabled = false;
        // Heal the player to full health

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
