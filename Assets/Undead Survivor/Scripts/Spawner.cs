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
