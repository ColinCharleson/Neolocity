using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health = 100f;


    public Animator enemyAttack;

    //enemy pathing

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    //Rigid Body

    public Rigidbody enemy;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public int damage = 1;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patrolling();
        }

        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
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
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {

        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    public void TakeDamage(float damageTaken)
    {
        health -= damageTaken;
        KockBack();


        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void AttackPlayer()
    {
        
        

        transform.LookAt(player);

        if (!alreadyAttacked)
        {

            enemyAttack.SetTrigger("Attacking");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

       
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemyAttack.SetTrigger("Attacking");

            Vector3 dmgDirection = collision.transform.position - transform.position;
            dmgDirection = dmgDirection.normalized;


            FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
        }
    }

    private void KockBack()
    {
        enemy.AddForce((transform.up * 3), ForceMode.Impulse);
        enemy.AddForce((-transform.forward * 60), ForceMode.Impulse);
    }
}
