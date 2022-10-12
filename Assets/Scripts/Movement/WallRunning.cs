using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
	public LayerMask groundMask;

	public bool wallLeft;
	public bool wallRight;
	public float wallCheckDistance;
	public float jumpForce = 10;
	public float wallRunCoolDown;

	private RaycastHit leftWallhit;
	private RaycastHit rightWallhit;

	public float fallingSpeed;

	private Transform orientation;
	private PlayerController movement;
	private void Start()
	{
		orientation = GetComponent<Transform>();
		movement = GetComponent<PlayerController>();
	}
	private void Update()
	{
		WallRun();

		if (wallRunCoolDown > 0)
			CoolDown();
	}

	void WallRun()
	{
		//Check for wall
		if (!movement.isGrounded && !movement.gliding)
		{
			wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, groundMask);
			wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, groundMask);
		}
		else
		{
			wallLeft = false;
			wallRight = false;
		}

		if (wallLeft || wallRight)
		{
			if (wallRunCoolDown <= 0)
			{
				fallingSpeed = 0.1f;
				movement.onWall = true;
				movement.gliding = false;
			}
		}
		else
		{
			movement.onWall = false;
			fallingSpeed = 1f;


		}
	}
	void CoolDown()
	{
		wallRunCoolDown -= Time.deltaTime;
	}
}

