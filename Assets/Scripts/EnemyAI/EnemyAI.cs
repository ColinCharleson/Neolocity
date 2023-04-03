using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    public SoundManager soundManager;
    public GameObject SoundManagerObject;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public ParticleSystem yellowParticles, explodeParticles, lightParticles, smokeDamaged, deadSmoke, explode, smokeExplode, sparks;

    //enemy health
    public float health;
    public float maxHealth;
    public float healthRegeneration;

    public Animator enemyAnims;

    public GameObject scrap;
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

    //Attacking
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public int damage = 50;
    bool isAlive = true;
    public float deathtimer = 0;
    public AudioSource attackSource;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        kasaAttack = player.GetComponent<KasaAttack>();
        playerController = player.GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;

        SoundManagerObject = GameObject.Find("SoundManager");
        soundManager = SoundManagerObject.GetComponent<SoundManager>();
        soundManager.chompAttackSource.enabled = true;
    }

    private void FixedUpdate()
    {
        if (isAlive == true)
        {
            if (agent.velocity.x == 0.0 || agent.velocity.z == 0.0)
            {
                enemyAnims.SetBool("IsMoving", false);
            }
            else
            {
                enemyAnims.SetBool("IsMoving", true);
            }

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

                    if (!playerInSightRange && playerInAttackRange)
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

                    if (!playerInSightRange && playerInAttackRange)
                    {
                        enemyState = EnemyState.attack;
                    }

                    break;

                case EnemyState.attack:
                    AttackPlayer();
                    //soundManager.chompAttackSource.Play();
                    SoundManager.Instance.ChompPlay();
                    if (!playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.patrol;
                    }
                    else if (playerInSightRange && !playerInAttackRange)
                    {
                        enemyState = EnemyState.chase;
                    }

                    break;

                default:

                    break;
            }
        }
        //soundManager.chompAttackSource.enabled = false;
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
            if (health < 2.5)
            {
                smokeDamaged.Play();
            }
            if (health <= 0)
            {
                isAlive = false;
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<NavMeshAgent>().enabled = false;
                enemyAnims.SetTrigger("Die");
                explodeParticles.Play();
                lightParticles.Play();

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
        if (isAlive == true)
        {
            transform.LookAt(player);
            
        }   
    }
    IEnumerator ResetAttack()
    {
        if (isAlive == true)
        {
            yield return new WaitForSeconds(0.5f);
            alreadyAttacked = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (isAlive == true)
        {
            if (collision.gameObject.tag == "Player" && !kasaAttack.isBlocking && !alreadyAttacked)
            {
                enemyAnims.SetTrigger("Attacking");
                SoundManager.Instance.ChompPlay();

                Vector3 dmgDirection = collision.transform.position - transform.position;
                dmgDirection = dmgDirection.normalized;

                FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
                alreadyAttacked = true;
                StartCoroutine(ResetAttack());
            }
            if (collision.gameObject.tag == "Player" && kasaAttack.isBlocking)
            {
                enemyAnims.SetTrigger("Attacking");
                yellowParticles.Play();
                kasaAttack.blockHealth -= 1;
                KnockBack();
                SoundManager.Instance.ChompPlay();
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

    public enum EnemyState
    {
        chase,
        attack,
        patrol
    }
}
