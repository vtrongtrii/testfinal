using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class takedame : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    public Animator animator;                           public Transform attackPoint;
    public float attackRange = 1f;                      public LayerMask enemyLayers;
    public int attackDamage = 20;                       public int CurrentAttackDamage;
    public float attackRate = 1f;                       public float nextAttackTime = 0f;
    private bool isAttacking = false;                   public AudioManager audioManager; // Thêm biến tham chiếu đến AudioManager
    public bool defense;                                public GameObject damageTextPrefab; // Reference to DamageText prefab
    public Health playerHealth;                         public bool isBossDead = false;
    // Update is called once per frame
    void Start()
    {
        bossHealth.BossDied += CanSpell;
    }
    void OnDestroy()
    {
        // Hủy đăng ký sự kiện BossDied để tránh rò rỉ bộ nhớ
        bossHealth.BossDied -= CanSpell;
    }

    void CanSpell()
    {
        isBossDead = true;
    }

    private void FixedUpdate()
    {
        // Di chuyển nền chỉ khi boss đã chết
        if (isBossDead)
        {
            Spell();
        }
    }
    void Update()
    {

        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
                nextAttackTime = Time.time;
            }
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(CritAttack());
                nextAttackTime = Time.time + 4f / attackRate;
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
    private void Cast()
    {
        fireballs[0].transform.position = firePoint.position;
        fireballs[0].GetComponent<spell>().SetDirection(Mathf.Sign(transform.localScale.x));
    }



    void Spell()
    {
        if (Input.GetKey("e"))
        {
            animator.SetTrigger("spell");
            Cast();
        }
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if(!fireballs[i].gameObject.activeInHierarchy)
                return i;
        }
        return 0;
    }
    void PerformAttack(int damage)
    {
        if (playerHealth != null && playerHealth.IsDefending())
        {
            Debug.Log("Cannot attack while defending.");
            return;
        }
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
                boss2Health boss2Component = enemy.GetComponent<boss2Health>();
                if (boss2Component != null)
                {
                    boss2Component.TakeDamage(damage);
                    ShowDamageText(damage, boss2Component.transform.position); // Show damage text
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