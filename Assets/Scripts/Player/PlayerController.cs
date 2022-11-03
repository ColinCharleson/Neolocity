using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	// Movement and Rotation
	float vertical;
	float horizontal;
	public float mouseSensitivity;
	float xRotation;

	//Ground Check
	public bool isGrounded;
	public Transform groundCheck;
	private float groundDistance = 0.4f;
	public LayerMask groundMask;

	// Player movement stats
	public float speed = 7f;
	public float jumpForce = 1.5f;
	public float groundDrag;
	public float airMultiplier;
	Vector3 moveDirection;

	// Shiled


	//Knock back
	public float knockBackForce = 10;

	//Lock On
	public float sightRange;
	public bool enemyInSightRange;
	public LayerMask whatIsEnemy;

	//Advanced movement variables
	private Gliding glideScript;
	public bool gliding = false;
	private WallRunning wallRunScript;
	public bool onWall = false;

	//Story Missions
	public int lastMission = 0;
	private bool isTalking;

	//Other components
	public Rigidbody body;
	public Transform enemy;
	Camera cam;
	public Animator tempKasa;
	void Start()
	{
		

		glideScript = GetComponent<Gliding>();
		wallRunScript = GetComponent<WallRunning>();
		body = GetComponent<Rigidbody>();
		cam = GetComponentInChildren<Camera>();

		Cursor.lockState = CursorLockMode.Locked;
	}
	private void Update()
	{

		enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);

		//temp animations
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("WallRunRight", wallRunScript.wallRight);
		tempKasa.SetBool("WallRunLeft", wallRunScript.wallLeft);

		if (Input.GetKey(KeyCode.R))
			transform.position = new Vector3(26, 18, -1);

			SpeedControl();
		TalkToNPC();
		if (!isTalking)
		{
			Movement();
			Rotation();
		}
	}

	void Movement()
	{
		//Keyboard inputs
		vertical = Input.GetAxisRaw("Vertical") * speed; 
		horizontal = Input.GetAxisRaw("Horizontal") * speed;


		// calculate movement direction
		moveDirection = this.transform.forward * vertical + this.transform.right * horizontal;

		if (isGrounded)
			body.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
		else 
			body.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

		body.velocity = new Vector3(body.velocity.x, body.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed, body.velocity.z);

		if ((Input.GetAxis("Jump") > 0))
		{
			if (isGrounded)
			{
				body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
			}

			if (onWall)
			{
				wallRunScript.wallRunCoolDown = 0.5f;

				if (wallRunScript.wallLeft)
					body.AddForce((transform.up * 5 + transform.right * 8) * jumpForce, ForceMode.Impulse);

				if (wallRunScript.wallRight)
					body.AddForce((transform.up * 5 + -transform.right * 8) * jumpForce, ForceMode.Impulse);
			} 
		}

		// handle drag
		if (isGrounded)
		{
			body.drag = groundDrag;
		}
		else
			body.drag = 0;

		// is grounded check
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
	}
	private void SpeedControl()
	{
		Vector3 flatVel = new Vector3(body.velocity.x, 0f, body.velocity.z);

		// limit velocity if needed
		if (flatVel.magnitude > speed)
		{
			Vector3 limitedVel = flatVel.normalized * speed;
			body.velocity = new Vector3(limitedVel.x, body.velocity.y, limitedVel.z);
		}
	}


	void Rotation()
	{
		// Get Inputs
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -89f, 89f);

		// Set Rotation
		cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
		this.transform.Rotate(Vector3.up * mouseX);
	}


	public void KnockBack(Vector3 direction)
	{
		body.AddForce((transform.up * 3), ForceMode.Impulse);
		body.AddForce((-transform.forward * 60), ForceMode.Impulse);

	}


	void TalkToNPC()
	{
		RaycastHit hit;

		if (Physics.Raycast(cam.transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.CompareTag("NPC"))
		{
			if (Input.GetKey(KeyCode.E) && !isTalking)
			{
				hit.transform.gameObject.GetComponent<MissionNPC>().TalkToNPC(lastMission);
				isTalking = true;
				body.velocity = Vector3.zero;
			}
		}

		if (isTalking)
		{
			if (Input.GetKey(KeyCode.Q))
			{
				isTalking = false;
			}
		}
	}
}
