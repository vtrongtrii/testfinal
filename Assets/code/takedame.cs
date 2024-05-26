using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class takedame : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float attackRate = 1f;
    public float nextAttackTime = 0f;
    private bool isAttacking = false;
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time + 1f / attackRate;
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(CritAttack());
                nextAttackTime = Time.time + 5f / attackRate;
            }
        }

    }
    //void Attack()
    //{
    //    animamtor.SetTrigger("Attack");
    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        enemyHealth enemyComponent = enemy.GetComponent<enemyHealth>();
    //        if (enemyComponent != null)
    //        {
    //            enemyComponent.TakeDamage(attackDamage);
    //        }
    //        else
    //        {
    //            // Kiểm tra xem đối tượng có thành phần bossHealth không
    //            bossHealth bossComponent = enemy.GetComponent<bossHealth>();
    //            if (bossComponent != null)
    //            {
    //                bossComponent.TakeDamage(attackDamage);
    //            }
    //        }

    //    }


    //    }
    //void critAttack()
    //{
    //    animamtor.SetTrigger("CritAttack");
    //    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    //    foreach (Collider2D enemy in hitEnemies)
    //    {
    //        enemyHealth enemyComponent = enemy.GetComponent<enemyHealth>();
    //        if (enemyComponent != null)
    //        {
    //            enemyComponent.TakeDamage(attackDamage*2);
    //        }
    //        else
    //        {
    //            // Kiểm tra xem đối tượng có thành phần bossHealth không
    //            bossHealth bossComponent = enemy.GetComponent<bossHealth>();
    //            if (bossComponent != null)
    //            {
    //                bossComponent.TakeDamage(attackDamage*2);
    //            }
    //        }

    //    }
    //}
    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PerformAttack(attackDamage);
        isAttacking = false;
    }

    IEnumerator CritAttack()
    {
        isAttacking = true;
        animator.SetTrigger("CritAttack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PerformAttack(attackDamage * 2);
        isAttacking = false;
    }

    void PerformAttack(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemyHealth enemyComponent = enemy.GetComponent<enemyHealth>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(damage);
            }
            else
            {
                bossHealth bossComponent = enemy.GetComponent<bossHealth>();
                if (bossComponent != null)
                {
                    bossComponent.TakeDamage(damage);
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
