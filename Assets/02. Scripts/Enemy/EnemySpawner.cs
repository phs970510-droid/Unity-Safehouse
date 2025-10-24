using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject enemyPrefab;
    public Vector2 areaMin;  // ���� ���� �ּ� ��ǥ
    public Vector2 areaMax;  // ���� ���� �ִ� ��ǥ

    [Header("Ÿ�̹� ����")]
    public float startDelay = 3f;    // �ʱ� ���� ����
    public float minDelay = 0.5f;    // �ּ� ���� (���� �� ����)
    public float decreaseAmount = 0.05f; // ���� �� ���� ���ҷ�

    private float currentDelay;
    private float timer;

    [Header("�浹 �˻�")]
    public float checkRadius = 0.6f;
    public LayerMask enemyLayer;

    private void Start()
    {
        currentDelay = startDelay;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentDelay)
        {
            TrySpawn();
            timer = 0f;
            currentDelay = Mathf.Max(minDelay, currentDelay - decreaseAmount);
        }
    }

    private void TrySpawn()
    {
        for (int i = 0; i < 10; i++) // �ִ� 10�� ��ġ ��õ�
        {
            Vector2 spawnPos = new Vector2(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y)
            );

            Collider2D hit = Physics2D.OverlapCircle(spawnPos, checkRadius, enemyLayer);
            if (hit == null)
            {
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = new Vector3((areaMin.x + areaMax.x) * 0.5f, (areaMin.y + areaMax.y) * 0.5f, 0f);
        Vector3 size = new Vector3(Mathf.Abs(areaMax.x - areaMin.x), Mathf.Abs(areaMax.y - areaMin.y), 1f);
        Gizmos.DrawWireCube(center, size);
    }
}
