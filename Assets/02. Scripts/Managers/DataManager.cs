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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
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
    public void SaveAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";
        PlayerPrefs.SetInt(prefix + "Money", Money);
        PlayerPrefs.SetInt(prefix + "Scrap", Scrap);

        if (playerData != null)
        {
            PlayerPrefs.SetFloat(prefix + "Player_MaxHP", playerData.maxHP);
            PlayerPrefs.SetFloat(prefix + "Player_MoveSpeed", playerData.moveSpeed);
        }

        foreach (var weapon in allWeaponData)
        {
            if (weapon == null) continue;
            PlayerPrefs.SetInt(prefix + $"{weapon.weaponName}_Unlocked", weapon.isUnlocked ? 1 : 0);
            PlayerPrefs.SetFloat(prefix + $"{weapon.weaponName}_Damage", weapon.damage);
        }

        PlayerPrefs.Save();
        Debug.Log($"[DataManager] 슬롯 {slotIndex} 저장 완료");
    }
    public void LoadAllData(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";
        Money = PlayerPrefs.GetInt(prefix + "Money", 0);
        Scrap = PlayerPrefs.GetInt(prefix + "Scrap", 0);

        if (playerData != null)
        {
            playerData.maxHP = PlayerPrefs.GetFloat(prefix + "Player_MaxHP", playerData.maxHP);
            playerData.moveSpeed = PlayerPrefs.GetFloat(prefix + "Player_MoveSpeed", playerData.moveSpeed);
        }

        foreach (var weapon in allWeaponData)
        {
            if (weapon == null) continue;
            weapon.isUnlocked = PlayerPrefs.GetInt(prefix + $"{weapon.weaponName}_Unlocked", 0) == 1;
            weapon.damage = PlayerPrefs.GetFloat(prefix + $"{weapon.weaponName}_Damage", weapon.damage);
        }

        UIManager.Instance?.UpdateMoney(Money);
        UIManager.Instance?.UpdateScrap(Scrap);

        Debug.Log($"[DataManager] 슬롯 {slotIndex} 불러오기 완료");
    }
    public bool HasSaveSlot(int slotIndex)
    {
        string prefix = $"Save{slotIndex}_";
        return PlayerPrefs.HasKey(prefix + "Money") || PlayerPrefs.HasKey(prefix + "Player_MaxHP");
    }
}
