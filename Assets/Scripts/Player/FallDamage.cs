using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{

	//Ground Check
	public bool isGrounded;
	public Transform groundCheck;
	public LayerMask groundMask;
	private float groundDistance = 0.4f;

	//Fall Damage
	bool wasGrounded;
	bool wasFalling;
	public float startOfFalling;
	public float minFall = 4f;

	//Advanced movement variables
	private Gliding glideScript;
	public bool gliding = false;
	private WallRunning wallRunScript;
	public bool onWall = false;

	//Other components
	public Animator tempKasa;
	public Rigidbody body;
	public PlayerHealth playerHealth;

	public AudioSource fallDamageSource;
	private void Update()
	{
		//checking if take fall damage
		GroundCheck();

		if (gliding || onWall)
		{
			startOfFalling = transform.position.y;
		}

		if (!wasFalling && isFalling)
		{
			startOfFalling = transform.position.y;
		}

		if (!wasGrounded && isGrounded && !gliding)
		{
			TakeDamage();
			fallDamageSource.enabled = true;
			
		}
		
		wasGrounded = isGrounded;
		wasFalling = isFalling;
		
	}

	public void TakeDamage()
	{
		float fallDistance = startOfFalling - transform.position.y;

		if (fallDistance > minFall)
		{
			tempKasa.SetTrigger("Shake");
			playerHealth.health -= fallDistance * 3;
			if (!fallDamageSource.isPlaying)
			{
			fallDamageSource.enabled = false;
			}

			if (PlayerHealth.hp.health <= 0)
			{
				PlayerHealth.hp.health = 0;
			}
		}
	}

	bool isFalling
	{
		get
		{
			return (!isGrounded && body.velocity.y < 0);
		}
	}

	void GroundCheck()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
	}



}
