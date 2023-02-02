using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour
{
    public float min = 0.5f;
    public float max =1;

    public float glidePower = 1;

    private Rigidbody body;
    private PlayerController movement;
    private FallDamage fallDamage;

    //Gliding Sound 
    public AudioSource glidingSource;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerController>();
        fallDamage = GetComponent<FallDamage>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Glide input
        if (Input.GetKey(InputSystem.key.glide) && !movement.onWall)
		{
            movement.gliding = true;
            fallDamage.gliding = true;
            glidePower = min;
            glidingSource.enabled = true;

            if (Input.GetKey(InputSystem.key.glide) && Input.GetKey(KeyCode.A) & !movement.onWall)
            {
                movement.glidingLeft = true;
            }
            else
            {
                movement.glidingLeft = false;
            }
            if (Input.GetKey(InputSystem.key.glide) && Input.GetKey(KeyCode.D) & !movement.onWall)
            {
                movement.glidingRight = true;
            }
            else
            {
                movement.glidingRight = false;
            }
        }

        else
        {
            movement.glidingLeft = false;
            movement.gliding = false;
            movement.glidingRight = false;
            fallDamage.gliding = false;
            glidePower = max;
            glidingSource.enabled = false;
		}
    }
}
