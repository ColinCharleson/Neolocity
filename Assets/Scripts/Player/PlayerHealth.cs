using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth hp;
    public float maxHealth = 100;
    public float health = 100;
    public ParticleSystem knockBackEffect;

    public float regenSpeed;

    public PlayerController thePlayer;

    private void Awake()
    {
        if (!hp)
        {
            hp = this;
        }
		else
		{
            Destroy(gameObject);
		}
    }
    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
    }
	private void FixedUpdate()
    { 

        if (health < maxHealth && health > 0)
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
