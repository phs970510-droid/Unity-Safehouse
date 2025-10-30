using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KeyGuides : MonoBehaviour
{
    [Header("안내 텍스트")]
    [SerializeField] private TMP_Text hintText;
    [TextArea]
    public string message =
        "WASD : 이동\n" +
        "마우스 : 조준\n" +
        "좌클릭 : 사격\n" +
        "R : 재장전\n" +
        "B : 단발 / 연사 전환\n" +
        "E : 상호작용 (예: 상점 입장)\n\n" +
        "[Stage Exit] 구역으로 이동해 상점으로 넘어가세요!";

    [Header("표시 설정")]
    [SerializeField] private float autoHideDelay = 5f;   // 자동 숨김 시간
    [SerializeField] private KeyCode toggleKey = KeyCode.H;

    private CanvasGroup canvasGroup;
    private bool isVisible = true;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    private IEnumerator Start()
    {
        if (hintText != null)
            hintText.text = message;

        ShowHint(true);  // 처음엔 표시
        yield return new WaitForSeconds(autoHideDelay);
        ShowHint(false); // 일정 시간 뒤 자동 숨김
    }

    private void Update()
    {
        // 토글 키로 표시/숨김 전환
        if (Input.GetKeyDown(toggleKey))
        {
            ShowHint(!isVisible);
        }
    }

    private void ShowHint(bool show)
    {
        isVisible = show;
        canvasGroup.alpha = show ? 1f : 0f;
        canvasGroup.interactable = show;
        canvasGroup.blocksRaycasts = show;
    }
}
