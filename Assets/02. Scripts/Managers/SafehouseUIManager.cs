using UnityEngine;

public class SafehouseUIManager : MonoBehaviour
{
    public GameObject canvasMain;
    public GameObject canvasShop;
    public GameObject canvasUpgrade;
    public GameObject canvasMission;
    public GameObject canvasSaveSlots;

    private void Start()
    {
        ShowMain();
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoney(DataManager.Instance.Money);
            UIManager.Instance.UpdateScrap(DataManager.Instance.Scrap);
        }
    }

    public void ShowMain() => SetActiveCanvas(canvasMain);
    public void ShowShop() => SetActiveCanvas(canvasShop);
    public void ShowUpgrade() => SetActiveCanvas(canvasUpgrade);
    public void ShowMission() => SetActiveCanvas(canvasMission);

    private void SetActiveCanvas(GameObject target)
    {
        canvasMain.SetActive(target == canvasMain);
        canvasShop.SetActive(target == canvasShop);
        canvasUpgrade.SetActive(target == canvasUpgrade);
        canvasMission.SetActive(target == canvasMission);
        if (canvasSaveSlots != null) canvasSaveSlots.SetActive(target == canvasSaveSlots);
    }
    public void OnSaveAndExitMenu()
    {
        SetActiveCanvas(canvasSaveSlots);
    }
    public void OnSelectSaveSlot(int slotIndex)
    {
        DataManager.Instance.SetCurrentSlot(slotIndex);
        Debug.Log($"[SafehouseUIManager] 슬롯 {slotIndex} 저장 중...");
        DataManager.Instance.SaveAllData(slotIndex);
        PlayerPrefs.SetInt("LastSaveSlot", slotIndex);
        PlayerPrefs.Save();

        Debug.Log($"[SafehouseUIManager] 슬롯 {slotIndex} 저장 완료!");
        Debug.Log("저장 경로: " + Application.persistentDataPath);

        Application.Quit();
    }

    public void OnSaveCancel()
    {
        ShowMain();
    }
}
