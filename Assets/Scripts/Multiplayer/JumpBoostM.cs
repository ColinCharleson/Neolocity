using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBoostM : MonoBehaviour
{
   private Cube1 player;
    public GameObject jumpBoostIcon;
    public ParticleSystem boostEffect;

    public bool canJumpBoost;

    public float cooldownLength;
    public float timeLeft;

    public float boostForce;

    public AudioSource boostSource;

    private void Start()
    {
        player = GetComponent<Cube1>();
    }
    void Update()
    {
        if (canJumpBoost)
            jumpBoostIcon.SetActive(true);

        else
            jumpBoostIcon.SetActive(false);

        if (canJumpBoost)
        {
            if (Input.GetKeyDown(InputSystem.key.boost))
            {
                player.rb.velocity = Vector3.zero;
                player.gliding = false;
                player.onWall = false;
                boostEffect.Play();

                player.rb.AddForce(transform.up * boostForce, ForceMode.Impulse);
                timeLeft = cooldownLength;
                canJumpBoost = false;
                boostSource.enabled = true;
            }
        }

        if (timeLeft <= 0)
        {
            canJumpBoost = true;
            boostSource.enabled = false;
        }
        else
        {
            timeLeft -= Time.deltaTime;
        }
    }
}
