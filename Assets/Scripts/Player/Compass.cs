using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
	public RawImage CompassImage;
	public Transform Player;

	public void Update()
	{
		CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);
	}
}