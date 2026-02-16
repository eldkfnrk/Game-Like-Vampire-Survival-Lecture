using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;  //관통률

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void BulletInit(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }

    public void BulletInit(float damage, int per, Vector3 dir)
    {
        float bulletVelocity = 8f;
        this.damage = damage;
        this.per = per;
        rigid.linearVelocity = dir * bulletVelocity;
    }

    void DisableBullet()
    {
        Vector3 targetPos = GameManager.instance.player.transform.position;
        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance > 10f)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -1)  //per이 -1이라는 것은 무한 관통이라는 뜻이고 이는 근접 무기임을 의미
            return;

        per--;  //적을 맞추면서 관통력 하락

        if (per < 0)
        {
            rigid.linearVelocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Invoke("DisableBullet", 5f);
    }

    private void OnDisable()
    {
        if (IsInvoking("DisableBullet"))
        {
            CancelInvoke("DisableBullet");
        }
    }
}
