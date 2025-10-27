using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("데이터 참조")]
    public WeaponDataSO weaponData;

    [Header("발사 관련")]
    public GameObject bulletPrefab;
    [HideInInspector] public Transform firePoint; // Player에서 주입됨

    public void Fire()
    {
        if (weaponData == null || bulletPrefab == null || firePoint == null)
            return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet b = bullet.GetComponent<Bullet>();
        if (b != null)
        {
            b.Initialize(weaponData.damage);
        }
    }
}