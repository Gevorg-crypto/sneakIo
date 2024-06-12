using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeasantFollow : MonoBehaviour
{
    public Transform target;
    public float followDistance = 0.5f;
    private Vector3 lastTargetPosition;
    private float targetSpeed;

    void Start()
    {
        if (target != null)
        {
            lastTargetPosition = target.position;
        }
    }

    void Update()
    {
        if (target != null)
        {
            targetSpeed = Vector3.Distance(target.position, lastTargetPosition) / Time.deltaTime;
            lastTargetPosition = target.position;

            float distance = Vector3.Distance(transform.position, target.position);
            if (distance > followDistance)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, target.position, targetSpeed * Time.deltaTime);
            }
        }
    }
}
