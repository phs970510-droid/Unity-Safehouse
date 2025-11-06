using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBox : MonoBehaviour
{
    [Header("상호작용")]
    public float interactRange = 1.2f;
    public KeyCode key = KeyCode.E;

    [Header("드랍 설정 (null 없음)")]
    public GameObject coinPickupPrefab;   //Pickup_Coin
    public GameObject scrapPickupPrefab;  //Pickup_Scrap

    [Tooltip("코인 드랍 확률(0~1) 스크랩은 1-(코인 확률)")]
    [Range(0f, 1f)] public float coinProbability = 0.6f;

    [Header("코인/스크랩 수량 범위")]
    public Vector2Int coinAmountRange = new Vector2Int(5, 15);
    public Vector2Int scrapAmountRange = new Vector2Int(1, 5);

    private bool opened = false;
    private Transform player;

    private void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (opened || player == null) return;
        float dist = Vector2.Distance(player.position, transform.position);

        //거리 멀어지면 자동파괴
        float destroyDistance = 25f;
        if (dist > destroyDistance)
        {
            Destroy(gameObject);
            return;
        }

        //상호작용 처리
        if (dist <= interactRange && Input.GetKeyDown(key))
        {
            Open();
        }
    }

    private void Open()
    {
        if (opened) return;
        opened = true;

        bool dropCoin = Random.value <= coinProbability;
        if (dropCoin && coinPickupPrefab != null)
        {
            var go = Instantiate(coinPickupPrefab, transform.position, Quaternion.identity);
            var pick = go.GetComponent<PickupItem>();
            if (pick != null)
            {
                pick.type = PickupItem.ItemType.Money;
                pick.minAmount = coinAmountRange.x;
                pick.maxAmount = coinAmountRange.y;
            }
        }
        else if (scrapPickupPrefab != null)
        {
            var go = Instantiate(scrapPickupPrefab, transform.position, Quaternion.identity);
            var pick = go.GetComponent<PickupItem>();
            if (pick != null)
            {
                pick.type = PickupItem.ItemType.Scrap;
                pick.minAmount = scrapAmountRange.x;
                pick.maxAmount = scrapAmountRange.y;
            }
        }

        //열림 후 상자 비활성/파괴
        Destroy(gameObject);
    }
}
