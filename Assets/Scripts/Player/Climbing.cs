using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
	public bool onLadder;
	public float climbingSpeed = 0.1f;

	private Rigidbody rb;

	public AudioSource ladderSource;
	private void Start()
	{
		rb = this.GetComponent<Rigidbody>();
	}
	private void FixedUpdate()
	{
		if (onLadder)
		{
			if(Input.GetKey(KeyCode.W))
			{
				rb.position += Vector3.up * climbingSpeed;
				ladderSource.enabled = true;
			}
			else
			{
				rb.velocity = Vector3.zero;
				ladderSource.enabled=false;
			}


			rb.useGravity = false;
		}
		else
		{
			rb.useGravity = true;
			ladderSource.enabled = false;
		}
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ladder")
		{
			onLadder = true;
		}
	}
	private void OnCollisionExit(Collision collision)
	{
		if (collision.gameObject.tag == "Ladder")
		{
			onLadder = false;
		}
	}
}
