using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("경제/자원")]
    public int Money;   //코인
    public int Scrap;   //스크랩

    [Header("데이터 참조")]
    public PlayerDataSO playerData;
    public List<WeaponDataSO> allWeaponData = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Load();
    }

    public void AddMoney(int amount)
    {
        Money = Mathf.Max(0, Money + amount);
        UIManager.Instance?.UpdateMoney(Money);
        Save();
    }

    public void AddScrap(int amount)
    {
        Scrap = Mathf.Max(0, Scrap + amount);
        UIManager.Instance?.UpdateScrap(Scrap);
        Save();
    }

    public bool TrySpendMoney(int amount)
    {
        if (Money < amount) return false;
        Money -= amount;
        UIManager.Instance?.UpdateMoney(Money);
        Save();
        return true;
    }

    public bool TrySpendScrap(int amount)
    {
        if (Scrap < amount) return false;
        Scrap -= amount;
        UIManager.Instance?.UpdateScrap(Scrap);
        Save();
        return true;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("Scrap", Scrap);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        Money = PlayerPrefs.GetInt("Money", 0);
        Scrap = PlayerPrefs.GetInt("Scrap", 0);
        UIManager.Instance?.UpdateMoney(Money);
        UIManager.Instance?.UpdateScrap(Scrap);
    }
    public void SaveAllData()
    {
        PlayerPrefs.SetInt("Money", Money);
        PlayerPrefs.SetInt("Scrap", Scrap);

        if (playerData != null)
        {
            PlayerPrefs.SetFloat("Player_MaxHP", playerData.maxHP);
            PlayerPrefs.SetFloat("Player_MoveSpeed", playerData.moveSpeed);
        }

        foreach (var weapon in allWeaponData)
        {
            if (weapon == null) continue;

            PlayerPrefs.SetInt($"{weapon.weaponName}_Unlocked", weapon.isUnlocked ? 1 : 0);
            PlayerPrefs.SetFloat($"{weapon.weaponName}_Damage", weapon.damage);
        }

        PlayerPrefs.Save();
        Debug.Log("[DataManager] 데이터 저장 완료");
    }
}
