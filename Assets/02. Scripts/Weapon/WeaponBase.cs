using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [Header("������ ����")]
    public WeaponDataSO weaponData;

    [Header("�߻� ����")]
    public GameObject bulletPrefab;
    [HideInInspector] public Transform firePoint; // Player���� ���Ե�

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