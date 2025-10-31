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

    [SerializeField] private TMPro.TMP_Text moneyText;
    [SerializeField] private TMPro.TMP_Text scrapText;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateAmmoUI(int currentAmmo, int maxAmmoPerMag, int currentMag, int extraBullets=0)
    {
        if (ammoText != null)
            ammoText.text = $"{currentAmmo} / {maxAmmoPerMag}";
        if (magText != null)
            magText.text = $"{currentMag} mags + {extraBullets} bullets";
    }
<<<<<<< Updated upstream
=======
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Safehouse")
            gameObject.SetActive(false);
    }

    public void UpdateMoney(int value)
    {
        if (moneyText != null) moneyText.text = $"$ {value}";
    }
    public void UpdateScrap(int value)
    {
        if (scrapText != null) scrapText.text = $"Scrap: {value}";
    }
>>>>>>> Stashed changes
}
