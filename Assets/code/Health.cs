using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth;                         private bool defense = false;
    public int currentHealth;                                      public GameObject pausebutton;
    public HealthBar healthBar;                                    public TextMeshProUGUI healCountText;
    public UnityEvent onDeath;                                     public takedame huhu;
    public Animator anim;
    public GameObject gameOverCanvas;
    public float maxFallHeight = -8f;
    public int maxHealingCount;
    public int currentHealingCount = 0;
    
    //private AudioManager audioManager;
    public GameObject pauseButton;
    //private void Awake()
    //{
    //    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    //}
    private void Start()
    {       
        currentHealth = maxHealth;
        healthBar.UpdateHealth(currentHealth, maxHealth);
        UpdateHealCountText();
    }
    public void FixedUpdate()
    {
        if (transform.position.y < maxFallHeight)
        {
            currentHealth = 0;
            Die();

        }
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
    public void Update()
    {
        if(defense)
        {
            huhu.attackDamage = 0;
        }
        else
        {
            huhu.attackDamage = 20;
        }
        defence();
        Recover();
    }
    public void takeDamage(int damage)
    {   
        if(!defense)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(currentHealth, 0);  // Đảm bảo máu không giảm xuống dưới 0
            if (currentHealth < maxHealth)
            {
                anim.SetTrigger("isHurt");
            }
            if (currentHealth <= 0)
            {
                Die();
            }
        }  
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    public void HealRandom()
    {
        int minValue = 0;
        int maxValue = 20;
        // Sinh một giá trị ngẫu nhiên trong phạm vi đã cho
        int healAmount = Random.Range(minValue, maxValue);

        // Hồi máu cho đối tượng
        currentHealth += healAmount;

        // Đảm bảo không vượt quá máu tối đa
        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    //hồi máu
    public void Recover()
    {
        maxHealingCount = enemyHealth.totalDeathCount;
        UpdateHealCountText();
        if (currentHealingCount < maxHealingCount)
        {
            if (Input.GetKeyDown("c"))
            {
                if (currentHealth < maxHealth)
                {
                    HealRandom();
                    anim.SetTrigger("Recover");
                }
                currentHealingCount++;
                UpdateHealCountText();
                healthBar.UpdateHealth(currentHealth, maxHealth);                
            }
        }
    }
    private void UpdateHealCountText()
    {
        healCountText.text = (maxHealingCount - currentHealingCount).ToString();
    }

    public void Die()
    {
        currentHealth = 0; // Đảm bảo máu không giảm xuống dưới 0 khi chết
        Destroy(gameObject as GameObject);
        onDeath.Invoke();

        anim.SetTrigger("Isdead");
        if (pauseButton != null)
        {
            pauseButton.SetActive(false); // hoặc pauseButton.SetActive(false);
        }

        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian
        //audioManager.musicAudioSource.Stop();
        //audioManager.PlaySFX(audioManager.musicDie);   
    }
    public void FullHeal()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    public void defence()
    {
        if (Input.GetKey("q"))
        {
            defense = true;
            anim.SetBool("defense", defense);
        }
        else
        {
            defense = false;
            anim.SetBool("defense", defense);
        }
    }
}