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
		//Keyboard inputs
		vertical = Input.GetAxis("Vertical") * currentSpeed;
		horizontal = Input.GetAxis("Horizontal") * currentSpeed;

		//Sprint input
		if (Input.GetKey(KeyCode.LeftShift))
			currentSpeed = sprintSpeed;		//Sprint Speed
		else
			currentSpeed = walkSpeed;		//Walk Speed

		//Adjust velocity
		body.velocity = (transform.forward * vertical) + (transform.right * horizontal) + (transform.up * body.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed);
		
		//Jump input
		if ((Input.GetAxis("Jump") > 0))
		{
			if(isGrounded)
			body.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
		}

		// Get Inputs
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
		float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

		xRotation -= mouseY;
		xRotation = Mathf.Clamp(xRotation, -89f, 89f);

		// Set Rotation
		cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
		this.transform.Rotate(Vector3.up * mouseX);

		// is grounded check
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (Input.GetKey(KeyCode.R))
			transform.position = new Vector3(26, 18, -1);

		//temp animations
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("WallRunRight", wallRunScript.wallRight);
		tempKasa.SetBool("WallRunLeft", wallRunScript.wallLeft);
	}
}
