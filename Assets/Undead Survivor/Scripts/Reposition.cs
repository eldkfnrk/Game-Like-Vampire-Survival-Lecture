using UnityEngine;

public class Reposition : MonoBehaviour
{
    Collider2D collider;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        switch (transform.tag)
        {
            case "Ground":
                float dirX = playerPos.x - myPos.x;
                float dirY = playerPos.y - myPos.y;

                float diffX = Mathf.Abs(dirX) - 10;
                float diffY = Mathf.Abs(dirY);

                dirX = dirX < 0 ? -1 : 1;
                dirY = dirY < 0 ? -1 : 1;

                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 60);
                else if(diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);  
                break;
            case "Enemy":
                if (collider.enabled)  //적 캐릭터가 죽은 상태면 콜라이더를 비활성화할 예정
                {
                    Vector3 distance = playerPos - myPos;  // 플레이어와 몬스터 오브젝트 간의 거리
                    Vector3 randomVec = new Vector3(Random.Range(-3f, 4f), Random.Range(-3f, 4f), 0);  // 위치의 랜덤 부여
                    // 영역에서 벗어난 몬스터와 플레이어 간 거리를 구하고 이 거리에 곱하기 2를 하여 반대로 보낸다. 거기다가 위치의 랜덤 값을 더해주면 몬스터가 랜덤하게 플레이어의 앞에서 나타나 게임의 긴장감을 줄 수 있다.
                    transform.Translate(distance * 2 + randomVec);
                }
                break;
        }
    }
}
