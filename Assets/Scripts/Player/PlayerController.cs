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
	private float sprintSpeed = 8f;
	private float walkSpeed = 6f;
	public float stamina = 100f;
	private float staminaDrain = 5f;
	private float jumpForce = 3f;
	private float groundDrag = 5f;
	private float airMultiplier = 0.2f;
	public bool isSprinting = false;
	private bool sprintLock = false;
	Vector3 moveDirection;

	public bool isAlive = true;

	// Shiled
	public KasaAttack kasaAttack;

	//Knock back
	private float knockBackForce = 10;

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

		if (Application.isEditor)
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

		if (Input.GetKey(InputSystem.key.sprint) && stamina > 0 && isGrounded && !sprintLock)
		{
			isSprinting = true;
			speed = sprintSpeed;
			stamina -= staminaDrain * Time.deltaTime;
		}
		else
		{
			isSprinting = false;
			speed = walkSpeed;
			if (stamina < 100)
			stamina += 7 * Time.deltaTime;
		}

		if(stamina <= 0)
		{
			sprintLock = true;
		}
		if(stamina >= 100)
		{
			sprintLock = false;
		}

		if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.S)) || (Input.GetKey(KeyCode.D)))
		{
			if (isGrounded == true && !gliding && !onWall)
			{
				if (Input.GetKey(InputSystem.key.sprint))
				{
					footstepsSource.enabled = false;
					sprintSource.enabled = true;

				}
				else
				{
					footstepsSource.enabled = true;
					sprintSource.enabled = false;
				}
			}
			else
			{
				footstepsSource.enabled = false;
				sprintSource.enabled = false;
			}
		}
		else
		{
			footstepsSource.enabled = false;
			sprintSource.enabled = false;
		}

		if (Input.GetKey(InputSystem.key.jump))
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
			if (Input.GetKey(InputSystem.key.interact) && !isTalking)
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
			if (Input.GetKey(InputSystem.key.cancelTalk))
			{
				isTalking = false;
			}
		}
	}
	void PickingUp()
	{
		RaycastHit hit;
		if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4) && hit.collider.gameObject.CompareTag("Grabbable"))
		{
			grabInteractBox.SetActive(true);
			if (Input.GetKey(InputSystem.key.interact))
			{
				Destroy(hit.transform.parent.parent.gameObject);
			}
		}
		else
			grabInteractBox.SetActive(false);
	}
}
