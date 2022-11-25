using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public ParticleSystem yellowParticles, explodeParticles, lightParticles;
   
    //enemy health
    public float health;
    public float maxHealth;
    public float healthRegeneration;

    public Animator enemyAnims;

    //UI
    public Slider slider;
    public GameObject healthBarUi;

    //Block
    public KasaAttack kasaAttack;
    public PlayerController playerController;

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
    public int damage = 50;
    bool isAlive = true;
    public float deathtimer = 0;


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isAlive == true)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange)
            {
                health += healthRegeneration * Time.deltaTime;
                slider.value = HealthUi();

                Patrolling();

                if (health > maxHealth)
                {
                    health = 5;
                }
            }

            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }

            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
        }
    }


    private void Patrolling()
    {
        if (isAlive == true)
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
    }


    void SearchWalkPoint()
    {
        if (isAlive == true)
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
    }

    private void ChasePlayer()
    {
        if (isAlive == true)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            agent.SetDestination(player.position);
        }
    }



    public void TakeDamage(float damageTaken)
    {
        if (isAlive == true)
        {
            yellowParticles.Play();
            explodeParticles.Play();
            lightParticles.Play();
            health -= damageTaken;
            KnockBack();
            slider.value = HealthUi();

            if (health > maxHealth)
            {
                health = 5;
            }
            if (health < maxHealth)
            {
                healthBarUi.SetActive(true);
            }
            if (health >= maxHealth)
            {
                healthBarUi.SetActive(false);
            }
            if (health <= 0)
            {
                isAlive = false;
                GetComponent<CapsuleCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                enemyAnims.SetTrigger("Die");
                explodeParticles.Play();
                lightParticles.Play();

                Destroy(gameObject, 2);
            }
        }
    }

    float HealthUi()
    {
            return health / maxHealth;
    }

    private void AttackPlayer()
    {
        if (isAlive == true)
        {
            transform.LookAt(player);

            if (!alreadyAttacked)
            {

                enemyAnims.SetTrigger("Attacking");
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
       
    }
    private void ResetAttack()
    {
        if (isAlive == true)
        {
            alreadyAttacked = false;
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (isAlive == true)
        {
            if (collision.gameObject.tag == "Player" && !kasaAttack.isBlocking)
            {
                enemyAnims.SetTrigger("Attacking");

                Vector3 dmgDirection = collision.transform.position - transform.position;
                dmgDirection = dmgDirection.normalized;

                FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
            }
            if (collision.gameObject.tag == "Player" && kasaAttack.isBlocking)
            {
                yellowParticles.Play();
                kasaAttack.blockHealth -= 1;
                KnockBack();
            }
        }
    }

    private void KnockBack()
    {
        if (isAlive == true)
        {
            enemy.AddForce((transform.up * 3), ForceMode.Impulse);
            enemy.AddForce((-transform.forward * 60), ForceMode.Impulse);
        }
    }
}
