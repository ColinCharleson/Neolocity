using UnityEngine;

[CreateAssetMenu(fileName = "NPC Name", menuName = "NPC/NPC with Mission")]
public class DataNPC : ScriptableObject
{
	public string npcName;

	public int missionNumber;

	[TextArea(4, 5)]
	public string notReadyMessage;
	[TextArea(4, 5)]
	public string missionMessage;
	[TextArea(4, 5)]
	public string finishedMessage;
	[TextArea(4, 5)]
	public string keepGoingMessage;
	[TextArea(4, 5)]
	public string allDoneMessage;
}
