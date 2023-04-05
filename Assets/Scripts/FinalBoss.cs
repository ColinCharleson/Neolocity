using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalBoss : MonoBehaviour
{
    private float health = 20;
    private float maxHealth;

    public Image healthBar;

	private void Awake()
	{
		maxHealth = health;
	}
	// Update is called once per frame
	public void TakeDamage(float damage)
    {
		Debug.Log("Boss hit! " + health) ;
        health -= damage; 
        
        //health
        float healthPercentage = health / maxHealth;
        healthBar.fillAmount = healthPercentage;
    }

	private void Update()
	{
		 if(health <= 0)
		{
			SceneManager.LoadScene("EndCutscene");
		}
	}
}
