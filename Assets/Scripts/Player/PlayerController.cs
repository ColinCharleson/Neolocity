using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	// Movement and Rotation
	float vertical;
	float horizontal;
	public float mouseSensitivity = 100;
	float xRotation;

	//Ground Check
	public bool isGrounded;
	public Transform groundCheck;
	private float groundDistance = 0.4f;
	public LayerMask groundMask;

	// Player movement stats
	private float speed;
	public float sprintSpeed = 7f;
	public float walkSpeed = 5f;
	public float stamina = 100f;
	private float staminaDrain = 5f;
	public float jumpForce = 1.5f;
	public float groundDrag;
	public float airMultiplier;
	public bool isSprinting = false;
	Vector3 moveDirection;

	public bool isAlive = true;

	// Shiled
	public KasaAttack kasaAttack;

	//Knock back
	public float knockBackForce = 10;

	//Fall Damage
	bool wasGrounded;
	bool wasFalling;
	float startOfFalling;
	public float minFall = 4f;

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
	private bool isTalking;
	public GameObject speakInteractBox;
	public GameObject grabInteractBox;

	//Other components
	public Rigidbody body;
	public Transform enemy;
	Camera cam;
	public Animator tempKasa;
	public Slider slider;
	public Text sensText;
	public Slider fovSlider;
	public Text fovText;
	public float fov = 60;

	//Footsteps
	public AudioSource footstepsSource, sprintSource;

	void Start()
	{
		glideScript = GetComponent<Gliding>();
		wallRunScript = GetComponent<WallRunning>();
		body = GetComponent<Rigidbody>();
		cam = GetComponentInChildren<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		mouseSensitivity = PlayerPrefs.GetFloat("CurrentSens", 100);
		slider.value = mouseSensitivity / 10;
		fov = PlayerPrefs.GetFloat("CurrentFov", 60);
		fovSlider.value = fov;
	}

	void Update()
    {
		cam.fieldOfView = fov;
		PlayerPrefs.SetFloat("CurrentFov", fov);
		PlayerPrefs.SetFloat("CurrentSens", mouseSensitivity);
    }

	private void FixedUpdate()
    {
		//temp animations
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("WallRunRight", wallRunScript.wallRight);
		tempKasa.SetBool("WallRunLeft", wallRunScript.wallLeft);
		tempKasa.SetBool("Sprint", isSprinting);

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
		{
			if (isGrounded == true && !gliding && !onWall)
            {
				if (Input.GetKey(KeyCode.LeftShift))
                {
					footstepsSource.enabled = false;
					sprintSource.enabled = true;
					isSprinting = true;

				}
                else
                {
					footstepsSource.enabled = true;
					sprintSource.enabled = false;
					isSprinting = false;
				}
            }else
            {
				footstepsSource.enabled = false;
				sprintSource.enabled = false;
				isSprinting = false;
			}
		}
		else
		{
			footstepsSource.enabled = false;
			sprintSource.enabled = false;
			isSprinting = false;
		}

		if (Input.GetKey(KeyCode.R))
			transform.position = new Vector3(26, 18, -1);

			SpeedControl();
		TalkToNPC();
		PickingUp();

		if (!isTalking)
		{
			if (isAlive)
			{
				Movement();
				Rotation();
			}
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

		if(kasaAttack.isBlocking)
        {
			body.velocity = new Vector3(body.velocity.x * kasaAttack.blockingSpeed, body.velocity.y , body.velocity.z * kasaAttack.blockingSpeed);
		}

		if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && isGrounded)
		{
			speed = sprintSpeed;
			stamina -= staminaDrain * Time.deltaTime;
		}
		else
		{
			speed = walkSpeed;
			if (stamina < 100)
			stamina += Time.deltaTime;
		}

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

	public void ChangeSensitivity(float sensi)
    {
		mouseSensitivity = sensi * 10;
		sensText.text = mouseSensitivity.ToString("F0");
	}

	public void ChangeFov(float foV)
	{
		fov = foV;
		fovText.text = fov.ToString("F0");
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
		body.AddForce((-transform.forward * 130), ForceMode.Impulse);
	}

	void TalkToNPC()
	{
		RaycastHit hit;

		if (Physics.Raycast(cam.transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.CompareTag("NPC") && !isTalking)
		{
			speakInteractBox.SetActive(true);
			if (Input.GetKey(KeyCode.E) && !isTalking)
			{
				hit.transform.gameObject.GetComponent<MissionNPC>().TalkToNPC(MissionManager.instance.lastMission);
				isTalking = true;
				body.velocity = Vector3.zero;
			}
		}
		else
			speakInteractBox.SetActive(false);

		if (isTalking)
		{
			if (Input.GetKey(KeyCode.Q))
			{
				isTalking = false;
			}
		}
	}
	void PickingUp()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4) && hit.collider.gameObject.CompareTag("MissionEnd"))
		{
			grabInteractBox.SetActive(true);
			if (Input.GetKey(KeyCode.E))
			{
				Destroy(hit.transform.parent.parent.gameObject);
			}
		}
		else
			grabInteractBox.SetActive(false);
	}
}
