using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionBoard : MonoBehaviour
{
    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void GoToStage01()
    {
        SceneManager.LoadScene("Stage01");
    }
}
