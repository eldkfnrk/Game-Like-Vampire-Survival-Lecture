using System.Collections;
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
    WaitForFixedUpdate wait;  //WaitForFixedUpdate - 한 물리 업데이트를 넘긴다는 의미

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    public void EnemyInit(SpawnData data)
    {
        animator.runtimeAnimatorController = animController[data.spriteType];
        maxHP = data.hp;
        moveSpeed = data.moveSpeed;
        curHP = maxHP;
    }

    private void LateUpdate()
    {
        spriteR.flipX = target.position.x < rigid.position.x;
    }

    private void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

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
        StartCoroutine(KnockBack());

        if (curHP > 0)
        {
            // alive, hit animation
            animator.SetTrigger("Hit");
        }
        else
        {
            // death, deate animation
            Death();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 direction = transform.position - playerPos;
        rigid.AddForce(direction.normalized * 0.5f, ForceMode2D.Impulse);
    }

    void Death()
    {
        isLive = false;
        gameObject.SetActive(false);
    }
}
