using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager instance;

    public int activeEnemyCount;
    public int maxEnemies = 15;
    GameObject[] enemies;

    public Vector3 randomPosition;
    GameObject player;
    Vector3 playerPos;
    public GameObject groundEnemy;

    public float spawnRadius = 20;
    public float safeRadius = 10;
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
	private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(playerPos, spawnRadius);
        Gizmos.DrawSphere(playerPos, safeRadius);
    }
	void Update()
    {
        playerPos = player.transform.position;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemies.Length < maxEnemies)
		{
            EnemySpawn();
		}
    }

    void EnemySpawn()
	{
         randomPosition = new Vector3(Random.Range(playerPos.x - spawnRadius, playerPos.x + spawnRadius),
                                      Random.Range(playerPos.y - spawnRadius, playerPos.y + spawnRadius),
                                      Random.Range(playerPos.z - spawnRadius, playerPos.z + spawnRadius));

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPosition, out hit, 1.0f, NavMesh.AllAreas);

        if (Vector3.Distance(playerPos, hit.position) > safeRadius && hit.position.x != float.PositiveInfinity)
        {
            Instantiate(groundEnemy, hit.position, Quaternion.identity);
            activeEnemyCount += 1;
        }
		else
		{
            EnemySpawn();
		}
	}

}
