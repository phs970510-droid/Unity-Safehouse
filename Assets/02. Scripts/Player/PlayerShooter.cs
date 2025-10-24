using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerShooter : MonoBehaviour
{
    private PlayerWeaponManager weaponManager;
    private WeaponBase currentWeapon;
    private float nextFireTime = 0.0f;

    private int currentAmmo;
    private int currentMag;

    private void Awake()
    {
        weaponManager = GetComponent<PlayerWeaponManager>();
    }

    private void Start()
    {
        SetCurrentWeapon();
    }

    private void Update()
    {
        if (weaponManager.CurrentWeapon != currentWeapon)
            SetCurrentWeapon();
        if (Input.GetMouseButtonDown(0))
            TryFire();
        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void SetCurrentWeapon()
    { 
        currentWeapon = weaponManager.CurrentWeapon;
        if (currentWeapon != null && currentWeapon.weaponData != null)
        {
            currentAmmo = currentWeapon.weaponData.maxAmmo;
            currentMag=currentWeapon.weaponData.maxMag;
            UpdateAmmoUI();
        }
    }
    private void TryFire()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;
        if (Time.time < nextFireTime)
            return;
        if (currentAmmo <= 0)
        {
            Debug.Log("Out of ammo! Press R to reload.");
            return;
        }

        currentWeapon.Fire();
        currentAmmo--;
        nextFireTime = Time.time + currentWeapon.weaponData.fireRate;
        UpdateAmmoUI();
    }
    private void Reload()
    {
        if (currentMag > 0 && currentAmmo < currentWeapon.weaponData.maxAmmo)
        {
            StopAllCoroutines();
            StartCoroutine(ReloadRoutine());
        }
        else
        {
            Debug.Log("재장전 불가 (예비 탄창 없음)");
        }
    }
    private IEnumerator ReloadRoutine()
    {
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(currentWeapon.weaponData.reloadTime);

        currentMag--;
        currentAmmo = currentWeapon.weaponData.maxAmmo;

        Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}/{currentWeapon.weaponData.maxAmmo}, 예비 탄창: {currentMag}");

        UpdateAmmoUI();
    }
    private void UpdateAmmoUI()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateAmmoUI(currentAmmo, currentWeapon.weaponData.maxAmmo, currentMag);
    }
}
