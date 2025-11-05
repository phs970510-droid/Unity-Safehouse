using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBench : MonoBehaviour
{
    [Header("무기 데이터")]
    public WeaponDataSO pistolData;
    public WeaponDataSO arData;
    public WeaponDataSO sgData;

    [Header("비용 설정")]
    public int pistolUpgradeCost = 30;
    public int arUpgradeCost = 50;
    public int sgUpgradeCost = 70;

    [Header("성능 증가량")]
    public float pistolDamageUp = 2f;
    public float arDamageUp = 3f;
    public float sgDamageUp = 5f;

    //Pistol 강화
    public void UpgradeWeapon_Pistol()
    {
        if (DataManager.Instance.TrySpendScrap(pistolUpgradeCost))
        {
            pistolData.damage += pistolDamageUp;
            Debug.Log("권총 강화 완료!");
        }
        else Debug.Log("스크랩이 부족합니다!");
    }

    //AR 강화
    public void UpgradeWeapon_AR()
    {
        if (!arData.isUnlocked)
        {
            Debug.Log("AR이 아직 해금되지 않았습니다!");
            return;
        }

        if (DataManager.Instance.TrySpendScrap(arUpgradeCost))
        {
            arData.damage += arDamageUp;
            Debug.Log("AR 강화 완료!");
        }
        else Debug.Log("스크랩이 부족합니다!");
    }
    //SG 강화
    public void UpgradeWeapon_SG()
    {
        if (DataManager.Instance.TrySpendScrap(sgUpgradeCost))
        {
            pistolData.damage += pistolDamageUp;
            Debug.Log("샷건 강화 완료!");
        }
        else Debug.Log("스크랩이 부족합니다!");
    }
}
