using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum ItemType { Money, Scrap, AmmoAR }

    [Header("설정")]
    public ItemType type;
    public int minAmount = 1;
    public int maxAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        int amount = Random.Range(minAmount, maxAmount + 1);

        switch (type)
        {
            case ItemType.Money:
                DataManager.Instance?.AddMoney(amount);
                break;

            case ItemType.Scrap:
                DataManager.Instance?.AddScrap(amount);
                break;

            case ItemType.AmmoAR:
                var shooter = other.GetComponent<PlayerShooter>();
                if (shooter != null)
                {
                    //AR 탄환 드랍 수급 로직 (탄창 단위 30발)
                    int maxCarry = shooter.MaxMag * shooter.MaxAmmo;
                    int currentTotal = shooter.CurrentAmmo + (shooter.CurrentMag * shooter.MaxAmmo);
                    int newTotal = Mathf.Min(currentTotal + amount, maxCarry);

                    int totalAdded = newTotal - currentTotal;

                    // 새 잔탄 수를 탄창 단위로 재분배
                    shooter.AddAmmoAR(totalAdded);
                }
                break;
        }

        Destroy(gameObject);
    }
}
