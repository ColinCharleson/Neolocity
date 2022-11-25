using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth hp;
    public float maxHealth;
    public float health;
    public ParticleSystem knockBackEffect;

    public float regenSpeed;

    public PlayerController thePlayer;

    private void Awake()
    {
        if (!hp)
        {
            hp = this;
        }
    }
    void Start()
    {
        health = maxHealth;

        thePlayer = FindObjectOfType<PlayerController>();

    }
	private void FixedUpdate()
    { 

        if (health < maxHealth)
            HealthRegen();
    }
	public void HealthRegen()
	{
        health += regenSpeed * Time.deltaTime;

        if (health >= maxHealth)
            health = maxHealth;

    }
    public void DamagePlayer(int damage, Vector3 direction)
    {
        knockBackEffect.Play();
        health -= damage;

        thePlayer.KnockBack(direction);
        if(health <= 0)
        {
            health = 0;
        }
    }
}
