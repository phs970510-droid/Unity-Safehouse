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

    [Header("드랍 설정")]
    [Range(0f, 1f)] public float coinDropChance = 0.35f;
    [Range(0f, 1f)] public float ammoDropChance = 0.20f;
    public Vector2Int coinAmountRange = new Vector2Int(3, 10);
    public Vector2Int ammoAmountRange = new Vector2Int(10, 30);

    public GameObject coinPickupPrefab;
    public GameObject ammoARPickupPrefab;

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

    private void DropOnDeath()
    {
        // 코인
        if (coinPickupPrefab != null && Random.value <= coinDropChance)
        {
            var go = Instantiate(coinPickupPrefab, transform.position, Quaternion.identity);
            var p = go.GetComponent<PickupItem>();
            if (p != null)
            {
                p.type = PickupItem.ItemType.Money;
                p.minAmount = coinAmountRange.x;
                p.maxAmount = coinAmountRange.y;
            }
        }
        // AR 탄
        if (ammoARPickupPrefab != null && Random.value <= ammoDropChance)
        {
            var go = Instantiate(ammoARPickupPrefab, transform.position, Quaternion.identity);
            var p = go.GetComponent<PickupItem>();
            if (p != null)
            {
                p.type = PickupItem.ItemType.AmmoAR;
                p.minAmount = ammoAmountRange.x;
                p.maxAmount = ammoAmountRange.y;
            }
        }
    }

    private void Die()
    {
        DropOnDeath();
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
