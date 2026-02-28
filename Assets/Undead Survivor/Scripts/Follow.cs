using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    //플레이어의 움직임이 물리 기반이기 때문에 따라가야 하는 HP바도 물리 기반으로 동작해야 어긋남이 없다.
    private void FixedUpdate()
    {
        if (GameManager.instance.isGameStop)
            return;

        //WorldToScreenPoint - 카메라에 있는 함수 중 하나로 월드 상 오브젝트의 위치를 스크린 좌표로 바꿔주는 함수
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
    }
}
