using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowBot : MonoBehaviour
{
    public NavMeshAgent agent;
    public Vector3 target;
    void FixedUpdate()
    {
        agent.SetDestination(target);

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
                Destroy(this.gameObject);
        }
    }
}
