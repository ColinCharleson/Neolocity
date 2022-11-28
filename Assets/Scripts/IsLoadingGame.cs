using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsLoadingGame : MonoBehaviour
{
	public static IsLoadingGame instance;
	public bool load;
	private void Awake()
	{
        if (!instance)
        {
            instance = this;
        }
		DontDestroyOnLoad(this.gameObject);
	}
	public void SetBoolTrue()
	{
		load = true;
	}
}
