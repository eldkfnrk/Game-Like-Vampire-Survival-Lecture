using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector2 inputVec;
    public float moveSpeed;

    Rigidbody2D rigid;
    SpriteRenderer spriteR;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");
    }

    private void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            spriteR.flipX = inputVec.x < 0;
        }
    }

    void FixedUpdate()
    {
        //Vector2 nextVec = inputVec.normalized * moveSpeed * Time.fixedDeltaTime;
        Vector2 nextVec = inputVec * moveSpeed * Time.fixedDeltaTime;  //InputSystem에서 normalized를 하도록 설정했기 때문에 normalized를 하지 않아도 된다. 

        rigid.MovePosition(rigid.position + nextVec);
    }

    void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    }
}
