using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{

    public float force = 1;

    private void OnTriggerEnter(Collider push)
    {
        if (push.gameObject.tag == "Enemy")
        {

            Vector3 kockDirection = push.transform.position - transform.position;

            kockDirection =-kockDirection.normalized;

            GetComponent<Rigidbody>().AddForce(kockDirection * force * 100);
        }
    }
}
