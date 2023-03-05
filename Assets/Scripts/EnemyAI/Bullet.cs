using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public KasaAttack kasaAttack;
    public int damage;
    public int reflectDamage;
    public Transform player;
    public Rigidbody laser;
    public bool deflected = false;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        kasaAttack = player.GetComponent<KasaAttack>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !kasaAttack.isBlocking)
        {
               Vector3 dmgDirection = collision.transform.position - transform.position;
               dmgDirection = dmgDirection.normalized;
               FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
               Destroy(gameObject);
        }

      else if (collision.gameObject.tag == "Player" && kasaAttack.isBlocking)
        {
            kasaAttack.blockHealth -= 2;
            kasaAttack.blockHealth = 0;
            Vector3 aimShot = Camera.main.transform.forward;
            float mag = laser.velocity.magnitude;
            laser.transform.rotation = Quaternion.LookRotation(aimShot);
            laser.velocity = aimShot * mag;
            laser.transform.position = transform.position;
            deflected = true;
        }

       else if (collision.gameObject.tag == "ProjectileEnemy" && deflected == true)
        {
            collision.GetComponent<ProjectileEnemyAI>().TakeDamage(reflectDamage);
            Destroy(gameObject);
        }

        else if(collision.gameObject.tag == "FlyingProjectileEnemy" && deflected == true)
        {
            collision.GetComponent<FlyingProjectileEnemyAI>().TakeDamage(reflectDamage);
            Destroy(gameObject);
        }

       else if (collision.gameObject.tag == "Enemy" && deflected == true)
        {
            collision.GetComponent<EnemyAI>().TakeDamage(reflectDamage);
            Destroy(gameObject);
        }

        else
        {
            Destroy(gameObject, 3);
            deflected = false;
        }
    }
}
