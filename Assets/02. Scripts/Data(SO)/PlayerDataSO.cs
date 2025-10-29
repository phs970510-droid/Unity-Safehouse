using UnityEngine;

[CreateAssetMenu(fileName ="PlayerData",menuName ="Game/Player Data")]
public class PlayerDataSO : ScriptableObject
{
    [Header("�⺻ ����")]
    public float maxHP = 100.0f;
    public float moveSpeed = 5.0f;

    [Header("���� ����ġ(�����)")]
    public float damageBonus = 0.0f;
    public float reloadSpeedBonus = 0.0f;
    public float fireRateBonus = 0.0f;

    [Header("��Ÿ �߰�����")]
    [HideInInspector] public float currentHP;
    public void UpgradeHP(float value)
    {
        maxHP += value;
        currentHP = maxHP;
    }
    public void UpgradeMoveSpeed(float value) 
    { 
        moveSpeed += value;
    }
    public void UpgradeDamage(float value)
    { 
        damageBonus += value; 
    }
    public void UpgradeReloadSpeed(float value)
    { 
        reloadSpeedBonus += value; 
    }
    public void UpgradeFireRate(float value)
    { 
        fireRateBonus += value; 
    }
}
