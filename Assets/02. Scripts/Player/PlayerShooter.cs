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
    private int currentExtra;

    private UIManager ui;

    private bool isAutoFire = false;    //기본은 단발

<<<<<<< Updated upstream
=======
    private Dictionary<WeaponBase, (int ammo, int mag)> weaponAmmoState
        = new Dictionary<WeaponBase, (int ammo, int mag)>();

    public int CurrentAmmo => currentAmmo;
    public int CurrentMag => currentMag;
    public int MaxAmmo => currentWeapon != null ? currentWeapon.weaponData.maxAmmo : 0;
    public int MaxMag => currentWeapon != null ? currentWeapon.weaponData.maxMag : 0;

>>>>>>> Stashed changes
    private void Awake()
    {
        weaponManager = GetComponent<PlayerWeaponManager>();
    }

    private void Start()
    {
        SetCurrentWeapon();
    }

    //탄 수급용 메서드 (PickupItem에서 호출)
    public void AddAmmoAR(int amount)
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        int maxAmmo = currentWeapon.weaponData.maxAmmo;
        int maxMag = currentWeapon.weaponData.maxMag;

        //여분 탄 먼저 추가
        currentExtra += amount;

        //여분 탄이 한 탄창 이상이면 탄창으로 변환
        while (currentExtra >= maxAmmo && currentMag < maxMag)
        {
            currentExtra -= maxAmmo;
            currentMag++;
        }

        UpdateAmmoUI();
        Debug.Log($"[PlayerShooter] AR 탄환 +{amount} → {currentMag}mags + {currentExtra}bullets");
    }
    private void Update()
    {
        // 마우스 방향 회전
        RotateToMouse();

        // 무기 교체 감지
        if (weaponManager.CurrentWeapon != currentWeapon)
            SetCurrentWeapon();

        HandleFire();             //발사 입력 처리
        HandleFireModeSwitch();   //발사 모드 전환

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
        {
            bool flip = (dx < 0);
            spriteRenderer.flipX = flip;

            //무기 회전 보정
            if (flip)
            {
                firePoint.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                firePoint.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void HandleFire()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        float fireRate = currentWeapon.weaponData.fireRate;

        if (isAutoFire)
        {
            // 연사 모드: 좌클릭 유지
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                TryFire();
            }
        }
        else
        {
            // 단발 모드: 좌클릭 눌렀을 때만
            if (Input.GetMouseButtonDown(0))
            {
                TryFire();
            }
        }
    }

    private void HandleFireModeSwitch()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (currentWeapon != null && currentWeapon.weaponData.weaponName == "AR")
            {
                isAutoFire = !isAutoFire;
                string mode = isAutoFire ? "연사" : "단발";
                Debug.Log($"[{currentWeapon.weaponData.weaponName}] 사격 모드 전환: {mode}");
            }
            else
            {
                Debug.Log("이 무기는 단발 전용입니다.");
            }
        }
    }

    //현재 무기 갱신 및 탄약 초기화
    private void SetCurrentWeapon()
    {
        currentWeapon = weaponManager.CurrentWeapon;
        if (currentWeapon != null && currentWeapon.weaponData != null)
        {
<<<<<<< Updated upstream
=======
            currentAmmo = saved.ammo;
            currentMag = saved.mag;
        }
        else
        {
            //첫 사용 무기면 기본값 초기화
>>>>>>> Stashed changes
            currentAmmo = currentWeapon.weaponData.maxAmmo;
            currentMag = currentWeapon.weaponData.maxMag;
            UpdateAmmoUI();
        }
    }

    //발사 처리
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

    //재장전 요청
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

    //재장전 처리 코루틴
    private IEnumerator ReloadRoutine()
    {
        Debug.Log("재장전 중...");
        yield return new WaitForSeconds(currentWeapon.weaponData.reloadTime);

        currentMag--;
        currentAmmo = currentWeapon.weaponData.maxAmmo;

        Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}/{currentWeapon.weaponData.maxAmmo}, 예비 탄창: {currentMag}");
        UpdateAmmoUI();
    }

    //탄약 UI 갱신
    private void UpdateAmmoUI()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateAmmoUI(currentAmmo, currentWeapon.weaponData.maxAmmo, currentMag, currentExtra);
    }
}