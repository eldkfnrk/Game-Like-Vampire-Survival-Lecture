using UnityEngine;

public class Scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targetsInRange;
    public Transform nearestTarget;

    private void FixedUpdate()
    {
        targetsInRange = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0f, targetLayer);
        nearestTarget = GetNearestTarget();
    }

    Transform GetNearestTarget()
    {
        Transform result = null;
        float minDistance = 100f;

        foreach(RaycastHit2D target in targetsInRange)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDistance = Vector3.Distance(myPos, targetPos);

            if(curDistance < minDistance)
            {
                minDistance = curDistance;
                result = target.transform;
            }
        }

        return result;
    }
}
