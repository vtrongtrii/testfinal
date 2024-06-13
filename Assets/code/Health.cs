using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
public class Health : MonoBehaviour
{
    [SerializeField] public int maxHealth;                                                                  public bool defense = false;
    public int currentHealth;                                                                               public GameObject pausebutton;
    public HealthBar healthBar;                                                                             public takedame huhu;
    public UnityEvent onDeath;                                                                              public GameObject healTextPrefab; 
    public Animator anim;                                                                                   
    public GameObject gameOverCanvas;                                                                        
    public float maxFallHeight = -8f;                                                                                                                                      
    public int maxHealingCount;                                                                             
    public int currentHealingCount = 0;                                                                      
    private AudioManager audioManager;                                                                       
    public GameObject pauseButton;
    public Vector3 healTextOffset = new Vector3(0, 2, 0);  
    private List<GameObject> healTextInstances = new List<GameObject>(); // Danh sách để quản lý các HealText instances
   
   

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
    private float defenceTime = 0f; // Thời điểm kích hoạt phòng thủ
    private float defenceDuration = 5f; // Thời lượng phòng thủ (5 giây)
    private float lastDefenceTime = -Mathf.Infinity; // Thời điểm cuối cùng khi phòng thủ được kích hoạt

    void Update()
    {
        defence();
        if (defense && Time.time >= defenceTime + defenceDuration)
        {
            defense = false; // Vô hiệu hóa phòng thủ
            anim.SetBool("defense", defense); // Cập nhật trạng thái animation phòng thủ
        }
    }

    public void takeDamage(int damage)
    {
        if (defense)
        {
            return;
        }

        currentHealth -= damage;
        anim.SetTrigger("isHurt");

        if (currentHealth <= 0)
        {
            currentHealth = 0;  // Ensure health doesn't drop below 0
            Die();
        }

        healthBar.UpdateHealth(currentHealth, maxHealth);
    }

   

    public  void defence()
    {
        if (Input.GetKey("q"))
        {
            defense = true; // Bật phòng thủ
            anim.SetBool("defense", defense); // Cập nhật trạng thái animation phòng thủ
        }
        else
        {
            defense = false; // Tắt phòng thủ
            anim.SetBool("defense", defense); // Cập nhật trạng thái animation phòng thủ
        }
         
    }

    public bool IsDefending()
    {
        return defense;
    }

    
    private void ShowHealText(string text, Color color)
    {
        if (healTextPrefab != null)
        {
            Vector3 healTextPosition = transform.position + healTextOffset; // Vị trí xuất hiện của HealText
            GameObject healTextInstance = Instantiate(healTextPrefab, healTextPosition, Quaternion.identity);
            healTextInstances.Add(healTextInstance); // Thêm instance vào danh sách
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

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Đặt lại máu hiện tại bằng máu tối đa mới
    }
    public void Die()
    {
        CleanUpHealTexts(); // Dọn dẹp các HealText trước khi phá hủy GameObject
         
        onDeath.Invoke(); 
       
        if (pauseButton != null)
        {
            pauseButton.SetActive(false); // hoặc pauseButton.SetActive(false);
        }

        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f; // Dừng thời gian
        audioManager.musicAudioSource.Stop();
        audioManager.PlaySFX(audioManager.musicDie);
    }

     
    private void OnApplicationQuit()
    {
        // Phá hủy tất cả các HealText instances khi ứng dụng kết thúc
        CleanUpHealTexts();
    }

    private void OnDestroy()
    {
        // Phá hủy tất cả các HealText instances khi GameObject bị phá hủy
        CleanUpHealTexts();
    }

    public void CleanUpHealTexts()
    {
        foreach (GameObject healText in healTextInstances)
        {
            if (healText != null)
            {
                Destroy(healText);
            }
        }
        healTextInstances.Clear();
    }
     
}
 