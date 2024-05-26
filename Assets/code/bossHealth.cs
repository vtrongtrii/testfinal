using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class bossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider BossHealth;
    public Health playerHealth;
    bool isAlive = true;

    public Animator anim;
    private BossDamage enemyAttack;
    private enemies_move enemyMovement;
    // Start is called before the first frame update
    void Start()
    {

        currentHealth = maxHealth;
        BossHealth.maxValue = maxHealth;
        BossHealth.value = maxHealth;
        BossHealth.gameObject.SetActive(false);

        // Lấy các thành phần liên quan đến hành vi của quái
        enemyAttack = GetComponent<BossDamage>();
        enemyMovement = GetComponent<enemies_move>();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (isAlive)
        {
            // Kích hoạt lại Slider khi quái nhận sát thương
            BossHealth.gameObject.SetActive(true);
            BossHealth.value = currentHealth; // Chỉ cập nhật giá trị fill của Slider khi quái còn sống
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
        if (!isAlive)
            return;

        isAlive = false;
        anim.SetTrigger("Isdead");
        BossHealth.gameObject.SetActive(false);
        playerHealth.currentHealth = playerHealth.maxHealth;
        Debug.Log("Boss đã chết");

        // Vô hiệu hóa các thành phần điều khiển hành vi của quái
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
        // Đợi cho đến khi animation chết hoàn tất
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

        // Hủy đối tượng sau khi animation hoàn tất
        Destroy(gameObject);
    }



    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
            return;
    }
}
