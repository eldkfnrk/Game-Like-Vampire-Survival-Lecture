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
    Collider2D coll;
    WaitForFixedUpdate wait;  //WaitForFixedUpdate - 한 물리 업데이트를 넘긴다는 의미

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteR = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
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
        if (GameManager.instance.isGameStop)
            return;

        spriteR.flipX = target.position.x < rigid.position.x;
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isGameStop)
            return;

        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        Vector2 dir = target.position - rigid.position;
        Vector2 moveVec = dir.normalized * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + moveVec);
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriteR.sortingOrder = 2;
        animator.SetBool("Dead", false);
        curHP = maxHP;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
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
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriteR.sortingOrder = 1;
            animator.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp();
            //Death();  //애니메이션 이벤트로 직접 하는 것이 아닌 애니메이션이 오브젝트 비활성화를 수행
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
        gameObject.SetActive(false);
    }
}
