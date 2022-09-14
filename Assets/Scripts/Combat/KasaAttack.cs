using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaAttack : MonoBehaviour
{
    public Animator kasa;
    public bool canAttack = true;
    public float attackCooldown = 0.5f;

    public float timeSinceLastHit;
    public float lastAttack;

    private PlayerController movement;

	private void Start()
    {
        movement = GetComponent<PlayerController>();
    }
	void Update()
    {
        timeSinceLastHit += Time.deltaTime;

        if (Input.GetMouseButtonDown(0))
        {
            if (canAttack && !movement.onWall && !movement.gliding)
            {
                Attack();
            }
        }
    }


    public void Attack()
    {
        canAttack = false;

        if (timeSinceLastHit > 1.3f)
		{
            kasa.SetTrigger("Attack");
            lastAttack = 1;
            attackCooldown = 0.5f;
            
        }
        else
		{
            if(lastAttack == 2)
			{
                kasa.SetTrigger("Attack3");
                lastAttack = 3;
                attackCooldown = 2.0f;
            }
            if (lastAttack == 1)
            {
                kasa.SetTrigger("Attack2");
                lastAttack = 2;
                attackCooldown = 0.5f;
            }

		}

        timeSinceLastHit = 0;

        StartCoroutine(AttackCooldownReset());
    }

    IEnumerator AttackCooldownReset()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
