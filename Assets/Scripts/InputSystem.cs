using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputSystem : MonoBehaviour
{
	public static InputSystem key;

	public KeyCode sprint = KeyCode.LeftShift;
	public KeyCode jump = KeyCode.Space;
	public KeyCode boost = KeyCode.F;
	public KeyCode glide = KeyCode.LeftControl;
	public KeyCode interact = KeyCode.E;
	public KeyCode attack = KeyCode.Mouse0;
	public KeyCode block = KeyCode.Mouse1;
	public KeyCode cancelTalk = KeyCode.Q;
	public KeyCode pause = KeyCode.Escape;

	public TextMeshProUGUI grabText;
	public TextMeshProUGUI speakText;
	public TextMeshProUGUI leaveText;

	bool lookingForInput;
	string selectedKey = null;
	TextMeshProUGUI selectedText = null;
    public void Awake()
    {
        if (!key)
        {
            key = this;
        }
		else
		{
			Destroy(gameObject);
		}
    }
    private void Start()
	{
		//   DontDestroyOnLoad(this.gameObject);
		foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
		{
			if(kc.ToString() == PlayerPrefs.GetString("SprintKey"))
			sprint = kc;
			if(kc.ToString() == PlayerPrefs.GetString("JumpKey"))
			jump = kc;
			if(kc.ToString() == PlayerPrefs.GetString("BoostKey"))
			boost = kc;
			if(kc.ToString() == PlayerPrefs.GetString("GlideKey"))
			glide = kc;
			if (kc.ToString() == PlayerPrefs.GetString("InteractKey"))
			{
				interact = kc;
				grabText.text = "Press " + kc.ToString() + " To Grab";
				speakText.text = "Press " + kc.ToString() + " To Speak";
			}
			if (kc.ToString() == PlayerPrefs.GetString("AttackKey"))
			attack = kc;
			if(kc.ToString() == PlayerPrefs.GetString("BlockKey"))
			block = kc;
			if (kc.ToString() == PlayerPrefs.GetString("CancelTalkKey"))
			{
				cancelTalk = kc;
				leaveText.text = "Press " + kc.ToString() + " To Continue";
			}
			if (kc.ToString() == PlayerPrefs.GetString("PauseKey"))
			pause = kc;
		}
	}
	private void Update()
	{
		if (lookingForInput)
			ChangeKey();
	}
	private void ChangeKey()
	{
		if (Input.anyKeyDown)
		{
			foreach(KeyCode kc in Enum.GetValues(typeof(KeyCode)))
			{
				if(Input.GetKeyDown(kc))
				{
					selectedText.text = kc.ToString();
					switch (selectedKey)
					{
						case "sprint":
							sprint = kc;
							PlayerPrefs.SetString("SprintKey", kc.ToString());
							break;
						case "jump":
							jump = kc;
							PlayerPrefs.SetString("JumpKey", kc.ToString());
							break;
						case "boost":
							boost = kc;
							PlayerPrefs.SetString("BoostKey", kc.ToString());
							break;
						case "glide":
							glide = kc;
							PlayerPrefs.SetString("GlideKey", kc.ToString());
							break;
						case "interact":
							interact = kc;
							PlayerPrefs.SetString("InteractKey", kc.ToString());
							grabText.text = "Press " + kc.ToString() + " To Grab";
							speakText.text = "Press " + kc.ToString() + " To Speak";
							break;
						case "attack":
							attack = kc;
							PlayerPrefs.SetString("AttackKey", kc.ToString());
							break;
						case "block":
							block = kc;
							PlayerPrefs.SetString("BlockKey", kc.ToString());
							break;
						case "cancelTalk":
							cancelTalk = kc;
							PlayerPrefs.SetString("CancelTalkKey", kc.ToString());
							leaveText.text = "Press " + kc.ToString() + " To Continue";
							break;
						case "pause":
							pause = kc;
							PlayerPrefs.SetString("PauseKey", kc.ToString());
							break;
						default:
							break;
					}
					lookingForInput = false;
				}
			}
		}
	}
	public void DetectInput(string key)
	{
		selectedKey = key;
		lookingForInput = true;
	}

	public void ChangeText(TextMeshProUGUI text)
	{
		selectedText = text;
	}
	public void SetTexts()
	{
		GameObject.Find("SprintChangeText").GetComponent<TextMeshProUGUI>().text = sprint.ToString();
		GameObject.Find("JumpChangeText").GetComponent<TextMeshProUGUI>().text = jump.ToString();
		GameObject.Find("BoostChangeText").GetComponent<TextMeshProUGUI>().text = boost.ToString();
		GameObject.Find("GlideChangeText").GetComponent<TextMeshProUGUI>().text = glide.ToString();
		GameObject.Find("InteractChangeText").GetComponent<TextMeshProUGUI>().text = interact.ToString();
		GameObject.Find("AttackChangeText").GetComponent<TextMeshProUGUI>().text = attack.ToString();
		GameObject.Find("BlockChangeText").GetComponent<TextMeshProUGUI>().text = block.ToString();
		GameObject.Find("CancelTalkChangeText").GetComponent<TextMeshProUGUI>().text = cancelTalk.ToString();
		GameObject.Find("PauseChangeText").GetComponent<TextMeshProUGUI>().text = pause.ToString();
	}
}