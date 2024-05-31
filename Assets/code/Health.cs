using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth;         private bool defense = false;
    public int currentHealth;                      public GameObject pausebutton;
    public HealthBar healthBar;                    public takedame huhu;
    public UnityEvent onDeath;                     public GameObject healTextPrefab; // Prefab cho HealText
    public Animator anim;
    public GameObject gameOverCanvas;
    public float maxFallHeight = -8f;
    public int maxHealingCount;
    public int currentHealingCount = 0;
    private AudioManager audioManager;
    public GameObject pauseButton;
    public Vector3 healTextOffset = new Vector3(0, 2, 0); // Vị trí offset của HealText
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.UpdateHealth(currentHealth, maxHealth);
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
        if (defense)
        {
            huhu.attackDamage = 0;
        }
        else
        {
            huhu.attackDamage = 20;
        }
        defence();

    }
    public void takeDamage(int damage)
    {
        if (!defense)

        {
            currentHealth -= damage;
            if (currentHealth < maxHealth)
            {
                anim.SetTrigger("isHurt");
            }
        }
        if (currentHealth <= 0)
        {
            Die();
        }
        healthBar.UpdateHealth(currentHealth, maxHealth);
    }
    private void ShowHealText(string text, Color color)
    {
        if (healTextPrefab != null)
        {
            Vector3 healTextPosition = transform.position + healTextOffset; // Vị trí xuất hiện của HealText
            GameObject healTextInstance = Instantiate(healTextPrefab, healTextPosition, Quaternion.identity);
            HealText healText = healTextInstance.GetComponent<HealText>();
            if (healText != null)
            {
                healText.SetText(text);
                healText.textMesh.color = color;
            }
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        ShowHealText("+" + amount, Color.green);
    }
    public void Die()
    {
        Destroy(gameObject as GameObject);
        onDeath.Invoke();

        anim.SetTrigger("Isdead");
        if (pauseButton != null)
        {
            pauseButton.SetActive(false); // hoặc pauseButton.SetActive(false);
        }

        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian
        audioManager.musicAudioSource.Stop();
        audioManager.PlaySFX(audioManager.musicDie);
    }
    public void defence()
    {
        if (Input.GetKey("q"))
        {

            defense = true;
            anim.SetBool("defense", defense);
        }
        else

            defense = false;
        anim.SetBool("defense", defense);
    }
}