using UnityEngine;

[CreateAssetMenu(fileName = "Follow Mission", menuName = "Mission/Follow Mission")]
public class FollowMission : ScriptableObject
{
	public GameObject followTarget;

	public Vector3 startLocation;
	public Vector3 endLocation;

}
