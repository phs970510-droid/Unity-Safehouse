using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI 참조")]
    public Slider hpBar;
    public TMP_Text ammoText;
    public TMP_Text magText;

    private void Awake()
    {
        Instance = this;
    }
    public void UpdatePlayerHP(float current, float max)
    {
        if (hpBar != null)
            hpBar.value = current / max;
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmoPerMag, int currentMag)
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmoPerMag}";
        if (magText != null)
            magText.text = $"{currentMag} mags";
    }
}
