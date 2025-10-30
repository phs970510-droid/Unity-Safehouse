using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("탄환 설정")]
    [Tooltip("탄환 속도")]
    public float speed = 12.0f;
    [Tooltip("생존 시간(초)")]
    public float lifeTime = 5.0f;

    private float damage;
    private int remainingPenetration = 0;
    private Rigidbody2D rb;
    private bool hasHit = false;

    public void Initialize(float dmg, int penetrationCount)
    { 
        damage = dmg;
        remainingPenetration = penetrationCount;
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * speed;
        Destroy(gameObject,lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            if (remainingPenetration > 0)
            {
                remainingPenetration--;
                hasHit = false; // 다음 적도 맞출 수 있게 초기화
            }
            else
            {
                hasHit = true;
                Destroy(gameObject);
            }
        }
    }
}
