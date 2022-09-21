using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
	public LayerMask groundMask;

	public  bool wallLeft;
	public bool wallRight;
	public float wallCheckDistance;
	public float jumpUpForce = 10;
	public float jumpSideForce = 10;

	public KeyCode jumpKey = KeyCode.Space;



	private RaycastHit leftWallhit;
	private RaycastHit rightWallhit;



	public float fallingSpeed;

	private Transform orientation;
	private PlayerController movement;
	private Camera cam;
	public Rigidbody rb;
	private void Start()
	{
		orientation = GetComponent<Transform>();
		movement = GetComponent<PlayerController>();
		cam = GetComponentInChildren<Camera>();
	}
	private void FixedUpdate()
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

		if (wallLeft|| wallRight)
		{
			fallingSpeed = 0.1f;
			movement.onWall = true;
			movement.gliding = false;


			if (Input.GetKeyDown(jumpKey))
			{
				WallJump();

			}
		}
		

	
		else
		{
			movement.onWall = false;
			fallingSpeed = 1f;
		}

        
	}

	private void WallJump()
    {
		Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

		Vector3 forceToApply = transform.up * jumpUpForce + wallNormal * jumpSideForce;

		//adding force
		rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
		rb.AddForce(forceToApply, ForceMode.Impulse);
	

	}
}

