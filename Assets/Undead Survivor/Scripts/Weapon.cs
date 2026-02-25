using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponId;
    public int prefabId;
    public float damage;
    public float rotateSpeed;
    public int count;

    float timer;
    Player player;

    private void Awake()
    {
        //player = GetComponentInParent<Player>();
        player = GameManager.instance.player;
    }

    void Update()
    {
        switch (weaponId)
        {
            case 0:
                transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);  //Vector3.back은 z축 -1의 값을 가지는데 시계 방향으로 회전하는 것은 -이기 때문이고 반시계 방향은 +이기 때문에 반시계 방향을 원하면 Vector3.forward를 사용하여도 된다.
                break;
            default:
                timer += Time.deltaTime;

                if(timer > rotateSpeed)  //원거리 발사체의 경우 rotateSpeed 변수가 회전 속도가 아닌 발사 속도로 볼 것이다.
                {
                    timer = 0f;
                    Fire();
                }
                break;
        }

    }

    public void WeaponInit(ItemData data)
    {
        // Basic Set
        name = string.Format("Weapon {0}", data.itemId);
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        weaponId = data.itemId;
        damage = data.baseDamage;
        count = data.baseCount;

        for (int i = 0; i < GameManager.instance.poolManager.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.poolManager.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch (weaponId)
        {
            case 0:
                rotateSpeed = 150f;
                WeaponPosition();
                break;
            default:
                rotateSpeed = 0.6f;
                break;
        }

        // Hand Control set - 알맞는 무기를 든 손에 대한 정보 초기화(알맞는 손의 HandControl 스크립트를 가지고 와서 이 스크립트를 가진 손의 스프라이트를 저장 및 적용하고 손을 활성화)
        HandControl hand = player.handControls[(int)data.itemType];  // 열거형은 기본적으로는 정수형 값을 가진다. 그러나 타입은 열거형 타입을 가지기 때문에 정수로 쓰려면 이렇게 강제 형 변환 등을 이용한 형 변환이 필요하다.
        hand.spriteR.sprite = data.hand;
        hand.gameObject.SetActive(true);

        // 이미 레벨 업한 기어의 변경 값을 적용하기 위해 변경 값 적용 함수를 실행시켜야 한다.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    void WeaponPosition()
    {
        float distance = 1.5f;  //임의로 둔 플레이어와의 거리
        for(int index = 0; index < count; index++)
        {
            Transform weaponTr = null;

            if(index < transform.childCount)
            {
                weaponTr = transform.GetChild(index);
            }
            else
            {
                weaponTr = GameManager.instance.poolManager.GetObject(prefabId).transform;
                weaponTr.parent = transform;
            }

            weaponTr.localPosition = Vector3.zero;
            weaponTr.localRotation = Quaternion.identity;  //Quaternion는 회전 값을 가지는 것이고 Quaternion.identity는 회전 값이 0임을 의미한다.

            Vector3 rotateVec = Vector3.forward * 360 * index / count;
            weaponTr.Rotate(rotateVec);
            weaponTr.Translate(weaponTr.up * distance, Space.World);  //자기 자신을 기준으로 위로 이동했으니 씬 내에서는 월드(글로벌)로 반영되어야 한다. 자기 자신을 기준으로 이동하지 않은 상태라면 Space.Self를 이용하면 된다.

            weaponTr.GetComponent<Bullet>().BulletInit(damage, -1);  // -1 is Infinitly per.(-1은 무한으로 관통한다는 의미)
        }
    }

    void Fire()
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 direction = targetPos - transform.position;
        direction = direction.normalized;

        Transform bullet = GameManager.instance.poolManager.GetObject(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        bullet.parent = transform;

        bullet.GetComponent<Bullet>().BulletInit(damage, count, direction);  //count는 근거리 무기에서는 무기 개수였지만 원거리 총알에서는 관통력으로 사용
    }

    public void LevelUp(float damage, int count)
    {
        // 데미지는 상승률을 적용한 값을 받고 관통력은 추가 값을 받는 형식
        this.damage = damage;
        this.count += count;

        // 근접 무기의 경우 무기 재배치
        if (weaponId == 0)
            WeaponPosition();

        // 무기가 레벨 업을 한 값에 기어의 적용 값을 적용시켜야 하기 때문에 변경 값 적용 함수를 실행시켜야 한다.
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
}
