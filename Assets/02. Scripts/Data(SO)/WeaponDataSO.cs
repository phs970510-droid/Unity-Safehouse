using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponData", menuName ="Game/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("기본")]
    public string weaponName="Pistol";
    [Tooltip("발당 데미지")]
    public float damage = 10.0f;
    [Tooltip("연사 간격(초)")]
    public float fireRate = 1.0f;
    [Tooltip("재장전 시간(초)")]
    public float reloadTime = 1.0f;

    [Header("탄약 설정")]
    [Tooltip("탄창당 탄 수")]
    public float maxAmmo = 12;
    [Tooltip("최대 탄창 수")]
    public int maxMag = 5;

    [Header("해금 여부")]
    public bool isUnlocked = true;
}
