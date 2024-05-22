using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLand : MonoBehaviour
{
    public Transform platfrom;
    public Transform starPoint;
    public Transform endPoint;
    public float speed=1.5f;
    int direction = 1;



    private void Update()
    {
        Vector2 target = currentMovementTarget();

        platfrom.position = Vector2.MoveTowards(platfrom.position, target, speed*Time.deltaTime);

        float distance = (target-(Vector2)platfrom.position).magnitude; 

        if(distance <= 0.1f)
        {
            direction*= -1;
        }
    }



    Vector2 currentMovementTarget()
    {
        if (direction == 1)
        {
            return starPoint.position;
        }
        else
        {
            return endPoint.position;
        }

    }
    private void OnDrawGizmos()
    {
        if(platfrom != null && starPoint!=null && endPoint!= null)
        {
            Gizmos.DrawLine(platfrom.transform.position, starPoint.position);
            Gizmos.DrawLine(platfrom.transform.position, endPoint.position);
        }
    }
}
