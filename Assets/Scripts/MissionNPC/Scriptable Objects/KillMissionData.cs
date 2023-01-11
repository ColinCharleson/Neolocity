using UnityEngine;

[CreateAssetMenu(fileName = "Kill Mission", menuName = "Mission/Kill Mission")]
public class KillMissionData : ScriptableObject
{
	public Vector3 spawnArea;

	public GameObject battlePrefab;
}
