using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	float vertical;
	float horizontal;
	public float mouseSensitivity;
	float xRotation;

	public bool isGrounded;
	public Transform groundCheck;
	public float groundDistance = 0.4f;
	public LayerMask groundMask;

	public float speed = 6f;
	public float jumpForce = 1.5f;

	private Gliding glideScript;
	public bool gliding;
	private WallRunning wallRunScript;
	public bool onWall;

	public Rigidbody body;
	Camera cam;
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
		vertical = Input.GetAxis("Vertical") * speed;
		horizontal = Input.GetAxis("Horizontal") * speed;

		//Sprint input
		if (Input.GetKey(KeyCode.LeftShift))
			speed = 9f;		//Sprint Speed
		else
			speed = 6f;		//Walk Speed

		//Adjust velocity
		body.velocity = (transform.forward * vertical) + (transform.right * horizontal) + (transform.up * body.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed);
		
		//Jump input
		if ((Input.GetAxis("Jump") > 0) && isGrounded)
		{
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

	}
}
