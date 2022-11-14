using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public Material shieldMat;

	//Blocking
	public bool canBlock = true;
	public bool isBlocking = false;
	public int blockHealth = 3;
	public float timeSinceBlockBroke;
	public float blockingSpeed;

	private PlayerController movement;

	private void Start()
	{
		movement = GetComponent<PlayerController>();
	}
	void Update()
	{
		timeSinceLastHit += Time.deltaTime;
		timeSinceBlockBroke += Time.deltaTime;
		kasa.SetBool("Blocking", isBlocking);

		if (Input.GetMouseButtonDown(0))
		{
			if (canAttack && !movement.onWall && !movement.gliding)
			{
				if (lastAttack == 3)
				{
					if (timeSinceLastHit >= 3)
						Attack();
				}
				else
					Attack();
			}
		}

		if (Input.GetKey(KeyCode.Mouse1) && !movement.gliding)
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
	}


	public void Attack()
	{

		if (timeSinceLastHit > 1.3f)
		{
			kasa.SetTrigger("Attack");
			lastAttack = 1;
			attackCooldown = 0.5f;
			canAttack = false;
			isAttacking = true;
			timeSinceLastHit = 0;
		}
		else
		{
			if (lastAttack == 2)
			{
				kasa.SetTrigger("Attack3");
				lastAttack = 3;
				attackCooldown = 4.0f;
				canAttack = false;
				isAttacking = true;
				timeSinceLastHit = 0;
			}
			if (lastAttack == 1)
			{
				kasa.SetTrigger("Attack2");
				lastAttack = 2;
				attackCooldown = 0.5f;
				canAttack = false;
				isAttacking = true;
				timeSinceLastHit = 0;
			}
		}

		StartCoroutine(AttackCooldownReset());
	}

	IEnumerator AttackCooldownReset()
	{
		StartCoroutine(ResetAttack());
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;
	}

	IEnumerator ResetAttack()
	{

		yield return new WaitForSeconds(0.5f);
		isAttacking = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy" && isAttacking)
		{
				other.GetComponent<EnemyAI>().TakeDamage(swingDamage);
		}
	}
	public void Block()
	{
		canBlock = false;
		isBlocking = true;
		blockingSpeed = 0.3f;


		if (blockHealth == 3)
		{
			shieldMat.color = Color.magenta;
			Color color = shieldMat.color;
			color.a = 0.8f;
			shieldMat.color = color;

		}
		else if (blockHealth == 2)
        {
			shieldMat.color = Color.yellow;
			Color color = shieldMat.color;
			color.a = 0.8f;
			shieldMat.color = color;
		}
		else if (blockHealth == 1)
		{
			shieldMat.color = Color.red;
			Color color = shieldMat.color;
			color.a = 0.8f;
			shieldMat.color = color;
		}
		else if (blockHealth == 0)
		{
			isBlocking = false;

			if (timeSinceBlockBroke > 2f)
			{
				blockHealth += 3;
			}
		}
		timeSinceBlockBroke = 0;
	}
}
