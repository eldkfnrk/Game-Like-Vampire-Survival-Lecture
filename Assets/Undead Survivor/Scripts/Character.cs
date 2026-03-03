using UnityEngine;

public class Character : MonoBehaviour
{
    // 이 스크립트는 각 캐릭터마다 고유 능력을 주었을 때 각 캐릭터에 맞게 이를 간단하게 적용시키기 위해 생성한 속성을 저장하는 클래스를 저장하기 위한 스크립트이다.
    // static 값은 정적이라는 뜻으로 이미 메모리에 값이 올라가 있어서 가져다 사용할 수 있게 해준 것이다.(private라면 따로 접근할 수 있도록 해주는 함수가 필요)

    // 캐릭터에 따른 속도 보정값
    public static float Speed
    {
        get
        {
            return GameManager.instance.playerId == 0 ? 1.1f : 1f;
        }
    }

    // 캐릭터에 따른 근접 무기 속도 보정값
    public static float WeaponSpeed
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 1.1f : 1f;
        }
    }

    // 캐릭터에 따른 원거리 무기 발사 속도 보정값
    public static float WeaponRate
    {
        get
        {
            return GameManager.instance.playerId == 1 ? 0.9f : 1f;  // 원거리 무기의 발사 속도는 빨라져야 하기 때문에 10% 작은 수를 반환하는 것이다.
        }
    }

    // 캐릭터에 따른 데미지 보정값
    public static float Damage
    {
        get
        {
            return GameManager.instance.playerId == 2 ? 1.1f : 1f;
        }
    }

    // 캐릭터에 따른 무기 개수(관통력) 추가값
    public static int Count
    {
        get
        {
            return GameManager.instance.playerId == 3 ? 1 : 0;
        }
    }
}
