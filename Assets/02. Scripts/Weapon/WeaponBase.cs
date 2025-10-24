using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("������ ����")]
    public WeaponDataSO weaponData;

    [Header("�߻� ����")]
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
