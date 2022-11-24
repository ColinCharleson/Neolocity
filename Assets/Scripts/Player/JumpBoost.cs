using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoost : MonoBehaviour
{
    PlayerController player;

    public bool canJumpBoost;

    public float cooldownLength;
    public float timeLeft;

    public float boostForce;

	private void Start()
	{
        player = GetComponent<PlayerController>();
	}
	void Update()
    {
        /*if(player.isGrounded)
		{
            canJumpBoost = true;
		}*/

        if(canJumpBoost)
        {
            if(Input.GetKeyDown(KeyCode.E))
			{
                player.body.velocity = Vector3.zero;
                player.gliding = false;
                player.onWall = false;

                player.body.AddForce(transform.up * boostForce, ForceMode.Impulse);
                timeLeft = cooldownLength;
                canJumpBoost = false;
            }
        }

        if(timeLeft <= 0)
        {
            canJumpBoost = true;
        }
		else
		{
            timeLeft -= Time.deltaTime;
		}
    }
}
