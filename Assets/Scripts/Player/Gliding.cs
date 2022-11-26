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
        if (Input.GetKey(KeyCode.LeftControl) && !movement.onWall)
		{
            movement.gliding = true;
            fallDamage.gliding = true;
            glidePower = min;
		}
        else
        {
            movement.gliding = false;
            fallDamage.gliding = false;
            glidePower = max;
		}
    }
}
