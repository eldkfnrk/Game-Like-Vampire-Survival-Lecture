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
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        scanner = GetComponent<Scanner>();
        handControls = GetComponentsInChildren<HandControl>(true);  // 인자 값으로 bool 타입 값을 넣을 수 있는데 이 매개 변수는 비활성화되어 있는 오브젝트도 포함할 것이냐를 선택하는 값이다.(true면 비활성화 오브젝트도 포함 false면 포함x)
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        anim.SetFloat("Speed", inputVec.magnitude);

        if (inputVec.x != 0)
        {
            spriteR.flipX = inputVec.x < 0;
        }
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
