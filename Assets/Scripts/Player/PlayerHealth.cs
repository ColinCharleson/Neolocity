using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public PlayerController thePlayer;

    void Start()
    {
        health = maxHealth;

        thePlayer = FindObjectOfType<PlayerController>();
    }

    public void DamagePlayer(int damage, Vector3 direction)
    {
        health -= damage;

        thePlayer.KnockBack(direction);
        if(health == 0)
        {
            this.transform.position = new Vector3(26, 18, -1);
            health += 100;
        }
    }
}
