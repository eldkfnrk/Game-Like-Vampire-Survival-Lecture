using UnityEngine;

public class Gear : MonoBehaviour
{
    // 장비 타입 정보, 적용할 비율 값을 변수로 저장
    public ItemData.ItemType type;
    public float rate;

    // 장비 정보를 초기화하는 함수
    public void GearInit(ItemData data)
    {
        // basic set
        name = string.Format("Gear {0}", data.itemId);
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;

        //property set
        type = data.itemType;
        rate = data.damages[0];  //초기화는 처음 레벨 업 때 호출되니 1레벨 적용 값인 0번 인덱스를 저장

        ApplyGear();
    }

    // 레벨 업 함수 - 레벨에 맞는 비율 값을 인자로 수신하여 적용
    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    // 변경된 비율을 적용하는 함수
    void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            default:
                SpeedUp();
                break;
        }
    }

    // 장갑일 때와 신발일 때 알맞게 값을 올리는 함수 ...
    void RateUp()
    {
        // 장갑의 경우 모든 무기의 연사력을 올리는 장비이기 때문에 모든 무기 정보를 가져와야 한다.
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach(Weapon weapon in weapons)
        {
            // 각 레벨에 맞는 연사력 상승률을 적용(중첩이 아닌 개별 값 적용)
            switch (weapon.weaponId)
            {
                case 0:  // 근거리 무기
                    weapon.rotateSpeed = 150f + 150f * rate;  // 근거리 무기의 기본 값이 150
                    break;
                default:  // 원거리 무기
                    weapon.rotateSpeed = 0.6f * (1f - rate); // 근거리 무기의 기본 값이 0.6(원거리 무기는 발사 시간이 짧아져야 성능이 올라가는 것이기 때문에 약간 다른 적용 방법을 사용해야 한다.)
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3f;  // 플레이어의 기본 이동 속력
        GameManager.instance.player.moveSpeed += speed * rate;  // 속도 상승 또한 중첩이 아닌 레벨에 대응하는 값 적용
    }
}
