using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    public int maxEnemies = 5;
    public int maxRangedEnemies = 2;
    GameObject[] enemies;
    GameObject[] rangedEnemies;

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

        if(enemies.Length < maxEnemies)
		{
            EnemySpawn(groundEnemy);
		}

        if(rangedEnemies.Length < maxRangedEnemies)
		{
            if (Random.Range(0, 2) > 0.5f)
            {
                EnemySpawn(rangedEnemy);
            }
			else
            {
                EnemySpawn(flyingRangedEnemy);
            }
		}

        EnemyDeSpawning();
    }
    void EnemySpawn(GameObject enemyType)
	{
         randomPosition = new Vector3(Random.Range(playerPos.x - spawnRadius, playerPos.x + spawnRadius),
                                      Random.Range(playerPos.y - spawnRadius, playerPos.y + spawnRadius),
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
	}
}
