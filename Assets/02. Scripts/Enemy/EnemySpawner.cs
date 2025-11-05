using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject enemyPrefab;
    public Vector2 areaMin;  //스폰 영역 최소 좌표
    public Vector2 areaMax;  //스폰 영역 최대 좌표

    [Header("타이밍 설정")]
    public float startDelay = 3f;    //초기 스폰 간격
    public float minDelay = 0.5f;    //최소 간격 (도달 후 고정)
    public float decreaseAmount = 0.05f; //스폰 후 간격 감소량

    private float currentDelay;
    private float timer;

    [Header("충돌 검사")]
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
        for (int i = 0; i < 10; i++) // 최대 10번 위치 재시도
        {
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(
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
