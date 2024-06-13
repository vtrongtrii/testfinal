using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class enemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider EnemyHealth;
    public int healAmount;
    bool isAlive = true;
    public int DamageIncreaseAmount;
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
        if (!isAlive) return; // Nếu enemy đã chết, không thực hiện hành động nào nữa
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
        StartCoroutine(DieCoroutine());
        // Vô hiệu hóa tất cả các collider của enemy
        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            collider.enabled = false;
        }
        // Ẩn thanh máu
        EnemyHealth.gameObject.SetActive(false);
        StartCoroutine(DieCoroutine());
    }
    IEnumerator DieCoroutine()
    {
        // Đợi cho đến khi animation chết hoàn tất
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Hủy đối tượng sau khi animation hoàn tất
        Destroy(gameObject);
    }
    public static int totalDeathCount = 0;

    private void OnDestroy()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Health playerHealth = player.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }
            takedame takedame = player.GetComponent<takedame>();
            if (takedame != null)
            {
                takedame.DamageIncreae(DamageIncreaseAmount);
            }
        }
        totalDeathCount++;
    }
    // Update is called once per frame
    void Update()
    {

    }
}