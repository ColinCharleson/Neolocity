using UnityEngine;

public class Scrap : MonoBehaviour
{
	private void Update()
	{
		if (this.transform.position.y < 0)
			this.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
	}
}
