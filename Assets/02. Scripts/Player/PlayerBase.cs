using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(PlayerShooter))]
[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerBase : MonoBehaviour
{
    [Header("������")]
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
        // ����: ü�� üũ
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
    }

    public void Heal(float amount)
    {
        playerData.currentHP += amount;
        if (playerData.currentHP > playerData.maxHP)
            playerData.currentHP = playerData.maxHP;
    }

    private void Die()
    {
        // ��� ó�� (��: ������ or �� �����)
        Debug.Log("�÷��̾� ���");
        controller.enabled = false;
        shooter.enabled = false;
        weaponManager.enabled = false;
    }

    public void ApplyData(PlayerDataSO newData)
    {
        playerData = newData;
        controller.playerData = playerData;
    }
}