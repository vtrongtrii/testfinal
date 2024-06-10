using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class boss2Health : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider BossHealth;
    public Health playerHealth;
    bool isAlive = true;

    public Animator anim;
    private BossDamage enemyAttack;
    private enemies_move enemyMovement;
    public static event Action Boss2Died;
    public int increaseMaxHealthAmount=200;
    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        BossHealth.maxValue = maxHealth;
        BossHealth.value = maxHealth;
        BossHealth.gameObject.SetActive(false);

        // L?y các thành ph?n liên quan ??n hành vi c?a quái
        enemyAttack = GetComponent<BossDamage>();
        enemyMovement = GetComponent<enemies_move>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isAlive)
        {
            // Kích ho?t l?i Slider khi quái nh?n sát th??ng
            BossHealth.gameObject.SetActive(true);
            BossHealth.value = currentHealth; // Ch? c?p nh?t giá tr? fill c?a Slider khi quái còn s?ng
        }
        if (currentHealth <= 0)
            BossHealth.value = currentHealth;


        if (currentHealth <= 0)

        {
            Die();
        }
    }
    void Die()
    {
        Boss2Died?.Invoke();
        if (!isAlive)
            return;

        isAlive = false;
        anim.SetTrigger("Isdead");
        BossHealth.gameObject.SetActive(false);
        playerHealth.currentHealth = playerHealth.maxHealth;
        playerHealth.IncreaseMaxHealth(increaseMaxHealthAmount);
        Debug.Log("Boss ?ã ch?t");

        // Vô hi?u hóa các thành ph?n ?i?u khi?n hành vi c?a quái
        if (enemyAttack != null)
        {
            enemyAttack.enabled = false;
        }

        if (enemyMovement != null)
        {
            enemyMovement.enabled = false;
        }

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        // ??i cho ??n khi animation ch?t hoàn t?t
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // H?y ??i t??ng sau khi animation hoàn t?t
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;
    }
}
