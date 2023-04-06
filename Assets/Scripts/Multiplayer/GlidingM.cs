using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingM : MonoBehaviour
{
    public float min = 0.5f;
    public float max = 1;

    public float glidePower = 1;

    private Rigidbody body;
    private Cube1 movement;
    public Animator anims;

    //Gliding Sound 
    public AudioSource glidingSource;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        movement = GetComponent<Cube1>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Glide input
        if (Input.GetKey(InputSystem.key.glide) && !movement.onWall)
        {
            anims.SetBool("isGliding", true);
            movement.gliding = true;
            glidePower = min;
            glidingSource.enabled = true;

            if (Input.GetKey(InputSystem.key.glide) && Input.GetKey(KeyCode.A) && !movement.isGrounded)
            {
                movement.glidingLeft = true;
            }
            else
            {
                movement.glidingLeft = false;
            }
            if (Input.GetKey(InputSystem.key.glide) && Input.GetKey(KeyCode.D) && !movement.isGrounded)
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
            anims.SetBool("isGliding", false);
            movement.glidingLeft = false;
            movement.gliding = false;
            movement.glidingRight = false;
            glidePower = max;
            glidingSource.enabled = false;
        }
    }
}
