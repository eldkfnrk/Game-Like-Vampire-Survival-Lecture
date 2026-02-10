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

        //방법1
        //Vector3 playerPos = GameManager.instance.player.transform.position;
        //Vector3 myPos = transform.position;
        //float diffX = Mathf.Abs(playerPos.x - myPos.x);  //Abs - 절대값
        //float diffY = Mathf.Abs(playerPos.y - myPos.y);

        //Vector3 playerDir = GameManager.instance.player.inputVec;
        //float dirX = playerDir.x < 0 ? -1 : 1;  
        //float dirY = playerDir.y < 0 ? -1 : 1;

        //방법2
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX < 0 ? -1 : 1;
        dirY = dirY < 0 ? -1 : 1;

        Vector3 playerDir = GameManager.instance.player.inputVec;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 40);
                else if(diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);  
                break;
            case "Enemy":
                if (collider.enabled)  //적 캐릭터가 죽은 상태면 콜라이더를 비활성화할 예정
                {
                    transform.Translate(playerDir * 20 + new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                }
                break;
        }
    }
}
