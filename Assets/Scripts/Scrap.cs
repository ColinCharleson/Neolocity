using UnityEngine;

public class Scrap : MonoBehaviour
{
	float timeSpawned;
	float despawnTime = 60;

	
	private void Update()
	{
		if (this.transform.position.y < 0)
			this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);

		if(despawnTime > timeSpawned)
		{
			timeSpawned += Time.deltaTime;
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
