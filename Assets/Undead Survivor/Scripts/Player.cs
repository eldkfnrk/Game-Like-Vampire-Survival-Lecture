using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float moveSpeed;

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.fixedDeltaTime;

        // 물리를 이용한 이동 방법 3가지
        // 1. 힘을 주는 방법
        //rigid.AddForce(inputVec);

        // 2. 속도를 조절하는 방법
        //rigid.linearVelocity = inputVec;

        // 3. 위치를 직접 조정하는 방법(이번 프로젝트에서 사용할 방법)
        //rigid.MovePosition(rigid.position + inputVec);
        
        rigid.MovePosition(rigid.position + nextVec);
    }
}
