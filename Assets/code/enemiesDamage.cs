using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemiesDamage : MonoBehaviour
{
    public Health playerHealth;
    public int damage = 20;
    public Animator anim;                                   public enemies_move speed;
    public Transform target;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    private float nextAttackTime = 2f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && Time.time >= nextAttackTime)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= attackRange)
            {
                Attack();
                nextAttackTime = Time.time + attackCooldown;  
            }
        }
    }
    public void Attack()
    {
        anim.SetTrigger("Attack");

        
        float attackDelay = 0.25f; // 
        Invoke("DealDamage", attackDelay);
    }
    public void DealDamage()
    {
        // Ki?m tra xem có d?i tu?ng m?c tiêu ? g?n không
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 5f); // Vùng c?m kích ho?t t?n công
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<Health>().takeDamage(damage);
            }
        }
    }
}
