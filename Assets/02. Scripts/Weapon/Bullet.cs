using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("źȯ ����")]
    [Tooltip("źȯ �ӵ�")]
    public float speed = 12.0f;
    [Tooltip("���� �ð�(��)")]
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
