using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider EnemyHealth;

    bool isAlive = true;


    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        totalDeathCount = 0;
        currentHealth = maxHealth;
        EnemyHealth.maxValue = maxHealth;
        EnemyHealth.value = maxHealth;
        EnemyHealth.gameObject.SetActive(false);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isAlive)
        {
            // Kích hoạt lại Slider khi quái nhận sát thương
            EnemyHealth.gameObject.SetActive(true);
            EnemyHealth.value = currentHealth; // Chỉ cập nhật giá trị fill của Slider khi quái còn sống
        }
        if (currentHealth <= 0)

            EnemyHealth.value = currentHealth;
        if (currentHealth < maxHealth)
        {
            anim.SetTrigger("isHurt");
        }
        if (currentHealth <= 0)

        {
            Die();
        }
    }
    void Die()
    {
        anim.SetTrigger("Isdead");
        isAlive = false;
        Debug.Log("chet roi");
        StartCoroutine(RemoveAfterDelay(0.75f));
    }


    private IEnumerator RemoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
    public static int totalDeathCount = 0;

    private void OnDestroy()
    {

        totalDeathCount++;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
