using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Show()
    {
        Time.timeScale = 0f;
        rectTransform.localScale = Vector3.one;
    }

    public void Hide()
    {
        Time.timeScale = 1f;
        rectTransform.localScale = Vector3.zero;
    }
}
