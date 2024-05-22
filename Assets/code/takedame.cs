using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class takedame : MonoBehaviour
{
    public Health playerHealth;
    public int damage = 20;
    public Animator animamtor;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float attackRate = 2f;
    public float NextAttacktime;
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= NextAttacktime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                NextAttacktime = Time.time + 1f / attackRate;
            }
            if (Input.GetMouseButtonDown(1))
            {
                CritAttak();
                NextAttacktime = Time.time + 4f / attackRate;
            }
        }
    }
    void Attack()
    {
        animamtor.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToEnemy, enemy.transform.up);
            enemy.GetComponent<enemyHealth>().TakeDamage(attackDamage);
            Debug.Log("Đánh trúng rồi!");
        }
    }
    public void CritAttak()
    {
        animamtor.SetTrigger("CritAttack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(directionToEnemy, enemy.transform.up);
            enemy.GetComponent<enemyHealth>().TakeDamage(attackDamage * 2);
            Debug.Log("Đánh trúng rồi!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


}
