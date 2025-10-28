using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerShooter : MonoBehaviour
{
    [Header("참조")]
    public Transform firePoint; // 총알 발사 기준 위치
    [SerializeField] private SpriteRenderer spriteRenderer; //(flipx 제어용)

    private PlayerWeaponManager weaponManager;
    private WeaponBase currentWeapon;
    private float nextFireTime = 0f;

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
        // 마우스 방향 회전
        RotateToMouse();

        // 무기 교체 감지
        if (weaponManager.CurrentWeapon != currentWeapon)
            SetCurrentWeapon();

        // 입력 처리
        if (Input.GetMouseButtonDown(0))
            TryFire();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    // 마우스 방향으로 FirePoint 회전
    private void RotateToMouse()
    {
        if (firePoint == null) return;

        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - playerPos.y;
        float dx = mousePos.x - playerPos.x;
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        //스프라이트는 좌우만 보기
        if (spriteRenderer != null)
            spriteRenderer.flipX = (dx < 0);
    }

    // 현재 무기 갱신 및 탄약 초기화
    private void SetCurrentWeapon()
    {
        currentWeapon = weaponManager.CurrentWeapon;
        if (currentWeapon != null && currentWeapon.weaponData != null)
        {
            currentAmmo = currentWeapon.weaponData.maxAmmo;
            currentMag = currentWeapon.weaponData.maxMag;
            UpdateAmmoUI();
        }
    }

    // 발사 처리
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

    // 재장전 요청
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

    // 재장전 처리 코루틴
    private IEnumerator ReloadRoutine()
    {
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(currentWeapon.weaponData.reloadTime);

        currentMag--;
        currentAmmo = currentWeapon.weaponData.maxAmmo;

        Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}/{currentWeapon.weaponData.maxAmmo}, 예비 탄창: {currentMag}");
        UpdateAmmoUI();
    }

    // 탄약 UI 갱신
    private void UpdateAmmoUI()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateAmmoUI(currentAmmo, currentWeapon.weaponData.maxAmmo, currentMag);
    }
}