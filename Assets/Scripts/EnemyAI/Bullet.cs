using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public KasaAttack kasaAttack;
    public int damage;
    public Transform player;

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
               Debug.Log("Penis");

                FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
       }

       else if (collision.gameObject.tag == "Player" && kasaAttack.isBlocking)
       {
                kasaAttack.blockHealth -= 1;
                Destroy(gameObject);
       }

        else
        {
            Destroy(gameObject, 3);
        }
    }
}
