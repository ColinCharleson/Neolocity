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
	private float currentSpeed;
	private float sprintSpeed = 9f;
	private float walkSpeed = 6f;
	private float jumpForce = 1.5f;

	//Advanced movement variables
	private Gliding glideScript;
	public bool gliding;
	private WallRunning wallRunScript;
	public bool onWall;

	//Story Missions
	private int currentMission =1;
	private bool isTalking;

	//Other components
	public Rigidbody body;
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
	private void FixedUpdate()
	{
		if (Input.GetKey(KeyCode.R))
			transform.position = new Vector3(26, 18, -1);

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
		vertical = Input.GetAxis("Vertical") * currentSpeed;
		horizontal = Input.GetAxis("Horizontal") * currentSpeed;

		//Sprint input
		if (Input.GetKey(KeyCode.LeftShift))
			currentSpeed = sprintSpeed;     //Sprint Speed
		else
			currentSpeed = walkSpeed;       //Walk Speed
											//Adjust velocity
		body.velocity = (transform.forward * vertical) + (transform.right * horizontal) + (transform.up * body.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed);

		//Jump input
		if ((Input.GetAxis("Jump") > 0) && isGrounded)
		{
			body.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
		}

		// is grounded check
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		//temp animations
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("WallRunRight", wallRunScript.wallRight);
		tempKasa.SetBool("WallRunLeft", wallRunScript.wallLeft);
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

	void TalkToNPC()
	{
		RaycastHit hit;

		if (Physics.Raycast(cam.transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.CompareTag("NPC"))
		{
			if (Input.GetKey(KeyCode.E))
			{
				hit.transform.gameObject.GetComponent<MissionNPC>().GiveMission(currentMission);
				isTalking = true;
				body.velocity = Vector3.zero;
			}
		}

		if(isTalking)
		{
			if (Input.GetKey(KeyCode.Q))
			{
				isTalking = false;
			}
		}
	}
}
