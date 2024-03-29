using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    public int maxEnemies = 3;
    public int maxRangedEnemies = 2;
    public int maxFlyingEnemies = 2;
    GameObject[] enemies;
    GameObject[] rangedEnemies;
    GameObject[] flyingEnemies;

    public Vector3 randomPosition;
    GameObject player;
    Vector3 playerPos;

    public GameObject groundEnemy;
    public GameObject rangedEnemy;
    public GameObject flyingRangedEnemy;

    public float spawnRadius = 20;
    public float safeRadius = 10;
    public float deSpawnRadius = 50;
    private void Awake()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
	void Update()
    {
        playerPos = player.transform.position;

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        rangedEnemies = GameObject.FindGameObjectsWithTag("ProjectileEnemy");
        flyingEnemies = GameObject.FindGameObjectsWithTag("FlyingProjectileEnemy");

        if(enemies.Length < maxEnemies)
		{
            EnemySpawn(groundEnemy);
		}

        if(rangedEnemies.Length < maxRangedEnemies)
		{
            EnemySpawn(rangedEnemy);
		}
        
        if(flyingEnemies.Length < maxFlyingEnemies)
		{
            EnemySpawn(flyingRangedEnemy);
		}

        EnemyDeSpawning();
    }
    void EnemySpawn(GameObject enemyType)
	{
         randomPosition = new Vector3(Random.Range(playerPos.x - spawnRadius, playerPos.x + spawnRadius),
                                      Random.Range(0, 30),
                                      Random.Range(playerPos.z - spawnRadius, playerPos.z + spawnRadius));

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas);

        if (Vector3.Distance(playerPos, hit.position) > safeRadius && hit.position.x != float.PositiveInfinity)
        {
            Instantiate(enemyType, hit.position, Quaternion.identity);
        }
		else
		{
            EnemySpawn(enemyType);
		}
	}
    void EnemyDeSpawning()
	{
		foreach (GameObject enemy in enemies)
		{
            if(Vector3.Distance(playerPos, enemy.transform.position ) > deSpawnRadius)
			{
                if(enemy.layer == 9)
				{

				}
				else
				{
                    Destroy(enemy);
				}
			}
		}
        foreach (GameObject enemy in rangedEnemies)
		{
            if(Vector3.Distance(playerPos, enemy.transform.position ) > deSpawnRadius)
			{
                if(enemy.layer == 9)
				{

				}
				else
				{
                    Destroy(enemy);
				}
			}
		}
        foreach (GameObject enemy in flyingEnemies)
		{
            if(Vector3.Distance(playerPos, enemy.transform.position ) > deSpawnRadius)
			{
                if(enemy.layer == 9)
				{
                   
				}
				else
				{
                    Destroy(enemy);
				}
			}
		}
	}
}
