using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int damage = 1;


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            Vector3 dmgDirection = collision.transform.position - transform.position;
            dmgDirection = dmgDirection.normalized;

            FindObjectOfType<PlayerHealth>().DamagePlayer(damage, dmgDirection);
        }
    }
}