using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerShooter))]
[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerBase : MonoBehaviour
{
    [Header("데이터")]
    public PlayerDataSO playerData;

    private PlayerController controller;
    private PlayerShooter shooter;
    private PlayerWeaponManager weaponManager;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        shooter = GetComponent<PlayerShooter>();
        weaponManager = GetComponent<PlayerWeaponManager>();

        if (playerData != null)
            playerData.currentHP = playerData.maxHP;
    }

    private void Update()
    {
        // 예시: 체력 체크
        if (playerData.currentHP <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float damage)
    {
        playerData.currentHP -= damage;
        if (playerData.currentHP < 0)
            playerData.currentHP = 0;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdatePlayerHP(playerData.currentHP, playerData.maxHP);
    }

    public void Heal(float amount)
    {
        playerData.currentHP += amount;
        if (playerData.currentHP > playerData.maxHP)
            playerData.currentHP = playerData.maxHP;

        if (UIManager.Instance != null)
            UIManager.Instance.UpdatePlayerHP(playerData.currentHP, playerData.maxHP);
    }

    private void Die()
    {
        // 사망 처리 (예: 리스폰 or 씬 재시작)
        Debug.Log("플레이어 사망");
        controller.enabled = false;
        shooter.enabled = false;
        weaponManager.enabled = false;

        DeathUI deathUI = FindObjectOfType<DeathUI>(true);
        if (deathUI != null)
            deathUI.ShowDeathMessage(false);
        else
            Debug.LogWarning("DeathUI를 찾지 못했습니다!");
    }

    public void ApplyData(PlayerDataSO newData)
    {
        playerData = newData;
        controller.playerData = playerData;
    }
}