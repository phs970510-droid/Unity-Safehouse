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
    public int unlockARCost = 50;

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

    //무기 해금
    public void UnlockWeapon(string weaponName)
    {
        WeaponDataSO target = null;

        //무기 이름으로 대상 찾기
        foreach (var data in DataManager.Instance.allWeaponData)
        {
            if (data.weaponName == weaponName)
            {
                target = data;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError($"[ShopSystem] '{weaponName}' 무기를 찾을 수 없습니다!");
            return;
        }

        if (target.isUnlocked)
        {
            Debug.Log($"[ShopSystem] {weaponName}은(는) 이미 해금되어 있습니다.");
            return;
        }
        int cost = 0;
        switch (weaponName)
        {
            case "AR": cost = unlockARCost; break;
            //case "Shotgun": cost = unlockSGCost; break;
            //이후 추가 무기도 여기서 비용만 등록
            default:
                Debug.LogWarning($"[ShopSystem] {weaponName}의 해금 비용이 지정되지 않았습니다.");
                return;
        }

        //구매 처리
        if (DataManager.Instance.TrySpendMoney(cost))
        {
            target.isUnlocked = true;
            DataManager.Instance.Save();
            Debug.Log($"[ShopSystem] {weaponName} 해금 완료! (비용 {cost})");
        }
        else
        {
            Debug.Log($"[ShopSystem] {weaponName} 해금 실패 - 돈이 부족합니다.");
        }
    }
}
