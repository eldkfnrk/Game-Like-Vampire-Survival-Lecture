using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float moveSpeed;

    public Scanner scanner;
    public HandControl[] handControls;

    Rigidbody2D rigid;
    SpriteRenderer spriteR;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        handControls = GetComponentsInChildren<HandControl>(true);  // 인자 값으로 bool 타입 값을 넣을 수 있는데 이 매개 변수는 비활성화되어 있는 오브젝트도 포함할 것이냐를 선택하는 값이다.(true면 비활성화 오브젝트도 포함 false면 포함x)
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        if (GameManager.instance.isGameStop)
            return;

        animator.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriteR.flipX = inputVec.x < 0;
        }
    }

    void FixedUpdate()
    {
        if (GameManager.instance.isGameStop)
            return;

        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.instance.isGameStop)
            return;

        if (collision.collider.CompareTag("Enemy"))
        {
            GameManager.instance.HP -= Time.deltaTime * 10f;  // 10f는 임시 값이고 실제로는 적이 주는 데미지 값이 들어갈 예정
        }

        if(GameManager.instance.HP <= 0)
        {
            for(int i = 2; i < transform.childCount; i++) {
                transform.GetChild(i).gameObject.SetActive(false);
            }

            animator.SetTrigger("Dead");
            spriteR.sortingOrder = 3;

            GameManager.instance.resultUI
        }
    }
}
