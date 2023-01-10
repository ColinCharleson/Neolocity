using UnityEngine.UI;
using UnityEngine;
public class Compass : MonoBehaviour
{
	public RawImage CompassImage;

	public RawImage WaypointImage;
	public Texture onLevel, offLevel;

	public Transform Player;
	public Transform Objective;

	float units;
	private void Start()
	{
		units = CompassImage.rectTransform.rect.width / 360;
	}
	public void FixedUpdate()
	{
		Objective = MissionManager.instance.currentObjective;

		CompassImage.uvRect = new Rect(Player.localEulerAngles.y / 360, 0, 1, 1);

		if (Objective == null)
		{
			WaypointImage.enabled = false;
		}
		else
		{
			WaypointImage.enabled = true;
			WaypointImage.rectTransform.anchoredPosition = GetItemPosition();


			if (MissionManager.instance.currentObjective.gameObject.tag == "NPC")
			{
				WaypointImage.color = new Color32(255, 192, 0, 255);
			}
			else// if (MissionManager.instance.currentObjective.gameObject.tag == "Mission End")
			{
				WaypointImage.color = new Color32(0, 255, 255, 255);
			}

			if (Player.position.y > Objective.position.y + 1) // too low
			{
				WaypointImage.texture = offLevel;
				WaypointImage.transform.rotation = Quaternion.Euler(0, 0, 180);
			}
			else if (Player.position.y < Objective.position.y - 1)// too high
			{
				WaypointImage.texture = offLevel;
				WaypointImage.transform.rotation = Quaternion.Euler(0, 0, 0);
			}
			else //just right
			{
				WaypointImage.texture = onLevel;
			}
		}
	}

	Vector2 GetItemPosition()
	{
		Vector2 playerPos = new Vector2(Player.transform.position.x, Player.transform.position.z);
		Vector2 playerForward = new Vector2(Player.transform.forward.x, Player.transform.forward.z);
		Vector2 objectivePos = new Vector2(Objective.position.x, Objective.position.z);

		float angle = Vector2.SignedAngle(objectivePos - playerPos, playerForward);

		return new Vector2(units * angle, 470f);
	}
}