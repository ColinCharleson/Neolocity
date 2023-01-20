using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KasaAttack : MonoBehaviour
{
	//Combat
	public Animator kasa;
	public bool isAttacking = false;
	public bool canAttack = true;
	public float attackCooldown = 0.5f;
	public float swingDamage = 1f;
	public float timeSinceLastHit;
	public float lastAttack;
	public bool attackLock = false;
	public float attackRegeneration;
	public float attackBar;

	//enemy damage cooldown
	public float timeSinceHit;
	public bool enemyTakeDmg = true;

	//public Material shieldMat;
	public GameObject uiSheild1, uiSheild2, uiSheild3, AttackUI;

	//Umbrella Cooldown UI 
	public GameObject umbrellaUI;

	//Blocking
	public bool canBlock = true;
	public bool isBlocking = false;
	public int blockHealth = 3;
	public float timeSinceBlockBroke;
	public float blockingSpeed;
	public float regenTimer;


	//Trail Effect
	public GameObject trail;

	private PlayerController movement;

	private void Start()
	{
		movement = GetComponent<PlayerController>();
	}
	void Update()
	{
		timeSinceLastHit += Time.deltaTime;
		timeSinceBlockBroke += Time.deltaTime;
		timeSinceHit += Time.deltaTime;
		kasa.SetBool("Blocking", isBlocking);
		if (Input.GetKeyDown(InputSystem.key.attack))
		{
			
			if (canAttack && !movement.onWall && !movement.gliding && timeSinceLastHit > 0.5f && movement.stamina >= 0 && !attackLock && !movement.sprintLock)
			{ 
				if (lastAttack == 3)
				{
					if (timeSinceLastHit >= 3)
						Attack();
				}
				else
					Attack();
			}

			if(movement.stamina <= 0)
            {
				attackLock = true;
				movement.stamina = 0;
			}

			if (movement.stamina >= 100)
			{
				attackLock = false;
			}
		}

		if(attackCooldown >= 3)
        {
			attackBar -= attackRegeneration * Time.deltaTime;
		}

		if (attackBar <= 0)
		{
			attackBar = 0;
		}

		if (Input.GetKey(InputSystem.key.block) && !movement.gliding && !movement.isSprinting && !movement.onWall)
		{
			Block();
			canAttack = false;
		}
		else
		{
			canBlock = true;
			isBlocking = false;
			canAttack = true;
		}

		if (timeSinceBlockBroke > 2f)
		{
			BlockRegen();
		}
	}


	public void Attack()
	{
		if (timeSinceLastHit > 1.3f)
		{
			trail.SetActive(true);
			kasa.SetTrigger("Attack");
			lastAttack = 1;
			attackCooldown = 0.5f;
			canAttack = false;
			isAttacking = true;
			timeSinceLastHit = 0;
			movement.stamina -= 10;
		}
		else
		{
			if (lastAttack == 2)
			{
				trail.SetActive(true);
				kasa.SetTrigger("Attack3");
				lastAttack = 3;
				attackCooldown = 4.0f;
				canAttack = false;
				isAttacking = true;
				timeSinceLastHit = 0;
				movement.stamina -= 30;

			}
			if (lastAttack == 1)
			{
				trail.SetActive(true);
				kasa.SetTrigger("Attack2");
				lastAttack = 2;
				attackCooldown = 0.5f;
				canAttack = false;
				isAttacking = true;
				timeSinceLastHit = 0;
				movement.stamina -= 20;
			}
		}
		StartCoroutine(AttackCooldownReset());
	}

	IEnumerator AttackCooldownReset()
	{
		StartCoroutine(ResetAttack());
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;
        AttackUI.SetActive(false);
		attackBar = 100;
	}

	IEnumerator ResetAttack()
	{
		yield return new WaitForSeconds(0.5f);
		isAttacking = false;
		trail.SetActive(false);
        AttackUI.SetActive(true);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy" && isAttacking && enemyTakeDmg)
		{
			other.GetComponent<EnemyAI>().TakeDamage(swingDamage);
			enemyTakeDmg = false;
		}

		if (other.gameObject.tag == "ProjectileEnemy" && isAttacking && enemyTakeDmg)
		{
			other.GetComponent<ProjectileEnemyAI>().TakeDamage(swingDamage);
			enemyTakeDmg = false;
		}

		StartCoroutine(CanAttackEnenmy());
	}

	IEnumerator CanAttackEnenmy()
	{
		yield return new WaitForSeconds(0.5f);
		enemyTakeDmg = true;

	}
	public void Block()
	{
		BlockUI();
		canBlock = false;
		isBlocking = true;
		blockingSpeed = 0.3f;

		if (blockHealth <= 0)
			isBlocking = false;

		timeSinceBlockBroke = 0;
	}
	void BlockUI()
	{
		switch (blockHealth)
		{
			case 3:
				uiSheild1.SetActive(true);
				uiSheild2.SetActive(true);
				uiSheild3.SetActive(true);
				break;

			case 2:
				uiSheild1.SetActive(false);
				uiSheild2.SetActive(true);
				uiSheild3.SetActive(true);
				break;

			case 1:
				uiSheild1.SetActive(false);
				uiSheild2.SetActive(false);
				uiSheild3.SetActive(true);
				break;

			case 0:
				uiSheild1.SetActive(false);
				uiSheild2.SetActive(false);
				uiSheild3.SetActive(false);
				break;
			default:
				break;
		}
	}
	void BlockRegen()
	{
		if (blockHealth < 3)    //less than 3 health (not full)
		{
			if (regenTimer > 0)		// if regen timer is more than 0
			{
				regenTimer -= Time.deltaTime;
			}
			if (regenTimer <= 0)  // if timer is at 0 (or less than)
			{
				blockHealth += 1;
				regenTimer = 1;
			}
		}
		BlockUI();
	}
}
