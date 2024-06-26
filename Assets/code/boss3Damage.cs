﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss3Damage : MonoBehaviour
{
    public Health playerHealth; // Sức khỏe của người chơi
    public int damage = 30; // Sát thương mà boss gây ra
    public Animator anim; // Bộ điều khiển hoạt hình của boss
    public float speed = 5f; // Tốc độ di chuyển của boss
    public Transform target; // Mục tiêu (người chơi)
    public float chaseRange = 10f; // Phạm vi truy đuổi của boss
    public float attackRange1 = 2f; // Phạm vi của kỹ năng tấn công 1
    public float attackRange2 = 5f; // Phạm vi của kỹ năng tấn công 2
    public float attackCooldown1 = 1f; // Thời gian hồi chiêu của kỹ năng 1
    public float attackCooldown2 = 4f; // Thời gian hồi chiêu của kỹ năng 2
 
    private float nextAttackTime1 = 0f; // Thời gian cho lần tấn công 1 tiếp theo
    private float nextAttackTime2 = 0f; // Thời gian cho lần tấn công 2 tiếp theo
    private bool IsDead = false; // Trạng thái chết của boss

    private float defaultSpeed; // Lưu trữ tốc độ mặc định
    private float increasedSpeed = 50f; // Tốc độ trong khi sử dụng kỹ năng tấn công 2
    private bool facingRight = true; // Để theo dõi hướng hiện tại của boss

    void Start()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogError("Animator chưa được gán và không tìm thấy Animator trên GameObject.");
            }
        }

        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            else
            {
                Debug.LogError("Không tìm thấy đối tượng với tag 'Player'.");
            }
        }

        defaultSpeed = speed; // Khởi tạo tốc độ mặc định
    }
    void Chase()
    {
        // Kích hoạt hoạt ảnh chạy
        anim.SetBool("isRunning", true);

        // Tính toán hướng tới mục tiêu
        Vector3 direction = (target.position - transform.position).normalized;

        // Di chuyển boss về phía mục tiêu một cách mượt mà
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Kiểm tra và lật hướng của boss nếu cần thiết
        if ((direction.x > 0 && !facingRight) || (direction.x < 0 && facingRight))
        {
            Flip();
        }
    }

    void Flip()
    {
        // Chuyển đổi hướng mà boss đang đối mặt
        facingRight = !facingRight;

        // Lật sprite/model của boss bằng cách đảo ngược tỷ lệ cục bộ
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    void Update()
    {
        // Nếu boss đã chết, không thực hiện bất kỳ hành động nào
        if (IsDead) return;

        // Kiểm tra nếu có mục tiêu
        if (target != null)
        {
            // Tính khoảng cách đến mục tiêu
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Nếu khoảng cách đến mục tiêu nằm trong tầm tấn công 1 và đã đến thời gian tấn công tiếp theo
            if (distanceToTarget <= attackRange1 && Time.time >= nextAttackTime1)
            {
                Attack1();
                nextAttackTime1 = Time.time + attackCooldown1;
            }
            // Nếu khoảng cách đến mục tiêu nằm trong tầm tấn công 2 và đã đến thời gian tấn công tiếp theo
            else if (distanceToTarget <= attackRange2 && Time.time >= nextAttackTime2)
            {
                Attack2();
                nextAttackTime2 = Time.time + attackCooldown2;
            }
            // Nếu khoảng cách đến mục tiêu nằm trong tầm đuổi theo
            else if (distanceToTarget <= chaseRange)
            {
                Chase();
            }
            // Nếu mục tiêu không nằm trong tầm tấn công hoặc đuổi theo, dừng chạy
            else
            {
                anim.SetBool("isRunning", false);
            }
        }
    }


    void Attack1()
    {
        if (IsDead) return;

        // Kích hoạt hoạt hình kỹ năng tấn công 1
        anim.SetTrigger("Attack");

        DealDamage();
    }

    void Attack2()
    {
        if (IsDead) return;

        // Kích hoạt hoạt hình kỹ năng tấn công 2
        anim.SetTrigger("cast");

        // Giả định rằng DealDamage sẽ được gọi trong quá trình hoạt hình hoặc ngay lập tức
        DealDamage();
    }

    // Hàm để tăng tốc độ (sử dụng cho sự kiện hoạt hình)
    public void IncreaseSpeed()
    {
        speed = increasedSpeed;
    }

    // Hàm để đặt lại tốc độ (sử dụng cho sự kiện hoạt hình)
    public void ResetSpeed()
    {
        speed = defaultSpeed;
    }

    void DealDamage()
    {
        if (IsDead) return;

        // Kiểm tra nếu có mục tiêu trong phạm vi tấn công
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(target.position, attackRange1);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<Health>().takeDamage(damage); // Gây sát thương gấp đôi cho kỹ năng cast
            }
        }
    }


    public void Die()
    {
        if (IsDead) return;

        IsDead = true;
        anim.SetTrigger("Die");
        this.enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange1);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}