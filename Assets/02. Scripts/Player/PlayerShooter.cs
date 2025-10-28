using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerShooter : MonoBehaviour
{
    [Header("����")]
    public Transform firePoint; // �Ѿ� �߻� ���� ��ġ
    [SerializeField] private SpriteRenderer spriteRenderer; //(flipx �����)

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
        // ���콺 ���� ȸ��
        RotateToMouse();

        // ���� ��ü ����
        if (weaponManager.CurrentWeapon != currentWeapon)
            SetCurrentWeapon();

        // �Է� ó��
        if (Input.GetMouseButtonDown(0))
            TryFire();

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    // ���콺 �������� FirePoint ȸ��
    private void RotateToMouse()
    {
        if (firePoint == null) return;

        Vector2 playerPos = transform.position;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        float dy = mousePos.y - playerPos.y;
        float dx = mousePos.x - playerPos.x;
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        //��������Ʈ�� �¿츸 ����
        if (spriteRenderer != null)
            spriteRenderer.flipX = (dx < 0);
    }

    // ���� ���� ���� �� ź�� �ʱ�ȭ
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

    // �߻� ó��
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

    // ������ ��û
    private void Reload()
    {
        if (currentMag > 0 && currentAmmo < currentWeapon.weaponData.maxAmmo)
        {
            StopAllCoroutines();
            StartCoroutine(ReloadRoutine());
        }
        else
        {
            Debug.Log("������ �Ұ� (���� źâ ����)");
        }
    }

    // ������ ó�� �ڷ�ƾ
    private IEnumerator ReloadRoutine()
    {
        Debug.Log("������ ��...");
        yield return new WaitForSeconds(currentWeapon.weaponData.reloadTime);

        currentMag--;
        currentAmmo = currentWeapon.weaponData.maxAmmo;

        Debug.Log($"������ �Ϸ�! ���� ź��: {currentAmmo}/{currentWeapon.weaponData.maxAmmo}, ���� źâ: {currentMag}");
        UpdateAmmoUI();
    }

    // ź�� UI ����
    private void UpdateAmmoUI()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateAmmoUI(currentAmmo, currentWeapon.weaponData.maxAmmo, currentMag);
    }
}