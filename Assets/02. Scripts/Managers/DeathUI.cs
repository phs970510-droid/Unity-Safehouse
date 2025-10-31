using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;

    [Header("페이드 설정")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration = 1.5f;

    private bool isEscape;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    //사망 or 생존시
    public void ShowDeathMessage(bool escape = false)
    {
        isEscape = escape;

        if (panel != null)
            panel.SetActive(true);

        Time.timeScale = 0f;

        if (messageText != null)
        {
            messageText.text = escape ? "탈출 성공!" : "사망하셨습니다";
            messageText.color = escape ? Color.green : Color.red;
        }

        StartCoroutine(FadeIn());
    }
    private void OnEnable()
    {
        //DeathPanel이 에디터에서 비활성 상태로 되어있을 경우 대비
        if (panel != null && !panel.activeSelf)
            panel.SetActive(true);

        //캔버스 그룹 알파 보정
        if (canvasGroup != null && canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            if (canvasGroup != null)
                canvasGroup.alpha = t;
            yield return null;
        }

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    //버튼용
    public void OnToSafehouse()
    {
        Time.timeScale = 1f;
        //이후 Safehouse 씬 완성 시 정산 로직 추가
        SceneManager.LoadScene("Safehouse");
    }
}
