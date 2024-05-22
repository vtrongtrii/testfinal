using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFlip : MonoBehaviour
{
    public void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
