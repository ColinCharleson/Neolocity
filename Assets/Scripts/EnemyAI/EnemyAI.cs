using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask WhatIsGround, WhatIsPlayer;


    //enemy pathing

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange;
    public bool playerInSightRange;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);

        if (!playerInSightRange)
        {
            Patrolling();
        }

        else if (playerInSightRange)
        {
            ChasePlayer();
        }
    }


    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceTowalkPoint = transform.position - walkPoint;

        //if walkpoint reached
        if (distanceTowalkPoint.magnitude < 1f)
            walkPointSet = false;
    }


    void SearchWalkPoint()
    {
        //caculating random point in range
        float randZ = Random.Range(-walkPointRange, walkPointRange);
        float randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        //Make sure enemy dosnt walk off map using raycast
        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }


}
