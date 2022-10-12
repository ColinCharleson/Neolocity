using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaAttack : MonoBehaviour
{
    public Animator kasa;
    public bool isAttacking = false;
    public bool canAttack = true;
    public float attackCooldown = 0.5f;
    public float swingDamage = 1f;

    public float timeSinceLastHit;
    public float lastAttack;

    private PlayerController movement;
    public BoxCollider weaponHitBox;

    public Rigidbody enemy;


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

        isAttacking = true;

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
        StartCoroutine(ResetAttack());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    IEnumerator ResetAttack()
    {

        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && isAttacking)
        {
            if (isAttacking)
                other.GetComponent<EnemyAI>().TakeDamage(swingDamage);
        }
    }



}
