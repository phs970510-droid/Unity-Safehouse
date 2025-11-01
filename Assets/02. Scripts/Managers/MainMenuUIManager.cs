using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject mainPanel;
    public GameObject loadPanel;

    [Header("버튼")]
    public Button continueButton;
    public Button[] loadSlotButtons = new Button[4];

    private void Start()
    {
        ShowMain();
        UpdateContinueButton();
    }

    public void ShowMain()
    {
        mainPanel.SetActive(true);
        loadPanel.SetActive(false);
    }

    public void ShowLoad()
    {
        mainPanel.SetActive(false);
        loadPanel.SetActive(true);
        UpdateLoadSlotButtons();
    }
    public void OnContinue()
    {
        int lastSlot = PlayerPrefs.GetInt("LastSaveSlot", 1);
        if (DataManager.Instance.HasSaveSlot(lastSlot))
        {
            DataManager.Instance.LoadAllData(lastSlot);
            SceneManager.LoadScene("Safehouse");
        }
        else
        {
            Debug.Log("저장된 세이브가 없습니다!");
        }
    }

    //슬롯 직접 선택 로드
    public void OnLoadSlot(int slotIndex)
    {
        if (DataManager.Instance.HasSaveSlot(slotIndex))
        {
            DataManager.Instance.LoadAllData(slotIndex);
            PlayerPrefs.SetInt("LastSaveSlot", slotIndex);
            SceneManager.LoadScene("Safehouse");
        }
        else
        {
            Debug.Log($"슬롯 {slotIndex}에 저장된 데이터가 없습니다!");
        }
    }

    public void OnStartNew()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Tutorial");
    }

    public void OnExit()
    {
        Application.Quit();
    }

    private void UpdateContinueButton()
    {
        bool anySave = false;
        for (int i = 1; i <= 4; i++)
        {
            if (DataManager.Instance.HasSaveSlot(i)) { anySave = true; break; }
        }
        continueButton.interactable = anySave;
    }

    private void UpdateLoadSlotButtons()
    {
        for (int i = 1; i <= 4; i++)
        {
            bool hasSave = DataManager.Instance.HasSaveSlot(i);
            loadSlotButtons[i - 1].interactable = hasSave;
        }
    }
}
