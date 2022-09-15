using UnityEngine;

[CreateAssetMenu(fileName = "NPC Name", menuName = "NPC/New NPC")]
public class DataNPC : ScriptableObject
{
	public string npcName;
	public int missionNumber;

	[TextArea(4, 5)]
	public string notReadyMessage;
	[TextArea(4, 5)]
	public string missionMessage;
}
