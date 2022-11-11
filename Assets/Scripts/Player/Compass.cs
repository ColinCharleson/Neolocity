using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
	public RawImage CompassImage;
	public RawImage WaypointImage;
	public Transform Player;
	public Transform Objective;

	public void Update()
	{
		CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

		float angle = Vector3.Angle(Player.forward, Objective.position);

		if (Objective)
			WaypointImage.uvRect = new Rect(angle / 360, 0, 1, 1);

		Debug.Log(angle);
	}
}