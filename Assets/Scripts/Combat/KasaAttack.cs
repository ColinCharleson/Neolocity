using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KasaAttack : MonoBehaviour
{
    public GameObject Kasa;
    public bool CanAttack = true;
    public float AttackCooldown = 2.0f;

    void update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (CanAttack)
            {
                kasaAttack();
            }
        }
    }


    public void kasaAttack()
    {
        CanAttack = false;
        Animator animation = Kasa.GetComponent<Animator>();
        animation.SetTrigger("Attack");
        StartCoroutine(attackcooldownReset());
    }

    IEnumerator attackcooldownReset()
    {
        yield return new WaitForSeconds(AttackCooldown);
        CanAttack = true;
    }
}
