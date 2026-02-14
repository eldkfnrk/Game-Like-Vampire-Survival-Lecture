using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float curHP;
    public float maxHP;

    public RuntimeAnimatorController[] animController;
    Rigidbody2D target;

    bool isLive;

    Rigidbody2D rigid;
    Animator animator;
    SpriteRenderer spriteR;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        curHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        curHP -= collision.GetComponent<Bullet>().damage;

        if (curHP > 0)
        {
            // alive, hit animation
            
        }
        else
        {
            // death, deate animation
            Death();
        }
    }

    public void EnemyInit(SpawnData data)
    {
        animator.runtimeAnimatorController = animController[data.spriteType];
        maxHP = data.hp;
        moveSpeed = data.moveSpeed;
        curHP = maxHP;
    }

    void Death()
    {
        isLive = false;
        gameObject.SetActive(false);
    }
}
