using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("데이터 참조")]
    public WeaponDataSO weaponData;

    [Header("발사 관련")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    public void Fire()
    {
        if (weaponData == null)
        {
            return;
        }
        else if (bulletPrefab == null)
        {
            return;
        }
        else if (firePoint == null)
        {
            return;
        }
        GameObject bullet=Instantiate(bulletPrefab, firePoint.position,firePoint.rotation);
        Bullet bull=bullet.GetComponent<Bullet>();
        if (bull != null) 
        {
            bull.Initialize(weaponData.damage);
        }
    }
}
