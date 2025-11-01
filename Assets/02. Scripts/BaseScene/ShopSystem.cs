using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    [Header("플레이어 데이터")]
    public PlayerDataSO playerData;

    [Header("무기 데이터")]
    public WeaponDataSO pistolData;
    public WeaponDataSO arData;

    [Header("가격 설정")]
    public int hpUpgradeCost = 200;
    public int speedUpgradeCost = 150;
    public int unlockARCost = 500;

    // 스탯 강화
    public void BuyStat_HP()
    {
        if (DataManager.Instance.TrySpendMoney(hpUpgradeCost))
        {
            playerData.UpgradeHP(20f);
            Debug.Log("HP 업그레이드 완료!");
        }
        else Debug.Log("돈이 부족합니다!");
    }

    public void BuyStat_Speed()
    {
        if (DataManager.Instance.TrySpendMoney(speedUpgradeCost))
        {
            playerData.UpgradeMoveSpeed(0.5f);
            Debug.Log("이동 속도 업그레이드 완료!");
        }
        else Debug.Log("돈이 부족합니다!");
    }

    // 무기 해금
    public void UnlockWeapon_AR()
    {
        if (arData.isUnlocked)
        {
            Debug.Log("이미 해금된 무기입니다!");
            return;
        }

        if (DataManager.Instance.TrySpendMoney(unlockARCost))
        {
            arData.isUnlocked = true;
            Debug.Log("AR 무기 해금 완료!");
        }
        else Debug.Log("돈이 부족합니다!");
    }
}
