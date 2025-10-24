using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponData", menuName ="Game/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("�⺻")]
    public string weaponName="Pistol";
    [Tooltip("�ߴ� ������")]
    public float damage = 10.0f;
    [Tooltip("���� ����(��)")]
    public float fireRate = 1.0f;
    [Tooltip("������ �ð�(��)")]
    public float reloadTime = 1.0f;

    [Header("ź�� ����")]
    [Tooltip("źâ�� ź ��")]
    public float maxAmmo = 12;
    [Tooltip("�ִ� źâ ��")]
    public int maxMag = 5;

    [Header("�ر� ����")]
    public bool isUnlocked = true;
}
