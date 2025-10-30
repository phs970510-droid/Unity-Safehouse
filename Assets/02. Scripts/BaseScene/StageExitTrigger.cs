using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0f;

            DeathUI deathUI = FindObjectOfType<DeathUI>(true);
            if (deathUI != null)
            {
                if (!deathUI.gameObject.activeSelf)
                    deathUI.gameObject.SetActive(true);
                deathUI.ShowDeathMessage(true);
            }
            else
            {
                Debug.LogError("[StageExitTrigger] DeathUI를 찾을 수 없습니다. DeathPanel 연결을 확인하세요!");
            }
        }
        else
            return;
    }
}
