using UnityEngine;
using UnityEngine.InputSystem;

public class Spawner : MonoBehaviour
{
    Transform[] spawnPoint;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();  //자기 자신도 해당 컴포넌트를 가지면 0번 인덱스에 저장 그 후 자식 컴포넌트를 순서대로 저장
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 1f)
        {
            Spawn();
            timer = 0f;
        }
    }

    void Spawn()
    {
        GameObject enemy = GameManager.instance.poolManager.GetObject(Random.Range(0, 5));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}
