using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("������ ����")]
    public EnemyDataSO enemyData;

    [Header("���� ����")]
    [Tooltip("�÷��̾�� ���ظ� �� ����(��)")]
    [SerializeField] private float attackInterval = 1.0f; // [����] �� ���� ��ٿ�(���ϸ� SO�� �°� ����)
    private float nextAttackTime = 0f;

    private float currentHP;
    private Rigidbody2D rb;
    private Transform player;
    private PlayerBase playerBase;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError($"{name}: EnemyDataSO�� ������� �ʾҽ��ϴ�!");
            return;
        }

        currentHP = enemyData.maxHP;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerBase = p.GetComponent<PlayerBase>(); // [����] PlayerBase ����, TakeDamage(float) ����
        }
    }

    private void FixedUpdate()
    {
        if (player == null || enemyData == null) return;

        // �÷��̾� ���� (�����Ÿ� ���� ��� ����)
        Vector2 dir = (player.position - transform.position);
        rb.velocity = dir.normalized * enemyData.moveSpeed;
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0f) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    // ���� �浹 ���¿��� �ֱ������� �÷��̾ ���� ���� ����
    private void TryAttackPlayer()
    {
        if (playerBase == null) return;
        if (Time.time < nextAttackTime) return;

        playerBase.TakeDamage(enemyData.damage); // [����] �ñ״�ó float
        nextAttackTime = Time.time + attackInterval;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
            TryAttackPlayer();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            TryAttackPlayer();
    }
}
