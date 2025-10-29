using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("데이터 참조")]
    public EnemyDataSO enemyData;

    [Header("공격 설정")]
    [Tooltip("플레이어에게 피해를 줄 간격(초)")]
    [SerializeField] private float attackInterval = 1.0f; // [가정] 적 공통 쿨다운(원하면 SO로 승격 가능)
    private float nextAttackTime = 0f;

    private float currentHP;
    private Rigidbody2D rb;
    private Transform player;
    private PlayerBase playerBase;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError($"{name}: EnemyDataSO가 연결되지 않았습니다!");
            return;
        }

        currentHP = enemyData.maxHP;

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
        {
            player = p.transform;
            playerBase = p.GetComponent<PlayerBase>(); // [가정] PlayerBase 존재, TakeDamage(float) 보유
        }
    }

    private void FixedUpdate()
    {
        if (player == null || enemyData == null) return;

        // 플레이어 추적 (정지거리 없이 계속 따라감)
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

    // ── 충돌 상태에서 주기적으로 플레이어에 피해 적용 ──
    private void TryAttackPlayer()
    {
        if (playerBase == null) return;
        if (Time.time < nextAttackTime) return;

        playerBase.TakeDamage(enemyData.damage); // [가정] 시그니처 float
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
