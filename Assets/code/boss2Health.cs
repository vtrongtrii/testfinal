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

        // L?y c�c th�nh ph?n li�n quan ??n h�nh vi c?a qu�i
        enemyAttack = GetComponent<BossDamage>();
        enemyMovement = GetComponent<enemies_move>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isAlive)
        {
            // K�ch ho?t l?i Slider khi qu�i nh?n s�t th??ng
            BossHealth.gameObject.SetActive(true);
            BossHealth.value = currentHealth; // Ch? c?p nh?t gi� tr? fill c?a Slider khi qu�i c�n s?ng
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
        Debug.Log("Boss ?� ch?t");

        // V� hi?u h�a c�c th�nh ph?n ?i?u khi?n h�nh vi c?a qu�i
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
        // ??i cho ??n khi animation ch?t ho�n t?t
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // H?y ??i t??ng sau khi animation ho�n t?t
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;
    }
}
