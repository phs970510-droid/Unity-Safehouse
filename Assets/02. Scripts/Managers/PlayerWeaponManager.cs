using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    [Header("���� ���� ���")]
    public List<WeaponBase> weaponPrefabs = new List<WeaponBase>();

    private int currentIndex = 0;
    public WeaponBase CurrentWeapon { get; private set;}

    private void Start()
    {
        //����� �ѹ��� �ϳ��� Ȱ��ȭ
        for (int i = 0; i < weaponPrefabs.Count; i++) 
        {
            if (weaponPrefabs[i] != null)
                weaponPrefabs[i].gameObject.SetActive(i==currentIndex);
        }
        CurrentWeapon = weaponPrefabs[currentIndex];
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(Mathf.Abs(scroll)<0.01f)return;

        int nextIndex = currentIndex;
        if (scroll > 0)
        {
            nextIndex = (currentIndex + 1) % weaponPrefabs.Count;
        }
        else if (scroll < 0) 
        {
            nextIndex = (currentIndex - 1+weaponPrefabs.Count) % weaponPrefabs.Count;
        }
        ChangeWeapon(nextIndex);
    }

    public void ChangeWeapon(int index)
    {
        if (index < 0)
        {
            return;
        }
        else if (index >= weaponPrefabs.Count)
        {
            return;
        }
        else if (weaponPrefabs[index] == null)
        {
            return;
        }
        else if (weaponPrefabs[index].weaponData == null)
        {
            return;
        }
        else if (!weaponPrefabs[index].weaponData.isUnlocked)
        {
            Debug.LogWarning("�رݵ��� ���� �����Դϴ�!");
            return;
        }

        for (int i = 0; i < weaponPrefabs.Count; i++) 
        {
            if (weaponPrefabs[i] != null)
            {
                weaponPrefabs[i].gameObject.SetActive(i==index);
            }
        }
        currentIndex = index;
        CurrentWeapon=weaponPrefabs[currentIndex];
    }
}
