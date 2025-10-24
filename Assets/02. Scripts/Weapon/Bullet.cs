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
    private Rigidbody2D rb;

    public void Initialize(float dmg)
    { 
        damage = dmg;
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
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
