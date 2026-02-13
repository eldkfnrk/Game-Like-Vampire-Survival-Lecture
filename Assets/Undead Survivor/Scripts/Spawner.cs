using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoint;
    public SpawnData[] spawnDatas;

    float timer;
    int level;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  //자기 자신도 해당 컴포넌트를 가지면 0번 인덱스에 저장 그 후 자식 컴포넌트를 순서대로 저장
    }

    void Update()
    {
        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnDatas.Length - 1);
        //Mathf.CeilToInt - 실수형 데이터의 소수점 부분을 올림 처리하고 정수형 데이터 값으로 변환해주는 Mathf 함수
        //Mathf.FloorToInt - 실수형 데이터의 소수점 부분을 버림 처리하고 정수형 데이터 값으로 변환해주는 Mathf 함수 
        //Mathf.Min - 첫 번째 인자 값이 두 번째 인자 값보다 커지면 두 번째 인자 값을 반환하고 아니면 첫 번째 인자 값을 반환하는 Mathf 함수
        //위에서 Mathf.Min 함수를 사용한 이유는 무한으로 커지는 시간 값으로 인한 인덱스 오류가 발생할 수 있는 것을 방지하기 위해서이다.

        //if (timer > 1f)
        //{
        //    Spawn();
        //    timer = 0f;
        //}

        if (timer > spawnDatas[level].spawnTime)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolManager.GetObject(0);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<Enemy>().EnemyInit(spawnDatas[level]);
    }
}

//클래스 같이 복잡한 구조를 사용하는 데이터 값을 바로 인스펙터 창으로 가져올 수는 없다. 그래서 [System.Serializable]라는 속성을 선언하여 직렬화를 시켜야 인스펙터 창에서 이 클래스 등의 정보를 수정할 수 있다.
[System.Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int hp;
    public float moveSpeed;
}
