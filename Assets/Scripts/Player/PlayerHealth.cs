using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth hp;
    public float maxHealth = 100;
    public float health = 100;
    public ParticleSystem knockBackEffect;

    public float regenSpeed;

    //Vignette
    private Volume postProcessVolume;
    private Vignette vignette;

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
        maxHealth = thePlayer.healthSP * 100;
        postProcessVolume = GameObject.Find("Global Volume").GetComponent<Volume>();
        postProcessVolume.profile.TryGet<Vignette>(out vignette);
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
        {
            health = maxHealth;
        }

        if (health <= 50)
        {
            vignette.color.Override(Color.red);

            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.4f, Time.deltaTime * 2);

            if (health <= 30)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.6f, Time.deltaTime * 2);
            }
            if (health <= 25)
            {
                vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.8f, Time.deltaTime * 2);
            }
        }
        else 
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.0f, Time.deltaTime * 2);
        }

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
