using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class takedame : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public int CurrentAttackDamage;
    public float attackRate = 1f;
    public float nextAttackTime = 0f;
    private bool isAttacking = false;
    public AudioManager audioManager; // Thêm biến tham chiếu đến AudioManager
    public GameObject damageTextPrefab; // Reference to DamageText prefab
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
    IEnumerator Attack()
    {
        // Phát âm thanh sfx khi đánh
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.strikePlayerClip);
        }
        isAttacking = true;
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PerformAttack(CurrentAttackDamage);
        isAttacking = false;
    }

    IEnumerator CritAttack()
    {
        // Phát âm thanh sfx khi đánh
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.strikePlayerClip);
        }
        isAttacking = true;
        animator.SetTrigger("CritAttack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        PerformAttack(CurrentAttackDamage * 2);
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
                ShowDamageText(damage, enemy.transform.position); // Show damage text
            }
            else
            {
                bossHealth bossComponent = enemy.GetComponent<bossHealth>();
                if (bossComponent != null)
                {
                    bossComponent.TakeDamage(damage);
                    ShowDamageText(damage, bossComponent.transform.position); // Show damage text
                }
            }
        }
    }

    void ShowDamageText(int damage, Vector3 position)
    {
        if (damageTextPrefab != null)
        {
            GameObject damageTextInstance = Instantiate(damageTextPrefab, position, Quaternion.identity);
            DamageText damageText = damageTextInstance.GetComponent<DamageText>();
            if (damageText != null)
            {
                damageText.SetText(damage.ToString());
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    public void DamageIncreae(int amount)
    {
        CurrentAttackDamage += amount;
    }
    public int GetCurrentDamage()
    {
        return CurrentAttackDamage;
    }
}