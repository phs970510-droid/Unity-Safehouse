using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("���� ���� ��� (������)")]
    public List<WeaponBase> weaponPrefabs = new List<WeaponBase>();

    [Header("�ʼ� ����")]
    public Transform weaponHolder;     // Player �ȿ� �� ������Ʈ WeaponHolder
    public Transform playerFirePoint;  // Player �ȿ� FirePoint (���� �߻� ��ġ)

    private List<WeaponBase> weaponInstances = new List<WeaponBase>();
    private int currentIndex = 0;
    public WeaponBase CurrentWeapon { get; private set; }

    private void Start()
    {
        // ���� ������ �ν��Ͻ� ���� & Player �ڽ����� ���̱�
        foreach (WeaponBase prefab in weaponPrefabs)
        {
            if (prefab == null) continue;
            WeaponBase w = Instantiate(prefab, weaponHolder);
            w.firePoint = playerFirePoint;
            w.gameObject.SetActive(false);
            weaponInstances.Add(w);
        }

        if (weaponInstances.Count > 0)
        {
            weaponInstances[0].gameObject.SetActive(true);
            CurrentWeapon = weaponInstances[0];
        }
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        int nextIndex = scroll > 0
            ? (currentIndex + 1) % weaponInstances.Count
            : (currentIndex - 1 + weaponInstances.Count) % weaponInstances.Count;

        ChangeWeapon(nextIndex);
    }

    public void ChangeWeapon(int index)
    {
        if (index < 0 || index >= weaponInstances.Count) return;
        if (index == currentIndex) return;

        for (int i = 0; i < weaponInstances.Count; i++)
        {
            weaponInstances[i].gameObject.SetActive(i == index);
        }

        currentIndex = index;
        CurrentWeapon = weaponInstances[currentIndex];
        CurrentWeapon.firePoint = playerFirePoint;
    }
}