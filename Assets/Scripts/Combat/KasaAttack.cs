using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaAttack : MonoBehaviour
{
    public GameObject Kasa;
    public bool CanAttack = true;
    public float AttackCooldown = 2.0f;

    private PlayerController movement;

	private void Start()
    {
        movement = GetComponent<PlayerController>();
    }
	void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanAttack && !movement.onWall && !movement.gliding)
            {
                Attack();
            }
        }
    }


    public void Attack()
    {
        CanAttack = false;
        Animator animation = Kasa.GetComponent<Animator>();
        animation.SetTrigger("Attack");
        StartCoroutine(AttackCooldownReset());
    }

    IEnumerator AttackCooldownReset()
    {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }
}
