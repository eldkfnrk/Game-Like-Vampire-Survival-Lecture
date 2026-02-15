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
        player = GetComponentInParent<Player>();
    }

    void Start()
    {
        WeaponInit();
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

        // test code
        if (Input.GetButtonDown("Jump"))
        {
            LevelUp(3.5f, 2);
            WeaponInit();
        }
    }

    public void LevelUp(float increaseDamage, int addCount)
    {
        damage += increaseDamage;
        count += addCount;
    }

    public void WeaponInit()
    {
        switch (weaponId)
        {
            case 0:
                rotateSpeed = 150f;
                WeaponPosition();
                break;
            default:
                rotateSpeed = 0.45f;
                break;
        }
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
        //객체 값이 있으면 true 없으면 false이기 때문에 null값인지 확인하기 위해서는 !를 붙여야 한다.
        if (!player.scanner.nearestTarget)
            return;

        Transform bullet = GameManager.instance.poolManager.GetObject(prefabId).transform;
        bullet.position = transform.position;
    }
}
