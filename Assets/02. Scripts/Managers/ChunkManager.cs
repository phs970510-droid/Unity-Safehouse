using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("기본 설정")]
    public Transform player;        //플레이어 Transform
    public int chunkSize = 20;      //각 청크의 가로/세로 크기 (월드 단위)
    public int loadRange = 1;       //3x3 로드 기준 (1이면 중심+1타일)
    public int unloadRange = 2;     //5x5 감시 기준 (2이면 중심+2타일)

    [Header("청크 프리팹 풀")]
    public GameObject[] chunkPrefabs;  // 랜덤 또는 순차 배치용 프리팹 리스트

    private Vector2Int currentCenter;   // 현재 중심 청크 좌표
    private readonly Dictionary<Vector2Int, GameObject> activeChunks = new();

    private void Start()
    {
        // 첫 초기화
        currentCenter = GetPlayerChunk();
        UpdateChunks();
    }

    private void Update()
    {
        Vector2Int newCenter = GetPlayerChunk();
        if (newCenter != currentCenter)
        {
            currentCenter = newCenter;
            UpdateChunks();
        }
    }
    //플레이어 청크 계산
    private Vector2Int GetPlayerChunk()
    {
        int x = Mathf.FloorToInt(player.position.x / chunkSize);
        int y = Mathf.FloorToInt(player.position.y / chunkSize);
        return new Vector2Int(x, y);
    }
    //청크 업데이트
    private void UpdateChunks()
    {
        HashSet<Vector2Int> needed = new();

        // 3x3 범위 내 청크만 유지
        for (int x = -loadRange; x <= loadRange; x++)
            for (int y = -loadRange; y <= loadRange; y++)
            {
                needed.Add(currentCenter + new Vector2Int(x, y));
            }

        // 5x5 범위 밖 청크는 제거
        List<Vector2Int> toRemove = new();
        foreach (var kv in activeChunks)
        {
            Vector2Int key = kv.Key;
            if (Mathf.Abs(key.x - currentCenter.x) > unloadRange ||
                Mathf.Abs(key.y - currentCenter.y) > unloadRange)
            {
                Destroy(kv.Value);
                toRemove.Add(key);
            }
        }

        foreach (var key in toRemove)
            activeChunks.Remove(key);

        // 새로 필요한 청크 로드
        foreach (var key in needed)
        {
            if (!activeChunks.ContainsKey(key))
            {
                Vector3 pos = new Vector3(key.x * chunkSize, key.y * chunkSize, 0.0f);
                GameObject prefab = GetChunkPrefab(key);
                GameObject chunk = Instantiate(prefab, pos, Quaternion.identity, transform);
                activeChunks[key] = chunk;
            }
        }
    }
    //청크 프리팹 선택
    private GameObject GetChunkPrefab(Vector2Int key)
    {
        if (chunkPrefabs == null || chunkPrefabs.Length == 0)
        {
            Debug.LogError("[ChunkManager] 청크 프리팹이 설정되지 않았습니다.");
            return null;
        }
        //간단히 랜덤 선택
        return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
    }
    //청크 간격 확인용(정식 빌드 제출 전에 코드 제거하기)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        // 실제 좌표 계산은 Vector2
        Vector2 centerPos2D = currentCenter * chunkSize;

        // Gizmos에 넘길 때만 Vector3로 변환
        Vector3 centerPos = new Vector3(centerPos2D.x, centerPos2D.y, 0f);

        Gizmos.DrawWireCube(centerPos,
            new Vector3(chunkSize * (loadRange * 2 + 1),
                        chunkSize * (loadRange * 2 + 1),
                        0f));
    }
}