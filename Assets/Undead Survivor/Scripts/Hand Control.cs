using UnityEngine;

public class HandControl : MonoBehaviour
{
    // 스프라이트가 보이는 것을 컨트롤 하기 위한 스프라이트 렌더러. 오른손인지 왼손인지 구분
    public SpriteRenderer spriteR;
    public bool isLeft;  //왼손은 근접 무기, 오른손은 원거리 무기

    // 각 손의 원래 값과 반대로 전환했을 때 값(오른손은 위치 이동이 있을 예정이고 왼손은 방향 전환만 있을 예정)
    Vector3 rightHandPos = new Vector3(0.35f, -0.13f, 0f);
    Vector3 rightHandPosReverse = new Vector3(-0.13f, -0.13f, 0f);
    Quaternion leftHandRot = Quaternion.Euler(0f, 0f, -30f);
    Quaternion leftHandRotReverse = Quaternion.Euler(0f, 0f, -130f);

    // 플레이어의 스프라이트 반전 값을 얻기 위한 스프라이트 렌더러
    SpriteRenderer playerSpriteR;

    private void Awake()
    {
        playerSpriteR = GetComponentsInParent<SpriteRenderer>()[1];  // GetComponentsInParent는 GetComponentsInChildren과 비슷하게 자기 자신에게도 해당 컴포넌트가 있으면 0번 인덱스에 저장하고 다음으로 부모의 것을 저장해서 1번 인덱스를 호출했다.
    }

    private void LateUpdate()
    {
        if (GameManager.instance.isGameStop)
            return;

        bool isReverse = playerSpriteR.flipX;  // 반전 상황인지 구분

        // 모든 transform 관련 동작은 반드시 local에서 진행하여야 한다.(부모를 기준으로 움직여야 하기 때문이다.)
        if (isLeft)  // 근접 무기
        {
            // 전환 상태의 따른 값 변경 및 스프라이트 반전(근접 무기는 이미 회전 값으로 방향은 전환되었기 때문에 어색하지 않도록 y축 반전만 있으면 된다.)
            transform.localRotation = isReverse ? leftHandRotReverse : leftHandRot;
            spriteR.flipY = isReverse;

            // 자연스럽게 보이기 위한 레이어 정리(반전되면 오른손과 왼손의 레이어가 반대되도록 하면 된다.)
            spriteR.sortingOrder = isReverse ? 4 : 6;
        }
        else  // 원거리 무기
        {
            // 전환 상태의 따른 값 변경 및 스프라이트 반전
            transform.localPosition = isReverse ? rightHandPosReverse : rightHandPos;
            spriteR.flipX = isReverse;

            spriteR.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
