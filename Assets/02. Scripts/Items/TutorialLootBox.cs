using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLootBox : MonoBehaviour
{
    [Header("상호작용")]
    public float interactRange = 1.2f;
    public KeyCode key = KeyCode.E;

    [Header("드랍 설정 (null 없음)")]
    public GameObject coinPickupPrefab;   // Pickup_Coin

    [Tooltip("코인 드랍 확률")]
    [Range(0f, 1f)] public float coinProbability = 1.0f;

    [Header("코인 수량 범위")]
    public Vector2Int coinAmountRange = new Vector2Int(50,50);

    private bool opened = false;
    private Transform player;

    private const string boxKey = "TutorialLootBox_Opened";

    private void Start()
    {
        //한번 열었으면 앞으로 못 열게 하기
        if (PlayerPrefs.GetInt(boxKey, 0) == 1)
        {
            Destroy(gameObject);
            return;
        }
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    private void Update()
    {
        if (opened || player == null) return;
        float dist = Vector2.Distance(player.position, transform.position);
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

        if (coinPickupPrefab != null)
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

        //1번 열었으면 다음 입장시엔 무조건 Destroy
        PlayerPrefs.SetInt(boxKey, 1);
        PlayerPrefs.Save();

        //열림 후 상자 비활성/파괴
        Destroy(gameObject);
    }
}