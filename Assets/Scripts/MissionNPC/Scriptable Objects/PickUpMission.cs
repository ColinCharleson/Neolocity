using UnityEngine;

[CreateAssetMenu(fileName = "Pick Up Mission", menuName = "Mission/Pick Up Mission")]
public class PickUpMission : ScriptableObject
{
	public GameObject missionPickUp;
	public GameObject pickUpItem;
	public Vector3 pickUpLocation;


}
