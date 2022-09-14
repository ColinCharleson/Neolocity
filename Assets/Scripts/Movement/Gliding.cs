using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gliding : MonoBehaviour
{
    public float glidePower = 1;

    private Rigidbody body;
    private PlayerController movement;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Glide input
        if (Input.GetKey(KeyCode.LeftControl) && !movement.onWall)
		{
            movement.gliding = true;
            glidePower = 0.5f;
		}
        else
        {
            movement.gliding = false;
            glidePower = 1f;
		}
    }
}
