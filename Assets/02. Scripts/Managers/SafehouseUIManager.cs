using UnityEngine;

public class SafehouseUIManager : MonoBehaviour
{
    public GameObject canvasMain;
    public GameObject canvasShop;
    public GameObject canvasUpgrade;
    public GameObject canvasMission;

    private void Start()
    {
        ShowMain();
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
    }
}
