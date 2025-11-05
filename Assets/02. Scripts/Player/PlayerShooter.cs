using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponManager))]
public class PlayerShooter : MonoBehaviour
{
    [Header("참조")]
    public Transform firePoint; //총알 발사 기준 위치
    [SerializeField] private SpriteRenderer spriteRenderer;

    private PlayerWeaponManager weaponManager;
    private WeaponBase currentWeapon;
    private float nextFireTime = 0f;

    private int currentAmmo;
    private int currentMag;
    private int currentExtra;

    private UIManager ui;

    private bool isAutoFire = false;    //기본은 단발

    private Dictionary<WeaponBase, (int ammo, int mag)> weaponAmmoState
        = new Dictionary<WeaponBase, (int ammo, int mag)>();

    [Header("오디오 출력")]
    [SerializeField] private AudioSource fireAudioSource;

    public int CurrentAmmo => currentAmmo;
    public int CurrentMag => currentMag;
    public int MaxAmmo => currentWeapon != null ? currentWeapon.weaponData.maxAmmo : 0;
    public int MaxMag => currentWeapon != null ? currentWeapon.weaponData.maxMag : 0;

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
        //마우스 방향 회전
        RotateToMouse();

        //무기 교체 감지
        if (weaponManager.CurrentWeapon != currentWeapon)
            SetCurrentWeapon();

        HandleFire();             //발사 입력 처리
        HandleFireModeSwitch();   //발사 모드 전환

        if (Input.GetKeyDown(KeyCode.R))
            Reload();
    }

    private void LateUpdate()
    {
        if (Camera.main != null)
        {
            // 회전값만 고정 (위치는 부모 따라감)
            Camera.main.transform.rotation = Quaternion.identity;
        }
    }

    //마우스 방향으로 회전
    private void RotateToMouse()
    {
        if (Camera.main == null)
            return;

        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dirVec = mousePos - (Vector2)transform.position;

        transform.right= dirVec.normalized;
    }

    private void HandleFire()
    {
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        float fireRate = currentWeapon.weaponData.fireRate;

        if (isAutoFire)
        {
            //연사 모드: 좌클릭 유지
            if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
            {
                TryFire();
            }
        }
        else
        {
            //단발 모드: 좌클릭 눌렀을 때만
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
        if (currentWeapon != null)
        {
            weaponAmmoState[currentWeapon] = (currentAmmo, currentMag);
        }

        currentWeapon = weaponManager.CurrentWeapon;
        if (currentWeapon == null || currentWeapon.weaponData == null)
            return;

        if (weaponAmmoState.TryGetValue(currentWeapon, out var saved))
        {
            currentAmmo = saved.ammo;
            currentMag = saved.mag;
        }
        else
        {
            //첫 사용 무기면 기본값 초기화
            currentAmmo = currentWeapon.weaponData.maxAmmo;
            currentMag = currentWeapon.weaponData.maxMag;
            weaponAmmoState[currentWeapon] = (currentAmmo, currentMag);
        }

        UpdateAmmoUI();
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

        if (fireAudioSource != null && currentWeapon.weaponData.fireSound != null)
        {
            fireAudioSource.PlayOneShot(currentWeapon.weaponData.fireSound);
        }
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
        //단발용
        if (currentWeapon.weaponData.isSingleLoad)
        {
            yield return StartCoroutine(ReloadSingleLoadWeapon());
            yield break;
        }

        Debug.Log("재장전 중...");

        if (fireAudioSource != null && currentWeapon.weaponData.reloadSound != null)
        {
            fireAudioSource.PlayOneShot(currentWeapon.weaponData.reloadSound);
        }

        yield return new WaitForSeconds(currentWeapon.weaponData.reloadTime);

        currentMag--;
        currentAmmo = currentWeapon.weaponData.maxAmmo;

        Debug.Log($"재장전 완료! 현재 탄약: {currentAmmo}/{currentWeapon.weaponData.maxAmmo}, 예비 탄창: {currentMag}");
        UpdateAmmoUI();
    }

    private IEnumerator ReloadSingleLoadWeapon()
    {
        Debug.Log("1발 단위 리로드 시작");

        float perShellTime = currentWeapon.weaponData.reloadTime;
        int needed = currentWeapon.weaponData.maxAmmo - currentAmmo;

        for (int i = 0; i < needed; i++)
        {
            //발사 시 리로드 중단
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("리로드 중단: 발사 명령");
                yield break;
            }

            //예비탄(=currentMag)이 없으면 더 이상 장전 불가
            if (currentMag <= 0)
            {
                Debug.Log("예비탄이 없습니다!");
                break;
            }

            //장전 사운드
            if (fireAudioSource != null && currentWeapon.weaponData.reloadSound != null)
                fireAudioSource.PlayOneShot(currentWeapon.weaponData.reloadSound);

            yield return new WaitForSeconds(perShellTime);

            //실제 탄 이동
            currentAmmo++;
            currentMag--; //예비탄 1개 사용
            UpdateAmmoUI();

            //마지막 장전 후 펌프 동작음
            if (i == needed - 1 && currentWeapon.weaponData.pumpSound != null)
                fireAudioSource.PlayOneShot(currentWeapon.weaponData.pumpSound);
        }

        Debug.Log("1발 단위 리로드 완료");
    }

    //탄약 UI 갱신
    private void UpdateAmmoUI()
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateAmmoUI(currentAmmo, currentWeapon.weaponData.maxAmmo, currentMag, currentExtra);
    }
}