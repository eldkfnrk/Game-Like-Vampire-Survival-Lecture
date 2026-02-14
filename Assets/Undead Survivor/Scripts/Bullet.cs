using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int per;  //°üÅë·ü

    public void BulletInit(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
