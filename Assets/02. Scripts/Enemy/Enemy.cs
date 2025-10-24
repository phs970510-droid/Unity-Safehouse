using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("������ ����")]
    public EnemyDataSO enemyData;

    private float currentHP;
    private Rigidbody2D rb;
    private Transform player;

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
            player = p.transform;
    }

    private void FixedUpdate()
    {
        if (player == null || enemyData == null)
            return;

        Vector2 dir = (player.position - transform.position);
        float dist = dir.magnitude;

        if (dist > enemyData.stopDistance)
        {
            rb.velocity = dir.normalized * enemyData.moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
            // TODO: PlayerBase ���� �� �÷��̾�� ������ ���� �߰�
        }
    }

    public void TakeDamage(float amount)
    {
        currentHP -= amount;
        if (currentHP <= 0f)
            Die();
    }

    private void Die()
    {
        // TODO: DropManager ���� �� ��� ���� �߰� ����
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, enemyData.stopDistance);
        }
    }
}
