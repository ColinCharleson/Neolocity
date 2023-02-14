using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Skillshop : MonoBehaviour
{
	public PlayerController player;
	public TextMeshProUGUI scrapText;
	public GameObject skillShop;

	public TextMeshProUGUI speedCost, damageCost, jumpCost, boostCost, healthCost;
	public TextMeshProUGUI speedLvl, damageLvl, jumpLvl, boostLvl, healthLvl;
	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}
	private void Update()
	{
	}
	public void CashUpdate()
	{
		scrapText.text = player.scrap.ToString() + " scrap";

		if (player.speedSP > 1)
		{
			speedCost.text = "Upgrade - " + 3 * (int)((player.speedSP - 0.9) * 10);
			speedLvl.text = "Level " + (int)((player.speedSP - 1) * 10);
		}
		if (player.damageSP > 1)
		{
			damageCost.text = "Upgrade - " + 3 * (int)((player.damageSP - 0.9) * 10);
			damageLvl.text = "Level " + (int)((player.damageSP - 1) * 10);
		}
		if (player.jumpSP > 1)
		{
			jumpCost.text = "Upgrade - " + 3 * (int)((player.jumpSP - 0.9) * 10);
			jumpLvl.text = "Level " + (int)((player.jumpSP - 1) * 10);
		}
		if (player.boostSP > 1)
		{
			boostCost.text = "Upgrade - " + 3 * (int)((player.boostSP - 0.9) * 10);
			boostLvl.text = "Level " + (int)((player.boostSP - 1) * 10);
		}
		if (player.healthSP > 1)
		{
			healthCost.text = "Upgrade - " + 3 * (int)((player.healthSP - 0.9) * 10);
			healthLvl.text = "Level " + (int)((player.healthSP - 1) * 10);
		}
	}
	public void UpgradeSpeed()
	{
		if (player.speedSP < 2 && (player.scrap + 0.7) >= (3 * ((player.speedSP - 0.9) * 10)))
		{
			player.speedSP += 0.1f;
			player.scrap -= 3 * (int)((player.speedSP - 1) * 10);
			speedCost.text = "Upgrade - " + 3 * (int)((player.speedSP - 0.9) * 10);
			speedLvl.text = "Level " + (int)((player.speedSP - 1) * 10);
			if (player.speedSP >= 2f)
			{
				speedCost.text = "Max Level";
			}
		}
	}
	public void UpgradeDamage()
	{
		if (player.damageSP < 2 && (player.scrap + 0.7) >= (3 * ((player.damageSP - 0.9) * 10)))
		{
			player.damageSP += 0.1f;
			player.scrap -= 3 * (int)((player.damageSP - 1) * 10);
			damageCost.text = "Upgrade - " + 3 * (int)((player.damageSP - 0.9) * 10);
			damageLvl.text = "Level " + (int)((player.damageSP - 1) * 10);
			if (player.damageSP >= 2f)
			{
				damageCost.text = "Max Level";
			}
		}
	}
	public void UpgradeJump()
	{
		if (player.jumpSP < 1.5f && (player.scrap + 0.7) >= (3 * ((player.jumpSP - 0.9) * 10)))
		{
			player.jumpSP += 0.1f;
			player.scrap -= 3 * (int)((player.jumpSP - 1) * 10);
			jumpCost.text = "Upgrade - " + 3 * (int)((player.jumpSP - 0.9) * 10);
			jumpLvl.text = "Level " + (int)((player.jumpSP - 1) * 10);
			if (player.jumpSP >= 1.5f)
			{
				jumpCost.text = "Max Level";
			}
		}
	}
	public void UpgradeBoost()
	{
		if (player.boostSP < 1.5 && (player.scrap + 0.7) >= (3 * ((player.boostSP - 0.9) * 10)))
		{
			player.boostSP += 0.1f;
			player.scrap -= 3 * (int)((player.boostSP - 1) * 10);
			boostCost.text = "Upgrade - " + 3 * (int)((player.boostSP - 0.9) * 10);
			boostLvl.text = "Level " + (int)((player.boostSP - 1) * 10);
			if (player.boostSP >= 1.5f)
			{
				boostCost.text = "Max Level";
			}
		}
	}
	public void UpgradeHealth()
	{
		Debug.Log((3 * ((player.healthSP - 0.9) * 10)));
		if (player.healthSP < 2 && (player.scrap + 0.7) >= (3 * ((player.healthSP - 0.9) * 10)))
		{
			player.healthSP += 0.1f;
			player.scrap -= 3 * (int)((player.healthSP - 1) * 10);
			healthCost.text = "Upgrade - " + 3 * (int)((player.healthSP - 0.9) * 10);
			healthLvl.text = "Level " + (int)((player.healthSP - 1) * 10);
			if (player.healthSP >= 2f)
			{
				healthCost.text = "Max Level";
			}
			player.GetComponent<PlayerHealth>().maxHealth = player.healthSP * 100;
		}
	}
}
