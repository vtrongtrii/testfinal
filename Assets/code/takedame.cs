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
    private float nextCritAttackTime = 0f;              private float critAttackCooldown = 2f;
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
    private void Update()
    {
        if (Time.time >= nextAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 0.5f / attackRate;
            }
        }
        if ( Time.time >= nextCritAttackTime && !isAttacking)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(CritAttack());
                nextCritAttackTime = Time.time + critAttackCooldown;
            }  
        }
        else
        {
            animator.SetBool("crit", false);
        }
    }
    

    public void Attack()
    {
        PlayAttackSFX();
        isAttacking = true;
        animator.SetTrigger("Attack");
   
        isAttacking = false;
    }

    private IEnumerator CritAttack()
    {
        PlayAttackSFX();
        isAttacking = true;
        animator.SetBool("crit", true);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Chờ đến khi hoạt ảnh kết thúc
        PerformCriticalAttack();
        isAttacking = false;
    }
    private void PlayAttackSFX()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.strikePlayerClip);
        }
    }

    // Animation Event cho đòn tấn công thường
    public void PerformNormalAttack()
    {
        PerformAttack(CurrentAttackDamage);
    }

    // Animation Event cho đòn tấn công chí mạng
    public void PerformCriticalAttack()
    {
        PerformAttack(CurrentAttackDamage * 2);
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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
    
    public void DamageIncreae(int amount)
    {
        CurrentAttackDamage += amount;
    }
    public int GetCurrentDamage()
    {
        return CurrentAttackDamage;
    }
}