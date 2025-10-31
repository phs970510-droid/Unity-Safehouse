using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    [Header("드랍 상자 프리팹")]
    public GameObject lootBoxPrefab;

    [Header("랜덤 스폰 설정")]
    public Vector2 areaMin = new Vector2(-10f, -10f);
    public Vector2 areaMax = new Vector2(10f, 10f);
    [Range(1, 50)] public int spawnCount = 5;
    [Tooltip("상자 간 최소 거리 (겹침 방지)")]
    public float minDistance = 1.5f;
    public LayerMask lootBoxLayer;

    private bool spawned = false;

    private void Start()
    {
        if (spawned || lootBoxPrefab == null) return;
        spawned = true;

        int placed = 0;
        int safety = 500; // 무한 루프 방지용

        while (placed < spawnCount && safety > 0)
        {
            safety--;

            Vector2 pos = new Vector2(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y)
            );

            // 주변에 LootBox가 있으면 재시도
            if (Physics2D.OverlapCircle(pos, minDistance, lootBoxLayer))
                continue;

            Instantiate(lootBoxPrefab, pos, Quaternion.identity);
            placed++;
        }

        if (placed < spawnCount)
            Debug.LogWarning($"[LootBoxSpawner] {spawnCount}개 중 {placed}개만 배치됨 (공간 부족)");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3(
            (areaMin.x + areaMax.x) * 0.5f,
            (areaMin.y + areaMax.y) * 0.5f,
            0f
        );
        Vector3 size = new Vector3(
            Mathf.Abs(areaMax.x - areaMin.x),
            Mathf.Abs(areaMax.y - areaMin.y),
            1f
        );
        Gizmos.DrawWireCube(center, size);
    }
}
