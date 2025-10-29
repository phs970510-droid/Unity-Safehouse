using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("UI 참조")]
    public Text ammoText;
    public Text magText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmoPerMag, int currentMag)
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmoPerMag}";
        if (magText != null)
            magText.text = $"{currentMag} mags";
    }
}
