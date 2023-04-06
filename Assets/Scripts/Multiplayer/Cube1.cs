using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Cube1 : MonoBehaviour
{
	public Rigidbody rb;

	// Movement and Rotation
	float vertical;
	float horizontal;
	public float mouseSensitivity = 100;
	float xRotation;
	public Camera cam;
	public float fov = 60;

	//Ground Check
	public bool isGrounded;
	public Transform groundCheck;
	private float groundDistance = 0.4f;
	public LayerMask groundMask;

	// Player movement stats
	private float speed;
	private float sprintSpeed = 8f;
	private float walkSpeed = 6f;
	public float stamina = 100f;
	private float staminaDrain = 5f;
	private float jumpForce = 3f;
	private float groundDrag = 5f;
	private float airMultiplier = 0.2f;
	public bool isSprinting = false;
	public bool sprintLock = false;
	Vector3 moveDirection;

	//Advanced movement variables
	private GlidingM glideScript;
	public bool gliding = false;
	public bool glidingLeft = false;
	public bool glidingRight = false;
	private WallRunningM wallRunScript;
	public bool onWall = false;

	public bool isAlive = true;

	public Animator tempKasa;

	public bool textChatting;
	public Canvas chatBox;
	public Canvas pauseUI;
	// Start is called before the first frame update
	void Awake()
	{
		glideScript = GetComponent<GlidingM>();
		wallRunScript = GetComponent<WallRunningM>();
		rb = GetComponent<Rigidbody>();
		mouseSensitivity = PlayerPrefs.GetFloat("CurrentSens", 100);
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("FlyLeft", glidingLeft);
		tempKasa.SetBool("FlyRight", glidingRight);
		tempKasa.SetBool("WallRunRight", wallRunScript.wallRight);
		tempKasa.SetBool("WallRunLeft", wallRunScript.wallLeft);


		if (textChatting == false && pauseUI.enabled == false)
		{
			if (isAlive)
			{
				Movement();
				Rotation();
			}
			SpeedControl();

			if (this.enabled)
				Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Confined;
			rb.velocity = Vector3.zero;
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
			rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
		else
			rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);

		rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed, rb.velocity.z);

		if (Input.GetKey(InputSystem.key.sprint) && stamina > 0 && isGrounded && !sprintLock)
		{
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov + 10, 10f * Time.deltaTime);
			isSprinting = true;
			speed = sprintSpeed;
			stamina -= staminaDrain * Time.deltaTime;
		}
		else
		{
			cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, 10f * Time.deltaTime);
			isSprinting = false;
			speed = walkSpeed;
			if (stamina < 100)
				stamina += 10 * Time.deltaTime;
		}

		if (stamina <= 0)
		{
			sprintLock = true;
		}

		if (stamina >= 100)
		{
			sprintLock = false;

		}

		if (Input.GetKey(InputSystem.key.jump))
		{
			if (isGrounded)
			{
				rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
			}

			if (onWall)
			{
				wallRunScript.wallRunCoolDown = 0.5f;

				if (wallRunScript.wallLeft)
					rb.AddForce((transform.up * 5 + transform.right * 8) * jumpForce, ForceMode.Impulse);

				if (wallRunScript.wallRight)
					rb.AddForce((transform.up * 5 + -transform.right * 8) * jumpForce, ForceMode.Impulse);
			}
		}

		// handle drag
		if (isGrounded)
		{
			rb.drag = groundDrag;
		}
		else
			rb.drag = 0;

		// is grounded check
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
	}
	private void SpeedControl()
	{
		Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

		// limit velocity if needed
		if (flatVel.magnitude > speed)
		{
			Vector3 limitedVel = flatVel.normalized * speed;
			rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
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

}
