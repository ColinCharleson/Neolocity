using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class FlyingProjectileEnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public ParticleSystem yellowParticles, explodeParticles, lightParticles, chargeLaser1, chargeLaser2, chargeShoot, smokeDamaged, deadSmoke, explode, smokeExplode, sparks;

    //enemy health
    public float health;
    public float maxHealth;
    public float healthRegeneration;

    public GameObject scrap;

    public Animator enemyAnims;

    //UI
    public Slider slider;
    public GameObject healthBarUi;

    //Block
    public KasaAttack kasaAttack;
    public PlayerController playerController;

    //enemy pathing
    private EnemyState enemyState = EnemyState.patrol;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Rigid Body
    public Rigidbody enemy;
    public GameObject laser;
    public Transform laserPos;

    //Attacking
    bool alreadyAttacked;
    bool isAlive = true;
    public float deathtimer = 0;
    public float chargebeam = 0;
    public bool charged = false;

    //projectile

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        kasaAttack = player.GetComponent<KasaAttack>();
        playerController = player.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {

            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            switch (enemyState)
            {
                case EnemyState.patrol:
                    health += healthRegeneration * Time.deltaTime;
                    slider.value = HealthUi();
                    Patrolling();

                    if (health > maxHealth)
                    {
                        health = 5;
                    }

                    if (health > 3 || health < 0)
                    {
                        smokeDamaged.Stop();
                    }

                    if (playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.chase;
                    }

                    if (playerInSightRange && playerInAttackRange)
                    {
                        enemyState = EnemyState.attack;
                    }
                    break;

                case EnemyState.chase:
                    ChasePlayer();
                    if (!playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.patrol;
                    }

                    if (playerInSightRange && playerInAttackRange)
                    {
                        enemyState = EnemyState.attack;
                    }

                    break;

                case EnemyState.attack:
                    AttackPlayer();

                    if (!playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.patrol;
                        chargebeam = 0;
                    }
                    else if (playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.chase;
                        chargebeam = 0;
                    }

                    break;

                default:

                    break;
            }
        }

    }


    private void Patrolling()
    {
        if (isAlive)
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
        if (isAlive)
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
        if (isAlive)
        {
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
            agent.SetDestination(player.position);
        }
    }


    public void TakeDamage(float damageTaken)
    {
        if (isAlive)
        {
            yellowParticles.Play();
            explodeParticles.Play();
            lightParticles.Play();
            health -= damageTaken;
            KnockBack();
            slider.value = HealthUi();
            SoundManager.Instance.AttackHitPlay();

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
            if (health < 2.5)
            {
                smokeDamaged.Play();
            }
            if (health <= 0)
            {
                isAlive = false;
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;


                Instantiate(scrap, this.transform.position, Quaternion.identity);
                explode.Play();
                smokeExplode.Play();
                sparks.Play();
                deadSmoke.Play();
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
        if (isAlive)
        {
            transform.LookAt(player);

            if (!alreadyAttacked && charged == false)
            {
                chargebeam += Time.deltaTime;
                chargeLaser1.Play();
                chargeLaser2.Play();

                if (chargebeam >= 3)
                {
                    Rigidbody rb = Instantiate(laser, laserPos.position, Quaternion.identity).GetComponent<Rigidbody>();
                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    rb.AddForce(transform.up * 2f, ForceMode.Impulse);
                    chargeShoot.Play();
                    charged = true;
                    alreadyAttacked = true;
                    StartCoroutine(ResetAttack());
                }
            }
        }
    }




    IEnumerator ResetAttack()
    {
        if (isAlive)
        {
            yield return new WaitForSeconds(5f);
            alreadyAttacked = false;
            chargebeam = 0;
            charged = false;
        }
    }
    private void KnockBack()
    {
        if (isAlive)
        {
            enemy.AddForce((transform.up * 3), ForceMode.Impulse);
            enemy.AddForce((-transform.forward * 60), ForceMode.Impulse);
        }
    }

    public enum EnemyState
    {
        chase,
        attack,
        patrol
    }
}

