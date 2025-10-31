using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBoxSpawner : MonoBehaviour
{
    public GameObject lootBoxPrefab;
    public Transform[] spawnPoints;

    [Tooltip("각 스폰 포인트별 생성 확률(0~1)")]
    [Range(0f, 1f)] public float spawnProbability = 0.5f;

    private bool spawned = false;

    private void Start()
    {
        if (spawned) return;
        spawned = true; // "게임 시작 직후 1회만"

        if (lootBoxPrefab == null || spawnPoints == null) return;
        foreach (var t in spawnPoints)
        {
            if (t == null) continue;
            if (Random.value <= spawnProbability)
            {
                Instantiate(lootBoxPrefab, t.position, Quaternion.identity);
            }
        }
    }
}
