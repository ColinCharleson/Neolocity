using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    void Start()
    {
        health = maxHealth;
    }

    public void DamagePlayer(int damage, Vector3 direction)
    {
        health -= damage;
    }
}
