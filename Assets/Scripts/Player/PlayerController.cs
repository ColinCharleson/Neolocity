using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

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
	public bool sprintLock = false;
	Vector3 moveDirection;

	public bool isAlive = true;

	//Vignette
	private Volume postProcessVolume;
	private Vignette vignette;
	bool clearVignette = false;

	// Shiled
	public KasaAttack kasaAttack;

	//Lock On
	public float sightRange;
	public bool enemyInSightRange;
	public LayerMask whatIsEnemy;

	//Advanced movement variables
	private Gliding glideScript;
	public bool gliding = false;
	public bool glidingLeft = false;
	public bool glidingRight = false;
	private WallRunning wallRunScript;
	public bool onWall = false;

	//Story Missions
	private bool isTalking;
	public GameObject speakInteractBox;
	public GameObject grabInteractBox;

	//Other components
	public Rigidbody body;
	Camera cam;
	public Animator tempKasa;
	public Slider slider;
	public Text sensText;
	public Slider fovSlider;
	public Text fovText;
	public float fov = 60;
	public KasaAttack attack;

	//Footsteps
	public AudioSource footstepsSource, sprintSource, wallRunningSource, ladderSource, boostSource, fallDamageSource, outOfBreathSource;

	//Upgradeable Skill Points
	public float speedSP = 1.0f;
	public float damageSP = 1.0f;
	public float jumpSP = 1.0f;
	public float boostSP = 1.0f;
	public float healthSP = 1.0f;

	public int scrap;
	public Text scrapText;
	public GameObject scrapFade;

	void Start()
	{
		attack = GetComponent<KasaAttack>();
		glideScript = GetComponent<Gliding>();
		wallRunScript = GetComponent<WallRunning>();
		body = GetComponent<Rigidbody>();
		cam = GetComponentInChildren<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		mouseSensitivity = PlayerPrefs.GetFloat("CurrentSens", 100);
		slider.value = mouseSensitivity / 10;
		fov = PlayerPrefs.GetFloat("CurrentFov", 60);
		fovSlider.value = fov;
		postProcessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
		postProcessVolume.profile.TryGet<Vignette>(out vignette);
	}

	void Update()
	{
		PlayerPrefs.SetFloat("CurrentSens", mouseSensitivity);
	}

	private void FixedUpdate()
	{
		//temp animations
		tempKasa.SetBool("Gliding", gliding);
		tempKasa.SetBool("FlyLeft", glidingLeft);
		tempKasa.SetBool("FlyRight", glidingRight);
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
			body.AddForce(moveDirection.normalized * speed * speedSP * 10f, ForceMode.Force);
		else
			body.AddForce(moveDirection.normalized * speed * speedSP * 10f * airMultiplier, ForceMode.Force);

		body.velocity = new Vector3(body.velocity.x, body.velocity.y * glideScript.glidePower * wallRunScript.fallingSpeed, body.velocity.z);

		if (kasaAttack.isBlocking)
		{
			body.velocity = new Vector3(body.velocity.x * kasaAttack.blockingSpeed, body.velocity.y, body.velocity.z * kasaAttack.blockingSpeed);
		}

		if (Input.GetKey(InputSystem.key.sprint) && stamina > 0 && isGrounded && !sprintLock && !attack.attackLock)
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
			tempKasa.SetTrigger("Exhaust");
			sprintLock = true;
			clearVignette = false;
			outOfBreathSource.enabled = true;
		}

		if (stamina >= 100)
		{
			sprintLock = false;
			outOfBreathSource.enabled = false;
		}

		if (sprintLock)
		{
			vignette.color.Override(Color.black);

			if (vignette.intensity.value >= 0.4f)
			{
				clearVignette = true;
			}

			if (clearVignette)
			{
				vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.0f, Time.deltaTime * 0.1f);
			}
			else
			{
				vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.41f, Time.deltaTime * 2);
			}
		}
		else
		{
			vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.0f, Time.deltaTime);
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
				body.AddForce(transform.up * jumpForce * jumpSP, ForceMode.Impulse);
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
		cam.fieldOfView = fov;
		PlayerPrefs.SetFloat("CurrentFov", fov);
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
				if (hit.transform.gameObject.GetComponent<MissionNPC>() != null)
				{
					hit.transform.gameObject.GetComponent<MissionNPC>().TalkToNPC(MissionManager.instance.lastMission);
				}
				isTalking = true;
				body.velocity = Vector3.zero;
			}
		}
		else if (Physics.Raycast(cam.transform.position, transform.forward, out hit, 3) && hit.collider.gameObject.CompareTag("Scrap Dealer") && !isTalking)
		{
			speakInteractBox.SetActive(true);
			if (Input.GetKey(InputSystem.key.interact) && !isTalking)
			{
				if (hit.transform.gameObject.GetComponent<ScrapDealer>() != null)
				{
					hit.transform.gameObject.GetComponent<ScrapDealer>().TalkToNPC(scrap);
				}
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
				if (hit.transform.GetComponent<Scrap>() != null)
				{
					scrapFade.SetActive(true);
					scrap += 1;
					Destroy(hit.transform.gameObject);
					StartCoroutine(ResetText());
					scrapText.text = "Scrap + 1 ";
				}
				else
				{
					Destroy(hit.transform.parent.parent.gameObject);
				}
			}
		}
		else if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 4) && hit.collider.gameObject.CompareTag("Chest"))
		{
			grabInteractBox.SetActive(true);
			if (Input.GetKey(InputSystem.key.interact))
			{
				scrapFade.SetActive(true);
				int scrapToAdd = Random.Range(1, 5);
				scrap += scrapToAdd;
				scrapText.text = "Scrap + " + scrapToAdd;
				Destroy(hit.transform.gameObject);
				StartCoroutine(ResetText());
			}
		}
		else
		{
			grabInteractBox.SetActive(false);
		}
	}
	IEnumerator ResetText()
	{
		yield return new WaitForSeconds(0.6f);
		scrapFade.SetActive(false);
	}
}
