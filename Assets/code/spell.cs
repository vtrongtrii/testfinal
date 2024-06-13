using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spell : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float lifetime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hit)
        {
            Damage();
        }

        // Di chuyển theo đường thẳng
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed * direction, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    private void Damage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag("enemies"))
            {
                anim.SetTrigger("explode");
                var enemyHealth = col.GetComponent<enemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(30);
                }

                // Kiểm tra và gây sát thương nếu có thành phần bossHealth
                var bossHealth = col.GetComponent<bossHealth>();
                if (bossHealth != null)
                {
                    bossHealth.TakeDamage(30);
                }
                hit = true;
                StartCoroutine(DeactivateWithDelay(0.5f));
                break;
            }
        }
    }

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;    
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Grond"))
        {
            anim.SetTrigger("explode");
            StartCoroutine(DeactivateWithDelay(0.5f));
        }
    }

    IEnumerator DeactivateWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}
