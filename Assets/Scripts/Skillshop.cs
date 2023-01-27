using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Skillshop : MonoBehaviour
{
    private PlayerController player;
	public TextMeshProUGUI cash;
	public GameObject skillShop;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	private void Update()
	{
	}
	public void CashUpdate()
	{
		cash.text = "$" + player.cash.ToString();
	}
	public void UpgradeSpeed()
	{
		if (player.speedSP < 2 && player.cash > 100)
		{
			player.speedSP += 0.1f;
			player.cash -= 100;
		}
	}
	public void UpgradeDamage()
	{
		if (player.damageSP < 2 && player.cash > 100)
		{
			player.damageSP += 0.1f;
			player.cash -= 100;
		}
	}
	public void UpgradeJump()
	{
		if (player.jumpSP < 1.5f && player.cash > 100)
		{
			player.jumpSP += 0.1f;
			player.cash -= 100;
		}
	}
	public void UpgradeBoost()
	{
		if (player.boostSP < 1.5 && player.cash > 100)
		{
			player.boostSP += 0.1f;
			player.cash -= 100;
		}
	}
	public void UpgradeHealth()
	{
		if (player.healthSP < 2 && player.cash > 100)
		{
			player.healthSP += 0.1f;
			player.cash -= 100;
			player.GetComponent<PlayerHealth>().maxHealth = player.healthSP *100;
		}
	}
}
