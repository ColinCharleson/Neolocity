using UnityEngine; 
[CreateAssetMenu(fileName = "Hack Mission", menuName = "Mission/Hack Mission")]
public class HackMission : ScriptableObject
{
	public GameObject hackTarget;

	public Vector3 startLocation;
	public Vector3 endLocation;

	public float hackingTime;
}
