using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriteR;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        spriteR.flipX = target.position.x < rigid.position.x;
    }

    private void FixedUpdate()
    {
        Vector2 dir = target.position - rigid.position;
        Vector2 moveVec = dir.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);
    }
}
